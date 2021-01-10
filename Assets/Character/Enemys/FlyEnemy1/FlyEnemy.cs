using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var speedBoyController = other.gameObject.GetComponent<SpeedBoyController>();
            speedBoyController.Die();
        }
    }
}
