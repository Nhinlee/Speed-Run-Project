using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour
{
    [SerializeField]
    [Tooltip("speed is 0 up to 180 degree persecond")]
    [Range(20, 720)]
    private float spinningSpeed = 90f;
    [SerializeField]
    private bool isSpinRightDirection;
    
    void Update()
    {
        if (isSpinRightDirection)
        {
            transform.RotateAround(transform.position, Vector3.back, spinningSpeed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(transform.position, Vector3.forward, spinningSpeed * Time.deltaTime);
        }
    }
}
