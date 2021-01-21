using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // TODO: Refactor this code 
    [SerializeField]
    private float moveDistanceY;    // distance door will move when open
    public void Open()
    {
        // temp
        iTween.MoveTo(gameObject, transform.position - new Vector3(0, moveDistanceY, 0), 5);
    }

    public void Close()
    {
        // Not implement yet
    }
}
