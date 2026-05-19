using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class HealthBase : MonoBehaviour, IHealth
{
    [SerializeField] protected int maxHP = 100;

    /// <summary>
    /// Event that is called whenever the HP changes passing the old, new, and max value.
    /// </summary>
    [SerializeField] UnityEvent<int, int, int> onHealthChanged;
    [SerializeField] UnityEvent healthZero;

    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;

    protected int currentHP;

    public HitType attackType;

    void Awake()
    {
        // TODO: want to load this from a file so health is consistent across scenes
        currentHP = maxHP;
    }

    private void ClampHP()
    {
        currentHP = Math.Clamp(currentHP, 0, maxHP);
    }

    public void Damage(int amount)
    {
        var oldHP = currentHP;
        currentHP -= amount;
        ClampHP();
        onHealthChanged.Invoke(oldHP, currentHP, maxHP);

        if (currentHP <= 0)
        {
            Debug.Log("Damage");
            healthZero.Invoke();   
        }
    }

    public void Heal(int amount)
    {
        var oldHP = currentHP;
        currentHP += amount;
        ClampHP();
        onHealthChanged.Invoke(oldHP, currentHP, maxHP);
    }

    public void TakeHit(HitInfo hitInfo)
    {
        attackType = hitInfo.attackType;
        Damage(hitInfo.damage);
        
    }
}
