using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField]
    private Vector2 resetPoint = new Vector2(0, 0);

    // 0 mean go from L -> R
    // 1 mean go from R -> L
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("1 is L -> R, 0 is R -> L")]
    private int playerDirection = 0;

    // Keep track only 1 PlayerController
    private void Awake()
    {
        int numberObject = FindObjectsOfType<GameSession>().Length;
        if (numberObject > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // set new resetPoint
    public void SetResetPoint(Vector2 newResetPoint, int direction)
    {
        resetPoint = newResetPoint;
        playerDirection = direction;
    }

    public Vector2 GetResetPoint()
    {
        return resetPoint;
    }
    
    public int GetDirection()
    {
        return playerDirection;
    }
}
