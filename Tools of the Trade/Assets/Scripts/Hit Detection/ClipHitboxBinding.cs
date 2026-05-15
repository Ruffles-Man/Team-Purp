using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ClipHitboxBinding {
    public AnimationClip clip;
    public List<AttackData> hitboxes;
}