using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawActivator : MonoBehaviour
{
    public Action onActivated;

    [SerializeField]
    private BoxCollider2D myCollider;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, myCollider.size);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onActivated?.Invoke();
        }
    }
}
