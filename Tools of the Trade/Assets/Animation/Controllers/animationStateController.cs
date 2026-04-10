using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isJabbingHash;
    int isCrossingHash;

    int crossHash;
    int jabHash;

    bool jabHeld;
    bool crossHeld;

    float crossStartUp = 0.25F;//0.33F;
    float jabStartUp = 0.23F;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
        isJabbingHash = Animator.StringToHash("isJabbing");
        isCrossingHash = Animator.StringToHash("isCrossing");

        crossHash = Animator.StringToHash("cross");
        jabHash = Animator.StringToHash("jab");

        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current state of the first layer (index 0)
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        bool isJabbing = animator.GetBool(isJabbingHash);
        //bool isCrossing = animator.GetBool(isCrossingHash);
        bool jabPressed = Keyboard.current.cKey.isPressed;
        bool crossPressed = Keyboard.current.xKey.isPressed;
        
        // // if player presses x key
        // if (!isJabbing && jabPressed) {
        //     // then set the isJabbing boolean to be true
        //     animator.SetBool(isJabbingHash, true);
        // }

        // // if player is not pressing x key
        // if (isJabbing && !jabPressed)
        // {
        //     // then set the isJabbing boolean to be false
        //     animator.SetBool(isJabbingHash, false);
        // }

        // // 0.7f has arms still up in guarding position
        // if (crossPressed && !stateInfo.IsName("UAL_Armature_Punch_Cross") && !crossHeld)
        // {
        //     animator.SetTrigger(crossHash);
        //     crossHeld = true;
        // }
        // else if(!crossPressed)
        // {
        //     crossHeld = false;
        // }
        
        float progress = stateInfo.normalizedTime;

        // if (stateInfo.IsName("UAL_Armature_Punch_Cross") && (progress > 0.33f))
        // {
        //     // This is your 'Recovery' or 'Cancel' window
        //     // You could check for a 'Kick' input here
        //     if (crossPressed && !crossHeld)
        //     {
        //         animator.SetTrigger(crossHash);
        //         crossHeld = true;
        //     }
        // }
        TrigAnimation(jabHash, "UAL_Armature_Punch_Jab", jabPressed, ref stateInfo, ref jabHeld, progress, jabStartUp);

        TrigAnimation(crossHash, "UAL_Armature_Punch_Cross", crossPressed, ref stateInfo, ref crossHeld, progress, crossStartUp);
    }

    void TrigAnimation(int animHash, string animName, bool pressed, ref AnimatorStateInfo stateInfo, ref bool isHeld, float progress, float startup)
    {
        if (pressed && !stateInfo.IsName(animName) && !isHeld)
        {
            animator.SetTrigger(animHash);
            isHeld = true;
        }
        else if(!pressed)
        {
            isHeld = false;
        }
        
        if (stateInfo.IsName(animName) && (progress > startup)) //cross start up is 0.33f))
        {
            // This is your 'Recovery' or 'Cancel' window
            // You could check for a 'Kick' input here
            if (pressed && !isHeld)
            {
                animator.SetTrigger(animHash);
                isHeld = true;
            }
        }
    }
}
