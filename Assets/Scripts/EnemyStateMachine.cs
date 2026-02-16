using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] Transform _player;

    [Header("Distances")]
    [SerializeField] float _detectDistance = 10f;
    [SerializeField] float _loseDistance = 16f;
    [SerializeField] float _stopBuffer = 1.4f;

    [Header("Movement")]
    [SerializeField] float _baseSpeed = 3.5f;
    [SerializeField] float _speedVariance = 0.6f;
    [SerializeField] float _repathMin = 0.4f;
    [SerializeField] float _repathMax = 1.2f;

    [Header("Roaming")]
    [SerializeField] float _roamRadius = 8f;
    [SerializeField] float _roamDelayMin = 1.5f;
    [SerializeField] float _roamDelayMax = 4f;

    [Header("Unpredictability")]
    [SerializeField] float _playerOffsetRadius = 1.8f;
    [SerializeField] float _hesitationChance = 0.15f;
    [SerializeField] float _hesitationTime = 0.6f;

    NavMeshAgent _agent;
    Vector3 _spawnPoint;

    float _repathTimer;
    float _hesitationTimer;
    float _roamTimer;
    float _mySpeed;

    bool _playerIsInvisible = false; // new flag

    IEnemyState _currentState;

    void OnEnable()
    {
        InvisibilityMask.OnInvisibilityMaskToggled += HandleInvisibilityToggled;
    }

    void OnDisable()
    {
        InvisibilityMask.OnInvisibilityMaskToggled -= HandleInvisibilityToggled;
    }

    void HandleInvisibilityToggled(bool isInvisible)
    {
        _playerIsInvisible = isInvisible;

        if (_playerIsInvisible)
            SwitchState(new RoamState(this)); // immediately start roaming
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = _stopBuffer;

        _spawnPoint = transform.position;
        _mySpeed = _baseSpeed + Random.Range(-_speedVariance, _speedVariance);
        _agent.speed = _mySpeed;

        _currentState = new RoamState(this);
    }

    void Update()
    {
        if (_player == null) return;

        _currentState.UpdateState();
    }

    public void SwitchState(IEnemyState newState)
    {
        _currentState = newState;
    }

    public interface IEnemyState
    {
        void UpdateState();
    }

    // Roaming state
    class RoamState : IEnemyState
    {
        EnemyStateMachine _enemy;

        public RoamState(EnemyStateMachine enemy)
        {
            _enemy = enemy;
            _enemy._roamTimer = Random.Range(_enemy._roamDelayMin, _enemy._roamDelayMax);
        }

        public void UpdateState()
        {
            // Only switch to chase if player is not invisible
            if (!_enemy._playerIsInvisible)
            {
                float distance = Vector3.Distance(_enemy.transform.position, _enemy._player.position);
                if (distance <= _enemy._detectDistance)
                {
                    _enemy.SwitchState(new ChaseState(_enemy));
                    return;
                }
            }

            _enemy._roamTimer -= Time.deltaTime;
            if (_enemy._roamTimer <= 0f)
            {
                _enemy._roamTimer = Random.Range(_enemy._roamDelayMin, _enemy._roamDelayMax);
                Vector3 randomDir = Random.insideUnitSphere * _enemy._roamRadius + _enemy._spawnPoint;
                if (NavMesh.SamplePosition(randomDir, out var hit, _enemy._roamRadius, NavMesh.AllAreas))
                {
                    _enemy._agent.SetDestination(hit.position);
                    _enemy._agent.isStopped = false;
                }
            }
        }
    }

    // Chase state
    class ChaseState : IEnemyState
    {
        EnemyStateMachine _enemy;

        public ChaseState(EnemyStateMachine enemy)
        {
            _enemy = enemy;
            _enemy._repathTimer = 0f;
            _enemy._agent.isStopped = false;
        }

        public void UpdateState()
        {
            // If player becomes invisible, immediately roam
            if (_enemy._playerIsInvisible)
            {
                _enemy.SwitchState(new RoamState(_enemy));
                return;
            }

            float distance = Vector3.Distance(_enemy.transform.position, _enemy._player.position);
            if (distance >= _enemy._loseDistance)
            {
                _enemy.SwitchState(new RoamState(_enemy));
                return;
            }

            if (_enemy._hesitationTimer > 0f)
            {
                _enemy._hesitationTimer -= Time.deltaTime;
                _enemy._agent.isStopped = true;
                return;
            }

            _enemy._agent.isStopped = false;

            _enemy._repathTimer -= Time.deltaTime;
            if (_enemy._repathTimer <= 0f)
            {
                _enemy._repathTimer = Random.Range(_enemy._repathMin, _enemy._repathMax);
                if (Random.value < _enemy._hesitationChance)
                {
                    _enemy._hesitationTimer = _enemy._hesitationTime;
                    return;
                }

                Vector3 offset = Random.insideUnitSphere * _enemy._playerOffsetRadius;
                offset.y = 0f;
                _enemy._agent.SetDestination(_enemy._player.position + offset);
            }
        }
    }
}