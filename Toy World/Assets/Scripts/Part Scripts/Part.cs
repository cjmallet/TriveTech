using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The core for every type of part is inherited from here.
/// </summary>
public abstract class Part : MonoBehaviour
{
    [NamedListAttribute(new string[] { "Right", "Left", "Top", "Bottom", "Back", "Front" })]

    [Header("Attachable Points")]
    public List<Part> attachedParts = new List<Part>();

    public List<bool> attachablePoints = new List<bool>();

    public int health;
    public float weight;
    public string description;

    public int Width { get; }
    public int Height { get; }

    [HideInInspector] public bool floodFilled;

    //! Arrow object/mesh that is used to indicate the front direction
    public bool useDirectionIndicator;
    private GameObject directionIndicatorPrefab;
    private GameObject myDirectionIndicator;
    private ParticleSystem destructionParticles;
    private Material particleMaterial;

    //! Colliders to switch between
    public Collider playModeCollider, buildModeCollider;

    private const int SIDES = 6;

    public Part()
    {
        for (int i = 0; i < SIDES; i++)
        {
            attachedParts.Add(null);
            attachablePoints.Add(true);
        }
    }

    public virtual void Awake()
    {
        if (useDirectionIndicator && myDirectionIndicator == null)
        {
            ShowFrontDirection();
        }

        //Get the components for the destruction particles
        if (TryGetComponent(out ParticleSystem particles))
        {
            destructionParticles = particles;
            particleMaterial = GetComponentInChildren<MeshRenderer>().material;
            GetComponent<ParticleSystemRenderer>().material = particleMaterial;
        }
    }

    /// <summary>
    /// This attaches the other part to this part and vice versa, in the right direction slots.
    /// Originally made to attach to a single part, based on the raycast normal of the clicked part.
    /// </summary>
    /// <param name="partToAttachTo">Part that's selected to connect to.</param>
    /// <param name="side">Side that's connecting.</param>
    public void AttachPart(Part partToAttachTo, Vector3 hitNormal)
    {
        int side = (int)DetermineSide(-hitNormal);
        int sideOpposite = (int)partToAttachTo.DetermineSide(hitNormal);

        if (partToAttachTo.attachablePoints[sideOpposite] && attachablePoints[side])
        {
            attachedParts[side] = partToAttachTo;
            partToAttachTo.attachedParts[sideOpposite] = this;
        }
    }

    public enum Orientation : int
    {
        Right,
        Left,
        Top,
        Bottom,
        Back,
        Front
    };

    /// <summary>
    /// Determines the side of the part that was clicked based on the hit normal. 
    /// The normal is
    /// </summary>
    /// <param name="normal">Hit normal</param>
    /// <returns></returns>
    public Orientation DetermineSide(Vector3 normal)
    {
        Vector3Int n = Vector3Int.RoundToInt(Quaternion.Inverse(transform.rotation) * normal);

        switch (n)
        {
            case Vector3Int v when v.Equals(Vector3Int.up):
                return Orientation.Top;
            case Vector3Int v when v.Equals(Vector3Int.left):
                return Orientation.Left;
            case Vector3Int v when v.Equals(Vector3Int.back):
                return Orientation.Back;
            case Vector3Int v when v.Equals(Vector3Int.forward):
                return Orientation.Front;
            case Vector3Int v when v.Equals(Vector3Int.down):
                return Orientation.Bottom;
            case Vector3Int v when v.Equals(Vector3Int.right):
                return Orientation.Right;

            default:
                Debug.LogWarning($"NORMAL IS INCORRECT - {n}");
                return Orientation.Right;//Not actually right at all
        }
    }

