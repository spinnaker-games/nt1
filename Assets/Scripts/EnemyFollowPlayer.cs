using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] Transform _player;

    [Header( "Distances" )]
    [SerializeField] float _detectDistance = 10f;
    [SerializeField] float _loseDistance = 16f;
    [SerializeField] float _stopBuffer = 1.4f;

    [Header( "Movement" )]
    [SerializeField] float _baseSpeed = 3.5f;
    [SerializeField] float _speedVariance = 0.6f;
    [SerializeField] float _repathMin = 0.4f;
    [SerializeField] float _repathMax = 1.2f;

    [Header( "Unpredictability" )]
    [SerializeField] float _playerOffsetRadius = 1.8f;
    [SerializeField] float _hesitationChance = 0.15f;
    [SerializeField] float _hesitationTime = 0.6f;

    NavMeshAgent _agent;

    enum State { Idle, Chase }
    State _state;

    float _repathTimer;
    float _hesitationTimer;
    float _mySpeed;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = _stopBuffer;

        // each enemy has a slightly different base speed
        _mySpeed = _baseSpeed + Random.Range( -_speedVariance, _speedVariance );
        _agent.speed = _mySpeed;
    }

    void Update()
    {
        if ( _player == null ) return;

        float distance = Vector3.Distance( transform.position, _player.position );

        switch ( _state )
        {
            case State.Idle:
                if ( distance <= _detectDistance )
                    EnterChase();
                break;

            case State.Chase:
                if ( distance >= _loseDistance )
                {
                    ExitChase();
                    break;
                }

                ChaseUpdate();
                break;
        }
    }

    void EnterChase()
    {
        _state = State.Chase;
        _repathTimer = 0f;
    }

    void ExitChase()
    {
        _state = State.Idle;
        _agent.ResetPath();
    }

    void ChaseUpdate()
    {
        if ( _hesitationTimer > 0f )
        {
            _hesitationTimer -= Time.deltaTime;
            _agent.isStopped = true;
            return;
        }

        _agent.isStopped = false;

        _repathTimer -= Time.deltaTime;
        if ( _repathTimer > 0f ) return;

        _repathTimer = Random.Range( _repathMin, _repathMax );

        // occasional hesitation burst
        if ( Random.value < _hesitationChance )
        {
            _hesitationTimer = _hesitationTime;
            return;
        }

        Vector3 offset = Random.insideUnitSphere * _playerOffsetRadius;
        offset.y = 0f;

        Vector3 target = _player.position + offset;
        _agent.SetDestination( target );
    }
}