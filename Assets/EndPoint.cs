using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public Action OnWin; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO refactor player tag -> move into constant class
        if (other.CompareTag("Player"))
        {
            Debug.Log("Win");
            OnWin?.Invoke();
        }
    }
}
