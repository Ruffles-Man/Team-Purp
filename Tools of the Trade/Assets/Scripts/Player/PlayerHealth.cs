using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : HealthBase
{
    void Awake()
    {
        // TODO: want to load this from a file so health is consistent across scenes
        currentHP = maxHP;
    }
}
