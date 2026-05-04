using UnityEngine;
using UnityEngine.Events;

public class EnemyBody : MonoBehaviour
{
    [SerializeField] UnityEvent<HitInfo> onHit;
    [SerializeField] LayerMask playerAttackMask;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == playerAttackMask.value)
        {
            HitInfo hitInfo = new()
            {
                damage = 0,
                position = collision.transform.position
            };

            onHit.Invoke(hitInfo);
        }
    }
}
