using UnityEngine;

public class TriggerProjectile : MonoBehaviour
{
    [SerializeField] GameObject _projectile1;
    [SerializeField] GameObject _projectile2;
    [SerializeField] GameObject _projectile3;
    [SerializeField] GameObject _projectile4;
    [SerializeField] GameObject _projectile5;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _projectile1.SetActive(true);
            _projectile2.SetActive(true);
            _projectile3.SetActive(true);
            _projectile4.SetActive(true);
            _projectile5.SetActive(true);
            Destroy(gameObject);
        }
    }
}
