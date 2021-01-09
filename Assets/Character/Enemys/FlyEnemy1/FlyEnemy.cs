using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    [SerializeField]
    private List<Transform> path;

    [SerializeField]
    private List<float> speeds;

    private int nextDestinatePointIndex;

    private void Start()
    {
        nextDestinatePointIndex = 1;
        StartMove();
    }

    private void OnDrawGizmos()
    {
        iTween.DrawLineGizmos(path.ToArray());
    }

    private void StartMove()
    {
        MoveToNextPosition();
    }
    private void MoveToNextPosition()
    {
        if (nextDestinatePointIndex >= path.Count) return;

        var hash = new Hashtable()
        {
            {"position", path[nextDestinatePointIndex]},
            {"speed", speeds[nextDestinatePointIndex-1] },
            {"easetype", iTween.EaseType.linear },
            {"oncomplete", "MoveToNextPosition"},
            {"ignoretimescale", true },
            {"islocal", true },
        };
        nextDestinatePointIndex++;

        iTween.MoveTo(gameObject, hash);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var speedBoyController = other.gameObject.GetComponent<SpeedBoyController>();
            speedBoyController.Die();
        }
    }
}
