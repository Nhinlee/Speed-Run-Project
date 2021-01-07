using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterResetPoint : MonoBehaviour
{
    [SerializeField]
    private Vector2 resetPosition = new Vector2(0, 0);

    [SerializeField]
    private int playerDirection = 0;
    // [SerializeField]
    //private GameSession gameSession;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameSession gameSession = FindObjectOfType<GameSession>();
            gameSession.SetResetPoint(resetPosition, playerDirection);
            Destroy(gameObject);
        }
    }
}
