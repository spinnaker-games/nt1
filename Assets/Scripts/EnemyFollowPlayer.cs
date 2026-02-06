using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] float _moveSpeed = 3.5f;
    [SerializeField] float _followStartDistance = 8f;
    [SerializeField] float _followEndDistance = 12f;
    [SerializeField] float _bufferDistance = 1.5f;

    [Header( "Dynamic Speed" )]
    [SerializeField] float _speedVariationAmount = 0.4f;
    [SerializeField] float _speedVariationSpeed = 0.8f;

    NavMeshAgent _agent;
    bool _isFollowing;
    float _noiseOffset;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = _bufferDistance;

        _noiseOffset = Random.Range( 0f, 1000f );
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
            float noise = Mathf.PerlinNoise( _noiseOffset, Time.time * _speedVariationSpeed );
            float speedMultiplier = Mathf.Lerp(
                1f - _speedVariationAmount,
                1f + _speedVariationAmount,
                noise
            );

            _agent.speed = _moveSpeed * speedMultiplier;
            _agent.stoppingDistance = _bufferDistance;
            _agent.SetDestination( _player.position );
        }
    }
}