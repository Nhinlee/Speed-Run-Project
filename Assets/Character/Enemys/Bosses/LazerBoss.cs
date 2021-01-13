using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LazerBoss : MonoBehaviour
{
    public Action OnBossGetHitByPlayer;

    [SerializeField]
    private LazerBossBrain bossBrain;

    [SerializeField]
    private Transform ActivatePos;

    [SerializeField]
    private Transform DeactivatePos;

    private void Start()
    {
        // Register Callback;
        bossBrain.OnDeactivate += BossGetHitByPlayer;
    }

    private void OnDestroy()
    {
        // Unregister Callback;
        bossBrain.OnDeactivate -= BossGetHitByPlayer;
    }

    public void Deactivate()
    {
        MoveToPos(DeactivatePos);
    }

    public void Reactivate()
    {
        bossBrain.ReactivateBrain();
        MoveToPos(ActivatePos);
    }

    private void MoveToPos(Transform pos)
    {
        iTween.MoveTo(bossBrain.gameObject, new Hashtable()
        {
            { "position" , pos },
            { "time", 5 },
            {"easetype", iTween.EaseType.easeOutBounce },
        });
    }

    private void BossGetHitByPlayer()
    {
        OnBossGetHitByPlayer?.Invoke();
    }
}
