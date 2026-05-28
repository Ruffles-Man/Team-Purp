using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerVFX))]
public class PlayerAttack : LockableMonoBehavior
{
    public event Action OnAttackComplete;

    [Header("Components & Settings")]
    public Transform hitboxOrigin;
    public MovesetData moveset;

    private Animator animator;
    private PlayerVFX playerVFX;

    private readonly int kickParamHash = Animator.StringToHash("kick");
    private readonly int punchParamHash = Animator.StringToHash("punch");
    private readonly int comboStepHash = Animator.StringToHash("ComboStep");

    [HideInInspector] public int comboStep;

    private int activeAttackRequests = 0;
    private bool isCurrentlyInComboSequence = false;

    private ClipHitboxBinding lastBinding;
    private readonly HashSet<IHittable> hitThisAttack = new();

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerVFX = GetComponent<PlayerVFX>();
    }

    private void OnDestroy()
    {
        OnAttackComplete = null;
    }

    public void OnAttackStarted()
    {
        activeAttackRequests++;
    }

    public void OnAttackAnimationComplete()
    {
        if (activeAttackRequests > 0)
        {
            activeAttackRequests--;
        }

        if (activeAttackRequests == 0)
        {
            isCurrentlyInComboSequence = false;
            comboStep = 0;

            if (animator != null)
            {
                animator.SetInteger(comboStepHash, 0);
                animator.ResetTrigger(punchParamHash);
                animator.ResetTrigger(kickParamHash);
            }

            hitThisAttack.Clear();
            OnAttackComplete?.Invoke();
        }
    }

    public void PerformAttackOne()
    {
        ExecuteAttack(punchParamHash);
    }

    public void PerformAttackTwo()
    {
        ExecuteAttack(kickParamHash);
    }

    private void ExecuteAttack(int attackParamHash)
    {
        animator.ResetTrigger(punchParamHash);
        animator.ResetTrigger(kickParamHash);

        if (isCurrentlyInComboSequence)
        {
            if (comboStep < 3)
            {
                comboStep++;
            }
            else
            {
                comboStep = 0;
            }
        }
        else
        {
            comboStep = 0;
            isCurrentlyInComboSequence = true;
        }

        animator.SetInteger(comboStepHash, comboStep);
        animator.SetTrigger(attackParamHash);
    }

    void Update()
    {
        CastHitboxes();
    }

    public void CastHitboxes()
    {
        var binding = AnimatorInfo.GetCurrentBinding(moveset, animator);

        if (binding != lastBinding)
        {
            hitThisAttack.Clear();
            if (lastBinding != null) playerVFX.ReleaseTrails();
            if (binding != null) playerVFX.RequestTrails();
            lastBinding = binding;
        }

        if (binding == null) return;

        int currentFrame = AnimatorInfo.GetCurrentFrameFromAnimator(animator);
        foreach (var hitbox in binding.hitboxes)
        {
            if (hitbox.HitboxActive(currentFrame))
            {
                ProcessHitbox(hitbox);
            }
        }
    }

    private void ProcessHitbox(AttackData hitbox)
    {
        var (colliders, worldOrigin) = hitbox.CastCheck(hitboxOrigin);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IHittable hittable) && hitThisAttack.Add(hittable))
            {
                hittable.TakeHit(new HitInfo
                {
                    damage = hitbox.damage,
                    position = collider.ClosestPoint(worldOrigin)
                });
            }
        }
    }
}