using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedBoyController : MonoBehaviour
{
    [SerializeField]
    private int timeDelayBeforeDie = 1;
    [SerializeField]
    private SpeedBoyAnimation speedBoyAnimation;
    [SerializeField]
    private SpeedBoyMovement speedBoyMovement;

    // Reset Point and facing direction is restored when player die
    [HideInInspector]
    public Vector3 ResetPointPosition { get; set; }
    [HideInInspector]
    public int SaveFacingDirection { get; set; }

    private Coroutine coroutineWaitingToDie;

    private void Awake()
    {
        int numberObject = FindObjectsOfType<SpeedBoyController>().Length;
        if (numberObject > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Die()
    {
        if (coroutineWaitingToDie != null) return;
        coroutineWaitingToDie = StartCoroutine(WaitingToDie());
    }

    private IEnumerator WaitingToDie()
    {
        speedBoyMovement.IsRunning = false;
        // Wait for delay time
        yield return new WaitForSeconds(timeDelayBeforeDie);
        // Set player position back to the reset point
        speedBoyMovement.CombackToResetPoint(ResetPointPosition, SaveFacingDirection);
        // Reset the scence
        int currentScence = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScence);
        //
        coroutineWaitingToDie = null;
        speedBoyMovement.IsRunning = true;
    }
}
