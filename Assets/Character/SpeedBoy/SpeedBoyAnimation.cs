using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoyAnimation : MonoBehaviour
{
    [Header("Component Ref")]
    [SerializeField]
    private Animator myAnimator;
    [SerializeField]
    private SpeedBoyMovement myMovement;

    [SerializeField]
    private GameObject animRoot;
    [SerializeField]
    private ParticleSystem cloakEffect;
    [SerializeField]
    private ParticleSystem dasingEffect;

    private int hashParamIsAutoRunLeft;
    private int hashParamIsAutoRunRight;
    private int hashParamIsJumping;
    // Start is called before the first frame update
    void Start()
    {
        // Convert Animator String Parameter -> Hash Code to easy control anim
        hashParamIsAutoRunLeft = Animator.StringToHash("isAutoRunLeft");
        hashParamIsAutoRunRight = Animator.StringToHash("isAutoRunRight");
        hashParamIsJumping = Animator.StringToHash("isMidAir");
    }

    // Update is called once per frame
    void Update()
    {
        ResetParams();
        myAnimator.SetBool(hashParamIsAutoRunRight, myMovement.IsFacingRightDirection == 1);
        myAnimator.SetBool(hashParamIsAutoRunLeft, myMovement.IsFacingRightDirection == -1);
        if (myMovement.IsMidAir)
        {
            ResetParams();
            myAnimator.SetBool(hashParamIsJumping, myMovement.IsMidAir);
        }
        // Update effect and anim root
        var speedBoyState = SpeedBoyState.GetInstance();
        dasingEffect.gameObject.SetActive(speedBoyState.PunchSlice || speedBoyState.Punching);
        cloakEffect.gameObject.SetActive(!speedBoyState.PunchSlice && !speedBoyState.Punching);
        animRoot.SetActive(!speedBoyState.PunchSlice && !speedBoyState.Punching);

    }

    private void ResetParams()
    {
        myAnimator.SetBool(hashParamIsAutoRunLeft, false);
        myAnimator.SetBool(hashParamIsAutoRunRight, false);
        myAnimator.SetBool(hashParamIsJumping, false);
    }
}
