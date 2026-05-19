using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HitInfo
{
    public HitType attackType;
    public int damage;
    public Vector3 position;
}