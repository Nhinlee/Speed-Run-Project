using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGun : MonoBehaviour
{

    [SerializeField]
    private float maxDistanceOfLazer = 100f;

    [SerializeField]
    private LineRenderer lineRenderers;

    //[SerializeField]
    private Vector2 direction;

    [SerializeField]
    [Tooltip("Collide with layers")]
    private LayerMask colliderLayer;

    private void Start()
    {
        direction = Vector2.right;
    }

    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        var hit = Physics2D.Raycast(transform.position, direction, maxDistanceOfLazer, colliderLayer);
        if (hit)
        {
            var o = hit.collider.gameObject;
            if (o.CompareTag("Player"))
            {
                o.GetComponent<SpeedBoyController>().Die();
            }
            
        }
        else
        {

        }
    }
    
}
