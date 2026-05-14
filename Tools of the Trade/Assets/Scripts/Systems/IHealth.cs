using UnityEngine;

public interface IHealth 
{
    public int MaxHP { get; }
    public int CurrentHP { get; } 
    public void TakeHit(HitInfo hitInfo);
    public void Damage(int amount);
}
