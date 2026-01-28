using UnityEngine;

public class FlyAtTarget : MonoBehaviour
{
    [SerializeField] float _speed = 1.0f;
    [SerializeField] Transform _target;
    Vector3 _targetPos;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        //TODO: Add null checks
        _targetPos = _target.transform.position;
    }

    void Update()
    {
        MoveToTarget();
        DestroyWhenReached();
    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);
    }

    void DestroyWhenReached()
    {
        if (transform.position == _targetPos)
        {
            Destroy(gameObject); //TODO: ObjectPool
        }
    }
}