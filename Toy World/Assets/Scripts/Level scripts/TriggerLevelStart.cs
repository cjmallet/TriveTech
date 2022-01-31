using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevelStart : MonoBehaviour
{
    public bool levelStarted;
    public float timerBeforeSpawn = 1;

    private void Start()
    {
        StartCoroutine(LevelStartOnTimer(timerBeforeSpawn));
    }

    /// <summary>
    /// Tell level manager to start the level
    /// </summary>
    private void StartLevel()
    {
        if (!levelStarted)
        {
            GameManager.Instance.levelManager.StartLevel();
            levelStarted = true;
        }
    }

    /// <summary>
    /// Start the level right away with some delay of spawning the cargo.
    /// </summary>
    /// <param name="timer">The amount of time in seconds between start of the game
    /// and cargo spwan.</param>
    /// <returns></returns>
    private IEnumerator LevelStartOnTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        StartLevel();
    }


    // Use if we want to go back to collider

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("CoreBlock"))
    //        StartLevel();
    //}
}
