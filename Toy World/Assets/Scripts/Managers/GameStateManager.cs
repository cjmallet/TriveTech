using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private GameState currentGameState;

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
    }

    private void SwitchState(GameState previousState, GameState nextState)
    {
        if (nextState == GameState.Playing && previousState == GameState.Building)
            GameManager.Instance.vehicleEditor.PrepareVehicle();
    }

    public enum GameState : int
    {
        Building,
        Playing,
        Paused,
        Restarting
    }
}
