using Unity.VisualScripting;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    [SerializeField] ParticleSystem _explosionVFX;
    void Start()
    {
        WeaponDamage weapon = this.GetComponent<WeaponDamage>();
        weapon.SetAttack(50, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag( "Player" ) )
        {
            _explosionVFX.Play();
        }
    }
}