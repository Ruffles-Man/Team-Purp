using UnityEngine;

public class FixedRotationHealth : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // Cache the main camera transform for performance
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    // LateUpdate runs after all movement and animations are calculated for the frame
    void LateUpdate()
    {
        if (mainCameraTransform != null)
        {
            // Force this object to match the exact rotation of the camera
            transform.rotation = mainCameraTransform.rotation;
        }
    }
}
