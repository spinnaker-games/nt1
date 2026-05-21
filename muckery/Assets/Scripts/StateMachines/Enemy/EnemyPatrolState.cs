using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    readonly int SpeedHash = Animator.StringToHash("Speed");

    const float CrossFadeDuration = 0.2f;
    const float AnimatorDampTime = 0.1f;
    const float WaypointReachThreshold = 1.5f; //TODO: Expose in state Machine???

    Transform CurrentWaypoint => _stateMachine.Waypoints[_stateMachine.CurrentWaypointIndex];

    public EnemyPatrolState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        SetDestination();
    }

    public override void Tick(float deltaTime)
    {
        MoveToWaypoint(deltaTime);

        if (CanSeePlayer(10f, 90f) && _stateMachine.IsTarget)
        {
            _stateMachine.SwitchState(new EnemyEscapeState(_stateMachine));
            return;
        }

        if (CanSeePlayer(10f, 90f) && IsInChaseRange())
        {
            _stateMachine.SwitchState(new EnemyChasingState(_stateMachine));
            return;
        }

        if (HasReachedWaypoint())
        {
            AdvanceWaypoint();
            SetDestination();
        }

        _stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        _stateMachine.NavMeshAgent.ResetPath();
        _stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }

    void MoveToWaypoint(float deltaTime)
    {
        if (!_stateMachine.NavMeshAgent.isOnNavMesh)
            return;

        Vector3 desiredVelocity = _stateMachine.NavMeshAgent.desiredVelocity;

        Move(desiredVelocity.normalized * _stateMachine.MovementSpeed, deltaTime);

        _stateMachine.NavMeshAgent.velocity = _stateMachine.CharacterController.velocity;

        FaceMovementDirection( desiredVelocity, deltaTime );
    }

    void SetDestination()
    {
        if (_stateMachine.Waypoints == null || _stateMachine.Waypoints.Length == 0)
            return;

        _stateMachine.NavMeshAgent.destination = CurrentWaypoint.position;
    }

    bool HasReachedWaypoint()
    {
        if (_stateMachine.Waypoints == null || _stateMachine.Waypoints.Length == 0)
            return false;

        if (_stateMachine.NavMeshAgent.pathPending)
            return false;

        return _stateMachine.NavMeshAgent.remainingDistance <= WaypointReachThreshold;
    }

    void AdvanceWaypoint()
    {
        if (_stateMachine.Waypoints.Length <= 1)
            return;

        _stateMachine.CurrentWaypointIndex += _stateMachine.WaypointDirection;

        if (_stateMachine.CurrentWaypointIndex >= _stateMachine.Waypoints.Length)
        {
            _stateMachine.CurrentWaypointIndex = _stateMachine.Waypoints.Length - 2;
            _stateMachine.WaypointDirection = -1;
        }
        else if (_stateMachine.CurrentWaypointIndex < 0)
        {
            _stateMachine.CurrentWaypointIndex = 1;
            _stateMachine.WaypointDirection = 1;
        }
    }
}