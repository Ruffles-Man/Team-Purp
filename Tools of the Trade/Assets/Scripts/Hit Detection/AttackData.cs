using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "AttackData", menuName = "AttackData", order = 0)]
public class AttackData : ScriptableObject 
{
    [Header("Detection")]
    public LayerMask attackingLayers;
    public Vector3 hitboxOffset;
    public HitboxShape hitboxShape;
    public Vector3 boxSize;
    public float sphereRadius;

    [Header("Hit Effects")]
    public int damage;
    public Vector2 knockback;
    
    [Header("Frame Data")]
    public int startupFrames;
    public int activeFrames;
    public int recoveryFrames;

    public bool HitboxActive(int frame)
    {
        return (frame >= startupFrames) && (frame <= startupFrames + activeFrames - 1);
    }

    public (Collider[] colliders, Vector3 worldOrigin) CastCheck(Transform origin)
    {
        Vector3 worldOrigin = origin.TransformPoint(hitboxOffset);

        if (hitboxShape == HitboxShape.Box)
        {
            return (Physics.OverlapBox(
                worldOrigin,
                boxSize / 2f,
                origin.rotation,
                attackingLayers
            ), worldOrigin);
        }
        else
        {
            return (Physics.OverlapSphere(
                worldOrigin,
                sphereRadius,
                attackingLayers
            ), worldOrigin);
        }
    }

    public void DrawGizmo(Transform origin) {
        Gizmos.color = new Color(1f, 0f, 0f, 0.35f);
        Gizmos.matrix = Matrix4x4.TRS(
            origin.TransformPoint(hitboxOffset),
            origin.rotation,
            Vector3.one
        );

        if (hitboxShape == HitboxShape.Box)
            Gizmos.DrawCube(Vector3.zero, boxSize);
        else
            Gizmos.DrawSphere(Vector3.zero, sphereRadius);

        // Wireframe overlay so the shape reads clearly
        Gizmos.color = new Color(1f, 0f, 0f, 1f);
        if (hitboxShape == HitboxShape.Box)
            Gizmos.DrawWireCube(Vector3.zero, boxSize);
        else
            Gizmos.DrawWireSphere(Vector3.zero, sphereRadius);
    }
}
