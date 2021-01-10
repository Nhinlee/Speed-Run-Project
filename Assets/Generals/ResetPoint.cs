using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPoint : MonoBehaviour
{
    // [SerializeField]
    //private GameSession gameSession;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            try
            {
                var speedBoyController = other.gameObject.GetComponent<SpeedBoyController>();
                var speedBoyMovement = other.gameObject.GetComponent<SpeedBoyMovement>();
                //
                speedBoyController.ResetPointPosition = this.transform.position;
                speedBoyController.SaveFacingDirection = speedBoyMovement.IsFacingRightDirection;
                Debug.Log(speedBoyMovement.IsFacingRightDirection);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
