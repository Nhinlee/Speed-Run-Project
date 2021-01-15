using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    public Action<string, string, string> OnCollideWithPlayer;

    [SerializeField]
    private string strInteract;

    [SerializeField]
    private string strButton;

    [SerializeField]
    private string strActionName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Tutorial Collider: " + other);
        if (other.CompareTag("Player"))
        {
            OnCollideWithPlayer?.Invoke(strInteract, strButton, strActionName);
        }
    }
}
