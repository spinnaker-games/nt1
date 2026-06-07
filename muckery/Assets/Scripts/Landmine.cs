using UnityEngine;

public class Landmine : MonoBehaviour
{
    void Start()
    {
        WeaponDamage weapon = this.GetComponent<WeaponDamage>();
        weapon.SetAttack(50, 0);
    }
}
