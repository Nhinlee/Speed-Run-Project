using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length;
    private float startPosX;

    [SerializeField]
    private Transform cameraPos;

    [SerializeField]
    [Range(0,1)]
    private float parallaxEffect;

    private void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        startPosX = transform.position.x;
    }

    private void Update()
    {
        float temp = cameraPos.position.x * (1 - parallaxEffect);
        float distance = cameraPos.position.x * parallaxEffect;

        transform.position = new Vector3(startPosX + distance, transform.position.y, transform.position.z);

        if(temp > startPosX + length)
        {
            startPosX += length;
        }
        else if(temp < startPosX - length)
        {
            startPosX -= length;
        }
    }
}
