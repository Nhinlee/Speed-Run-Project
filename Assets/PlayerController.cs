using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int timeDelayBeforeDie = 1;

    [SerializeField]
    private Transform playerTransform;

    //[SerializeField]
    //private GameSession gameSession;
    GameSession gameSession;
    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        playerTransform.position = gameSession.GetResetPoint();
        // changing the direction 
        if(gameSession.GetDirection() == 1)
        {
            playerTransform.Rotate(0f, 180f, 0f);

        }
    }

    public void Die()
    {
        StartCoroutine(WaitingToDie());
        // Set player position back to the reset point

        playerTransform.position = gameSession.GetResetPoint();
        // Reset the scence
        int currentScence = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScence);
    }

    IEnumerator WaitingToDie()
    {
        yield return new WaitForSeconds(timeDelayBeforeDie);
    }
}
