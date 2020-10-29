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
        if(myMovement.IsRunRight)
        {
            ResetParams();
            myAnimator.SetBool(hashParamIsAutoRunRight, true);
        }
        if(myMovement.IsRunLeft)
        {
            ResetParams();
            myAnimator.SetBool(hashParamIsAutoRunLeft, true);
        }    
        if(myMovement.IsMidAir)
        {
            ResetParams();
            myAnimator.SetBool(hashParamIsJumping, true);
        }
    }

    private void ResetParams()
    {
        myAnimator.SetBool(hashParamIsAutoRunLeft, false);
        myAnimator.SetBool(hashParamIsAutoRunRight, false);
        myAnimator.SetBool(hashParamIsJumping, false);
    }
}
