using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public float FreeLookMovementSpeed;
    public float TargetingMovementSpeed;
    public float RotationDamping;
    public float DodgeDuration;
    public float DodgeDistance;
}