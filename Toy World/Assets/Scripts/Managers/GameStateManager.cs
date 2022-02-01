using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    private GameState currentGameState;

    private Camera mainCam;

    public TextMeshProUGUI RestartText;

    public PlayerInput playerInput;

    /// <summary>
    /// Getter/Setter for gamestate.
    /// </summary>
    public GameState CurrentGameState
    {
        get { return currentGameState; }
        set
        {
            SwitchState(currentGameState, value);

            currentGameState = value;
        }
    }

    private void Start()
    {
        currentGameState = GameState.Building;
        mainCam = Camera.main;
        playerInput.SwitchCurrentActionMap("UI");
    }

    /// <summary>
    /// Called to switch to a new state.
    /// </summary>
    /// <param name="previousState">State before switch.</param>
    /// <param name="nextState">State we're switching to.</param>
    private void SwitchState(GameState previousState, GameState nextState)
    {
        if (nextState == GameState.Playing && previousState == GameState.Building)
            GameManager.Instance.vehicleEditor.PrepareVehicle();
    }

    /// <summary>
    /// Switches to play-mode from building mode by setting up the scene and UI.
    /// </summary>
    public void SwitchToPlay()
    {
        GameManager.Instance.stateManager.CurrentGameState = GameState.Playing;

        AudioManager.Instance.SetMusic(AudioManager.clips.DrivingMusic);
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.EngineSounds());

        EscMenuBehaviour.buildCameraPositionStart = mainCam.transform.position;
        EscMenuBehaviour.buildCameraRotationStart = mainCam.transform.rotation;

        mainCam.gameObject.SetActive(false);

        if (GameManager.Instance.partSelectionManager.buildUIOpen)
        {
            GameManager.Instance.partSelectionManager.ClosePartSelectionUI();
            GameManager.Instance.partSelectionManager.ChangeActiveBuildState();
        }

        GameManager.Instance.partSelectionManager.crossHair.SetActive(false);

        RestartText.text = "Restart";

        playerInput.SwitchCurrentActionMap("Player");

        StartCoroutine(GameManager.Instance.levelManager.StartLevel());
    }

    /// <summary>
    /// Current gamestates.
    /// </summary>
    public enum GameState : int
    {
        Building,
        Playing
    }
}
