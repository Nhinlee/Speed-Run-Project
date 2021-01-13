using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBossBrain : MonoBehaviour
{
    public Action OnDeactivate;

    [SerializeField]
    private Animator myAnimator;

    [SerializeField]
    private BoxCollider2D myCollider;

    private void Start()
    {
        ReactivateBrain();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var state = SpeedBoyState.Instance;
            if (state.Punching || state.PunchSlice)
            {
                DeactivateBrain();
            }
            else
            {
                other.GetComponent<SpeedBoyController>().Die();
            }
        }
    }

    // This public because activate brain by himself (after deactivate time in seconds)
    public void ReactivateBrain()
    {
        myAnimator.SetBool("IsActivate", true);
        myCollider.enabled = true;
    }

    // This private because deactivate brain by Speedboy (main character)
    private void DeactivateBrain()
    {
        OnDeactivate?.Invoke();
        myAnimator.SetBool("IsActivate", false);
        myCollider.enabled = false;
    }

}
