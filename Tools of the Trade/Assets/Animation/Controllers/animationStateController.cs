using UnityEngine;
using UnityEngine.InputSystem;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isJabbingHash;
    int isCrossingHash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
        isJabbingHash = Animator.StringToHash("isJabbing");
        isCrossingHash = Animator.StringToHash("isCrossing");
    }

    // Update is called once per frame
    void Update()
    {
        bool isJabbing = animator.GetBool(isJabbingHash);
        bool isCrossing = animator.GetBool(isCrossingHash);
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

        // if player presses x key
        if (!isCrossing && crossPressed)
        {
            // then set the isCrossing boolean to be true
            animator.SetBool(isCrossingHash, true);
        }

        // if player is not pressing x key
        if (isCrossing && !crossPressed)
        {
            // then set the isCrossing boolean to be false
            animator.SetBool(isCrossingHash, false);
        }
    }
}
