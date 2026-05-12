using System;
using UnityEngine;

[Serializable]
public class Attack
{
    [field: SerializeField] public string AnimationName { get; set; }
    [field: SerializeField] public float TransitionDuration { get; set; }
    [field: SerializeField] public int ComboStateIndex { get; set; } = -1;
    [field: SerializeField] public float ComboAttackTime { get; set; }
    [field: SerializeField] public float ForceTime { get; set; }
    [field: SerializeField] public float Force { get; set; }
    [field: SerializeField] public int DamageAmount { get; set; }
    [field: SerializeField] public float Knockback { get; set; }
}