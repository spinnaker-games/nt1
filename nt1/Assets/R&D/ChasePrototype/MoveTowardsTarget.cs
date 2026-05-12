using UnityEngine;

public class MoveTowardsTarget : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _speed = 5f;

    void Update()
    {
        if ( _target == null ) return;

        transform.position = Vector3.MoveTowards
        (
            transform.position,
            _target.position,
            _speed * Time.deltaTime
        );
    }

    public void SetTarget( Transform target )
    {
        _target = target;
    }
}