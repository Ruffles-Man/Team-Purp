using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class HealthBase : MonoBehaviour, IHealth
{
    [SerializeField] protected int maxHP = 100;
    [SerializeField] protected int currentHP;

    /// <summary>
    /// Event that is called whenever the HP changes passing the old, new, and max value.
    /// </summary>
    public UnityEvent<int, int, int> onHealthChanged;
    public UnityEvent healthZero;

    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;


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
        Debug.Log($"{gameObject.name} took {amount} damage!");

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
