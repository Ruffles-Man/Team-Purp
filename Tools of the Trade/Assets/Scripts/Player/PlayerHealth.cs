using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PlayerHealth : HealthBase
{
    [SerializeField] private HealthBar healthBar;

    void OnEnable()
    {
        var healthBar = UnityEngine.Object.FindObjectsByType<HealthBar>().Where(hpBar => hpBar.CompareTag("Player")).Single();
        onHealthChanged.AddListener(healthBar.UpdateValue);
    }

    void OnDisable()
    {
        onHealthChanged.RemoveListener(healthBar.UpdateValue);
    }

    private void Awake()
    {
        currentHP = maxHP;
        // If the inspector slot is empty (which it will be on a Prefab), find it dynamically!
        if (healthBar == null)
        {
            GameObject managerObj = GameObject.Find("PlayerHealthBar");
            if (managerObj != null)
            {
                healthBar = managerObj.GetComponent<HealthBar>();
            }
            else
            {
                Debug.LogWarning($"[PlayerHealth] Could not find a PlayerHealthBar in the scene context of {gameObject.name}!");
            }
        }
    }
}
