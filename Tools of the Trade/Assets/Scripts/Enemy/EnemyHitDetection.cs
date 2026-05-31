using System;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(PlayerVFX))]
public class EnemyHitDetection : LockableMonoBehavior
{
    public Transform hitboxOrigin;
    public MovesetData moveset;
    Animator animator;
    //PlayerVFX playerVFX;
    
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        // playerVFX = GetComponent<PlayerVFX>();
    }

    void Update()
    {
        CastHitboxes();
    }

    private ClipHitboxBinding lastBinding;     
    private readonly HashSet<IHittable> hitThisAttack = new();
    public void CastHitboxes()
    {
        var binding = AnimatorInfo.GetCurrentBinding(moveset, animator);

        // flush if we've moved to a different attack
        if (binding != lastBinding)
        {
            hitThisAttack.Clear();

            // // Release when leaving an attack, request when entering one
            // if (lastBinding != null) playerVFX.ReleaseTrails();
            // if (binding != null) playerVFX.RequestTrails();

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
                    attackType = hitbox.attackType,
                    damage = hitbox.damage,
                    position = collider.ClosestPoint(worldOrigin)  // closest point on target to hitbox center
                });
            }
        }
    }
}