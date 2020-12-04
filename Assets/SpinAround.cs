using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField]
    [Tooltip("speed is 0 up to 180 degree persecond")]
    [Range(20, 180)]
    private int spinningSpeed;
    [SerializeField] [Range(-1, 1)] private int direction;
    private Vector3 d;
    private void Start()
    {
        if (direction == -1)
        {
            d = new Vector3(1, 0, 0);
        }
        if(direction == 0)
        {
            d = new Vector3(0, 1, 0);
        }
        if(direction == 1)
        {
            d = new Vector3(0, 0, 1);
        }
    }
    void Update()
    {
        transform.RotateAround(target.transform.position, d, spinningSpeed * Time.deltaTime);
    }
}
