using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawDamage : MonoBehaviour
{
    //[SerializeField] private float waitingTime = 2f;
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject resetPlaceObj = GameObject.FindObjectOfType<ResetPlace>().gameObject;
            ResetPlace resetPlaceSc = resetPlaceObj.GetComponent<ResetPlace>();
            resetPlaceSc.PlayerDie();
        }
    }   
}