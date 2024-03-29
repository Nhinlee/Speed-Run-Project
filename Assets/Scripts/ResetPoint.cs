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
                Debug.Log("Reset Point");
                //
                var speedBoyController = other.gameObject.GetComponent<SpeedBoyController>();
                var speedBoyMovement = other.gameObject.GetComponent<SpeedBoyMovement>();
                //
                speedBoyController.ResetPointPosition = this.transform.position;
                speedBoyController.SaveFacingDirection = speedBoyMovement.IsFacingRightDirection;
                Destroy(gameObject);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
