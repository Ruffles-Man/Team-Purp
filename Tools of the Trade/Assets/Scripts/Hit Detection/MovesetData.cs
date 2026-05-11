using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Fighting/MovesetData")]
public class MovesetData : ScriptableObject {
    public List<ClipHitboxBinding> clipBindings;
}