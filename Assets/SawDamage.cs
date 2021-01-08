using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawDamage : MonoBehaviour
{
    //[SerializeField] private float waitingTime = 2f;
    //[SerializeField]
    //private PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SpeedBoyController playerController = collision.gameObject.GetComponent<SpeedBoyController>();
            playerController.Die();
        }
    }   
}
