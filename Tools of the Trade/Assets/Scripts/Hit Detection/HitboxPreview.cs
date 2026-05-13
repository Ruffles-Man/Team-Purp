using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitboxPreview : MonoBehaviour {
    public MovesetData moveset;
    public Animator animator;

    [HideInInspector] public int currentFrame;
    [HideInInspector] public int selectedBindingIndex;

    void Update() 
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
        currentFrame = AnimatorInfo.GetCurrentFrameFromAnimator(animator);
#endif
    }

    void OnDrawGizmos()
    {
        if (moveset == null) return;

        ClipHitboxBinding binding = AnimatorInfo.GetCurrentBinding(moveset, animator);
        if (binding == null) return;

        foreach (AttackData data in binding.hitboxes) 
        {
            if (data == null) continue;

            if (data.HitboxActive(currentFrame))
            {
                data.DrawGizmo(transform);
            }
        }
    }
}