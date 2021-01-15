using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMapManager : MonoBehaviour
{
    [SerializeField]
    private CanvasTutorial canvasTutorial;

    [SerializeField]
    private List<TutorialCollider> colliders;

    private void Awake()
    {
        // Register Callbacks
        for (int i = 0; i < colliders.Count; i++) 
        {
            colliders[i].OnCollideWithPlayer += canvasTutorial.Show;
        }
    }

    private void OnDestroy()
    {
        // Unregister Callbacks
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].OnCollideWithPlayer -= canvasTutorial.Show;
        }
    }

}
