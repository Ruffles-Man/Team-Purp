using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isJabbingHash;
    int isCrossingHash;

    int crossHash;

    bool jabHeld;
    bool crossHeld;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
        isJabbingHash = Animator.StringToHash("isJabbing");
        isCrossingHash = Animator.StringToHash("isCrossing");

        crossHash = Animator.StringToHash("cross");
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current state of the first layer (index 0)
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        bool isJabbing = animator.GetBool(isJabbingHash);
        //bool isCrossing = animator.GetBool(isCrossingHash);
        bool jabPressed = Keyboard.current.xKey.isPressed;
        bool crossPressed = Keyboard.current.cKey.isPressed;
        
        // if player presses x key
        if (!isJabbing && jabPressed) {
            // then set the isJabbing boolean to be true
            animator.SetBool(isJabbingHash, true);
        }

        // if player is not pressing x key
        if (isJabbing && !jabPressed)
        {
            // then set the isJabbing boolean to be false
            animator.SetBool(isJabbingHash, false);
        }

        // if (crossPressed && !crossHeld)
        // {
        //     animator.SetTrigger(crossHash);
        //     crossHeld = true;
        // }
        // else if(!crossPressed)
        // {
        //     crossHeld = false;
        // }


        // 0.7f has arms still up in guarding position
        if (crossPressed && !stateInfo.IsName("UAL_Armature_Punch_Cross") && !crossHeld)
        {
            animator.SetTrigger(crossHash);
            crossHeld = true;
        }
        else if(!crossPressed)
        {
            crossHeld = false;
        }
        
        float progress = stateInfo.normalizedTime;

        if (stateInfo.IsName("UAL_Armature_Punch_Cross") && (progress > 0.33f))
        {
            // This is your 'Recovery' or 'Cancel' window
            // You could check for a 'Kick' input here
            if (crossPressed && !crossHeld)
            {
                animator.SetTrigger(crossHash);
                crossHeld = true;
            }
        }
    }
}
