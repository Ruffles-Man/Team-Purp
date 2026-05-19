using UnityEngine;

public class ChildAnimatorBridge : MonoBehaviour
{
    private Animator animator;
    private readonly int dieHash = Animator.StringToHash("Die");
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void onDeath()
    {
        animator.SetTrigger(dieHash);
    }
    public void DisableParentObject()
    {
        // transform.parent grabs "OffensiveEnemy" and turns it off!
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}