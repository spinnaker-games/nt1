using UnityEngine;
using UnityEngine.AI;

public class ChickenFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] float _moveSpeed = 3.5f;
    [SerializeField] float _followStartDistance = 8f;
    [SerializeField] float _followEndDistance = 12f;
    [SerializeField] float _bufferDistance = 1.5f;

    NavMeshAgent _agent;
    bool _isFollowing;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = _bufferDistance;
    }

    void Update()
    {
        if ( _player == null ) return;

        float distance = Vector3.Distance( transform.position, _player.position );

        if ( !_isFollowing && distance <= _followStartDistance )
        {
            _isFollowing = true;
        }

        if ( _isFollowing && distance >= _followEndDistance )
        {
            _isFollowing = false;
            _agent.ResetPath();
        }

        if ( _isFollowing )
        {
            _agent.speed = _moveSpeed;
            _agent.stoppingDistance = _bufferDistance;
            _agent.SetDestination( _player.position );
        }
    }
}