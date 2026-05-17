using UnityEngine;

public class RotateTest : MonoBehaviour
{
    public float rotateSpeed = 90f; // degrees per second

    void Update()
    {
        // Rotate the object around its Y-axis
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
