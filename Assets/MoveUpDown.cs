using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] [Range(0, 10)] private float speed;
    [SerializeField] private float maxHight;
    [SerializeField] private float minHight;
    private GameObject[] spike;
    private float direction = 1f;
    void Start()
    {
        spike = GameObject.FindGameObjectsWithTag("SpikeUp");
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject oneSpike in spike)
        {
            float y;
            y = oneSpike.transform.position.y;    
            if (y >= minHight || y <= maxHight) direction *= -1;          
        }
        foreach (GameObject oneSpike in spike)
        {
            float x, z, y;
            x = oneSpike.transform.position.x;
            y = oneSpike.transform.position.y;
            z = oneSpike.transform.position.z;
            y += direction * speed;
            oneSpike.transform.position = new Vector3(x, y, z);
        }
    }
}
