using UnityEngine;
using UnityEngine.InputSystem;

public class attackInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public bool jabPressed;
    
    // Note that this a reference UAL1_Standard which is the most parent level object including the model
    Animator animator;
    int crossHash;
    int jabHash;

    float crossStartUp = 0.25F;//0.33F;
    float jabStartUp = 0.23F;


    public InputActionReference jab;
    public InputActionReference cross;
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);

        crossHash = Animator.StringToHash("cross");
        jabHash = Animator.StringToHash("jab");

        //jabPressed = jab.action.ReadValue<bool>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    private void OnEnable()
    {
        jab.action.started += Jab;
        jab.action.Enable();

        cross.action.started += Cross;
        cross.action.Enable();
    }

    private void OnDisable()
    {
        jab.action.started -= Jab;
        jab.action.Disable();

        cross.action.started -= Cross;
        cross.action.Disable();
    }
    private void Jab(InputAction.CallbackContext obj)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //animator.SetTrigger(jabHash);
        Debug.Log("Jabbed");

        /*
        (The jab function was called so the jab button is being pressed)
        if the name of the already triggered animation is not Jab
            trigger jab if it is past the start up of the other
        if the name of the already triggered animation IS Jab
            trigger jan if it is past the startup of Jab
        */
        float progress = stateInfo.normalizedTime;
        if (stateInfo.IsName("UAL_Armature_Idle_Loop")){
            animator.SetTrigger(jabHash);
        } else if (stateInfo.IsName("UAL_Armature_Punch_Jab") && progress > jabStartUp){
            animator.SetTrigger(jabHash);
        } else if(stateInfo.IsName("UAL_Armature_Punch_Cross") && progress > crossStartUp){
            animator.SetTrigger(jabHash);
        }

        Debug.Log("Jabbed");
    }

    private void Cross(InputAction.CallbackContext obj)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.SetTrigger(crossHash);
        Debug.Log("Crossed");
    }
    
}
