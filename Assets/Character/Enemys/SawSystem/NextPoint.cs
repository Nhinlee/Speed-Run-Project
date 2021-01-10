using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPoint : MonoBehaviour
{
    [SerializeField]
    private float radius;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
