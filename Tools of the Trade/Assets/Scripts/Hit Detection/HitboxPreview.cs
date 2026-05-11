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
        UpdateFrameFromAnimator();
#endif
    }

    void UpdateFrameFromAnimator()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length == 0) return;

        AnimationClip clip = clipInfo[0].clip;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float normalizedTime = stateInfo.normalizedTime % 1f;
        currentFrame = Mathf.RoundToInt(normalizedTime * clip.frameRate * clip.length);
    }

    ClipHitboxBinding GetCurrentBinding()
    {
        if (moveset == null) return null;

        if (Application.isPlaying) {
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length == 0) return null;
            AnimationClip currentClip = clipInfo[0].clip;
            return moveset.clipBindings.FirstOrDefault(b => b.clip == currentClip);
        }

        if (selectedBindingIndex < 0 || selectedBindingIndex >= moveset.clipBindings.Count) return null;
        return moveset.clipBindings[selectedBindingIndex];
    }

    void OnDrawGizmos()
    {
        if (moveset == null) return;

        ClipHitboxBinding binding = GetCurrentBinding();
        if (binding == null) return;

        foreach (AttackData data in binding.hitboxes) {
            if (data == null) continue;
            int start = data.startupFrames;
            int end = start + data.activeFrames - 1;
            if (currentFrame >= start && currentFrame <= end)
                data.DrawGizmo(transform);
        }
    }
}