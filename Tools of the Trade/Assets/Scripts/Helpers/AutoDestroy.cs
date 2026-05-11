using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float destroyTime = 3f;

    void Start()
    {
        Destroy(this, destroyTime);
    }
}