    /// <summary>
    /// Check if the side that is being attached has a working attachpoint for both blocks
    /// </summary>
    /// <param name="hitNormal">Where the block was hit</param>
    /// <returns></returns>
    public bool CheckIfAttachable(Vector3 hitNormal)
    {
        int side = (int)DetermineSide(hitNormal);
        if (attachablePoints[side] && attachedParts[side] == null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Instantiates an arrow to indicate the direction this part is facing.
    /// </summary>
    public void ShowFrontDirection()
    {
        directionIndicatorPrefab = Resources.Load("DirectionIndicationArrow") as GameObject;
        myDirectionIndicator = Instantiate(directionIndicatorPrefab, this.transform.position, this.transform.rotation);
        myDirectionIndicator.transform.Translate(0, 0, 1, Space.Self);
        myDirectionIndicator.transform.Rotate(90, 0, 0);
        myDirectionIndicator.transform.SetParent(this.transform);
    }

    public void ToggleDirectionIndicator(bool visible)
    {
        myDirectionIndicator.SetActive(visible);
    }

    /// <summary>
    /// Handle the collision of this specific part
    /// </summary>
    /// <param name="collider">Object that the part collided with</param>
    public virtual void HandleCollision(Collider collider)
    {
        if (collider.name.Contains("Enemy"))
        {
            TakeDamage(collider.gameObject.GetComponent<MoveObstacle>().damage, collider);
        }

        if (collider.name.Contains("Projectile"))
        {
            TakeDamage(collider.gameObject.GetComponent<Projectile>().damage, collider);
        }
    }

    public virtual void TakeDamage(int damage, Collider collider)
    {
        if (gameObject.TryGetComponent(out CorePart core))
        {
            return;
        }
        if (health - damage > 0)
        {
            this.health -= damage;
        }
        else
        {
            this.health = 0;
            RemovePart(true);
        }
        destructionParticles.Play();
    }

    /// <summary>
    /// Remove the part from the vehicle 
    /// either after taking damage or after floodfill check
    /// </summary>
    /// <param name="start"></param>
    public void RemovePart(bool start)
    {
        if (gameObject.TryGetComponent(out CorePart core))
        {
            return;
        }

        //Remove the attached parts from its list and itself from the other parts
        for (int x = 0; x < attachedParts.Count; x++)
        {
            if (attachedParts[x] != null)
            {
                if (x % 2 == 0)
                {
                    attachedParts[x].attachedParts[x + 1] = null;
                    attachedParts[x] = null;
                }
                else
                {
                    attachedParts[x].attachedParts[x - 1] = null;
                    attachedParts[x] = null;
                }
            }
        }
        //Switch colliders for wheels as wheelcolliders are not very smart while off the vehicle
        if (gameObject.TryGetComponent(out MovementPart movePart))
        {
            movePart.SwitchColliders();
        }

        if (GameManager.Instance.vehicleEditor.coreBlockPlayMode.GetComponent<PartGrid>() != null)
        {
            //Remove the part from the partGrid
            GameManager.Instance.vehicleEditor.coreBlockPlayMode.GetComponent<PartGrid>().
                    RemovePart(Vector3Int.CeilToInt(transform.localPosition));

            //Start the floodfill algorithm if this was the first part
            if (!transform.CompareTag("CoreBlock") && start)
            {
                GameManager.Instance.vehicleEditor.coreBlockPlayMode.GetComponent<PartGrid>().
                    CheckConnection();
                start = !start;
            }

            //Reset all the components that were neccesary for the vehicle to the detached part components
            transform.parent = null;
            gameObject.AddComponent<AudioSource>();
            AudioManager.Instance.Play(AudioManager.clips.PartDestruction, GetComponent<AudioSource>());
            gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<Part>().ResetAction();
            gameObject.layer = 0;
            gameObject.tag = "Untagged";
            Destroy(gameObject.GetComponent<Part>());

            //Remove the part from the activation list if it was active
            if (gameObject.GetComponent<UtilityPart>() != null)
            {
                GameManager.Instance.vehicleEditor.coreBlockPlayMode.GetComponent<ActivatePartActions>()
                    .allUtilityParts.Remove(gameObject.GetComponent<UtilityPart>());
            }
        }
    }

    /// <summary>
    /// Switches colliders between the one used in playmode and the one used to build (1 by 1 boxes)
    /// </summary>
    public virtual void SwitchColliders()
    {
        buildModeCollider.enabled = !buildModeCollider.enabled;
        playModeCollider.enabled = !playModeCollider.enabled;
    }

    /// <summary>
    /// Function to reset part action values (called in ActivatePartActions)
    /// </summary>
    public virtual void ResetAction()
    {

    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < attachablePoints.Count; i++)
        {
            if (attachedParts[i] == null)
            {
                if (attachablePoints[i]) { Gizmos.color = Color.green; }
                else { Gizmos.color = Color.red; }

                //{ "Right", "Left", "Top", "Bottom", "Back", "Front" })]
                switch (i)
                {
                    case 0:
                        Gizmos.DrawSphere(transform.right * 0.5f + transform.position, 0.2f);
                        break;
                    case 1:
                        Gizmos.DrawSphere(-transform.right * 0.5f + transform.position, 0.2f);
                        break;
                    case 2:
                        Gizmos.DrawSphere(transform.up * 0.5f + transform.position, 0.2f);
                        break;
                    case 3:
                        Gizmos.DrawSphere(-transform.up * 0.5f + transform.position, 0.2f);
                        break;
                    case 4:
                        Gizmos.DrawSphere(-transform.forward * 0.5f + transform.position, 0.2f);
                        break;
                    case 5:
                        Gizmos.DrawSphere(transform.forward * 0.5f + transform.position, 0.2f);
                        break;
                }
            }

        }
    }
}
