using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    private GameState currentGameState;

    private Camera mainCam;

    public TextMeshProUGUI RestartText;

    public PlayerInput playerInput;

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

    private void SwitchState(GameState previousState, GameState nextState)
    {
        if (nextState == GameState.Playing && previousState == GameState.Building)
            GameManager.Instance.vehicleEditor.PrepareVehicle();
    }

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
    }

    public enum GameState : int
    {
        Building,
        Playing
    }
}
