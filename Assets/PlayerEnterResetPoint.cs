using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterResetPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameObject resetPlaceObj = GameObject.FindObjectOfType<ResetPlace>().gameObject;
            ResetPlace resetPlaceSc = resetPlaceObj.GetComponent<ResetPlace>();
            resetPlaceSc.SetResetPlace(gameObject.transform.position);
            Debug.Log(gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
