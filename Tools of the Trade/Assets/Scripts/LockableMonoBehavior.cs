using UnityEngine;

public class LockableMonoBehavior : MonoBehaviour
{
    public bool _Locked { get; private set; }= false;

    public virtual void Lock() { enabled = false; _Locked = true; }
    public virtual void Unlock() { enabled = true; _Locked = false; }
}