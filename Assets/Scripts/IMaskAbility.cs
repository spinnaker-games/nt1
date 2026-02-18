using UnityEngine;

public interface IMaskAbility
{
    void ActivateMask();
    void DeactivateMask();
    bool IsActive { get; }
}