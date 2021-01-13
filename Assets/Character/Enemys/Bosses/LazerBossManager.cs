using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LazerBossManager : MonoBehaviour, ICharacter
{
    [SerializeField]
    private LazerBoss bossBody;

    [SerializeField]
    private Slider healthBarUI;

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private List<LazerBossBrain> brains;

    [SerializeField]
    private Lazer lazerGun;

    [SerializeField]
    private ParticleSystem deadEffect;

    [SerializeField]
    [Tooltip("Seconds")]
    private float timeDeactivate;

    [SerializeField]
    [Tooltip("Seconds")]
    private float timeDelayToDead;

    // Private fields
    private float numCurrentDeactivatedBrain = 0;
    private int currentHealth;

    public CharacterState State => throw new NotImplementedException();


    private void Start()
    {
        // Init values
        currentHealth = maxHealth;
        // Register Callback;
        bossBody.OnBossGetHitByPlayer += BossGetHitByPlayer;
        foreach (var brain in brains)
        {
            brain.OnDeactivate += DeActiveBrain;
        }
    }

    private void OnDestroy()
    {
        // Unregister Callback;
        bossBody.OnBossGetHitByPlayer -= BossGetHitByPlayer;
        foreach (var brain in brains)
        {
            brain.OnDeactivate -= DeActiveBrain;
        }
    }

    private void DeActiveBrain()
    {
        numCurrentDeactivatedBrain++;
        if(numCurrentDeactivatedBrain >= brains.Count)
        {
            StartCoroutine(StartDeactiveBossForSeconds());
            numCurrentDeactivatedBrain = 0;
        }
    }

    private IEnumerator StartDeactiveBossForSeconds()
    {
        // Deactivate
        lazerGun.gameObject.SetActive(false);
        bossBody.Deactivate();
        // Wait for seconds
        yield return new WaitForSeconds(timeDeactivate);
        // Reactivate
        ReactivateBoss();
    }

    private void ReactivateBoss()
    {
        // Lazer Gun
        lazerGun.gameObject.SetActive(true);
        // Boss body
        bossBody.Reactivate();
        // All Boss brain
        foreach (var brain in brains)
        {
            brain.ReactivateBrain();
        }
    }

    private void BossGetHitByPlayer()
    {
        currentHealth--;
        if(currentHealth <= 0)
        {
            Die();
        }
        healthBarUI.value = currentHealth * 1.0f / maxHealth;
    }

    public void Die()
    {
        deadEffect.Play();
        bossBody.gameObject.SetActive(false);
        lazerGun.gameObject.SetActive(false);
        Destroy(gameObject, timeDelayToDead);
        // TODO: Open the door to player through and win this level.
    }
}
