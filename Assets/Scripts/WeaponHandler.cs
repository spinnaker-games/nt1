using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] GameObject _weaponCollider;

    public void EnableWeaponCollider()
    {
        _weaponCollider.SetActive(true);
    }


    public void DisableWeaponCollider()
    {
        _weaponCollider.SetActive(false);
    }
}