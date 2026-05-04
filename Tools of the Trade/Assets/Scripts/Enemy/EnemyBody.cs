using UnityEngine;
using UnityEngine.Events;

public class EnemyBody : MonoBehaviour
{
    [SerializeField] UnityEvent<HitInfo> onHit;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.PlayerAttack))
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
