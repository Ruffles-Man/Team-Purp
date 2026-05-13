using UnityEngine;
using System.Linq;

public class AnimatorInfo
{
    public static ClipHitboxBinding GetCurrentBinding(MovesetData moveset, Animator animator)
    {
        if (moveset == null) return null;

        if (Application.isPlaying) {
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length == 0) return null;
            AnimationClip currentClip = clipInfo[0].clip;
            return moveset.clipBindings.FirstOrDefault(b => b.clip == currentClip);
        }

        return null;
    }

    public static int GetCurrentFrameFromAnimator(Animator animator)
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length == 0) return -1;

        AnimationClip clip = clipInfo[0].clip;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float normalizedTime = stateInfo.normalizedTime % 1f;
        int currentFrame = Mathf.RoundToInt(normalizedTime * clip.frameRate * clip.length);
        return currentFrame;
    }
}