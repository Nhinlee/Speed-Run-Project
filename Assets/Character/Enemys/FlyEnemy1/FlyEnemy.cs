using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour, ICharacter
{
    [SerializeField]
    private ParticleSystem DeadEffect;
    [SerializeField]
    private float timeDelayToDead;

    public CharacterState State { get; protected set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(SpeedBoyState.Instance.Punching || SpeedBoyState.Instance.PunchSlice)
            {
                Die();
            }
            else
            {
                var speedBoyController = other.gameObject.GetComponent<SpeedBoyController>();
                speedBoyController.Die();
            }
        }
    }
    public void Die()
    {
        DeadEffect.Play();
        gameObject.SetActive(false);
        Destroy(gameObject, timeDelayToDead);
    }
}
