using Unity.Mathematics;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class PlayerLockOn : LockableMonoBehavior
{
    [SerializeField] private float lockOnRange = 10f;
    [SerializeField] private float lockOnAngle = 45f;
    
    public GameObject lockedOnTarget = null;
    private UnityEngine.Events.UnityAction currentDeathListener = null; // stored so we can remove it later

    public struct LockOnData
    {
        public GameObject target;
        public float distance;
        public float angle;
        public LockOnData(GameObject target, float distance, float angle)
        {
            this.target = target;
            this.distance = distance;
            this.angle = angle;
        }
    }

    private void ClearLockOn()
    {
        // remove the death listener from the current target before releasing
        if (lockedOnTarget != null && currentDeathListener != null)
        {
            lockedOnTarget.GetComponent<EnemyHealth>().onDeath.RemoveListener(currentDeathListener);
        }
        currentDeathListener = null;
        lockedOnTarget = null;
    }

    private void OnDestroy()
    {
        // clean up listener if player is destroyed while locked on
        ClearLockOn();
    }

    public void PerformLockOn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // release lock if already locked on
            if (lockedOnTarget != null)
            {
                ClearLockOn();
                return;
            }
            List<LockOnData> potentialTargets = new();
            // find all enemies in the scene and lock onto closest one within range and angle
            FindObjectsByType<EnemyHealth>().ToList().ForEach(enemy => {
                Vector3 toEnemy = enemy.transform.position - transform.position;
                float distanceToEnemy = toEnemy.magnitude;
                float angleToEnemy = Vector3.Angle(transform.forward, toEnemy);
                // lock-on criteria: within the specified range and within the specified angle
                if (distanceToEnemy < lockOnRange && angleToEnemy < lockOnAngle)
                {
                    potentialTargets.Add(new LockOnData(enemy.gameObject, distanceToEnemy, angleToEnemy));
                }
            });
            // select the closest target from the potential targets
            if (potentialTargets.Count > 0)
            {
                var closestTarget = potentialTargets.OrderBy(t => t.distance).First();
                lockedOnTarget = closestTarget.target;

                // store listener reference so it can be removed later
                currentDeathListener = () => ClearLockOn();
                lockedOnTarget.GetComponent<EnemyHealth>().onDeath.AddListener(currentDeathListener); // Clear lock-on if target dies
            }
        }
    }
}