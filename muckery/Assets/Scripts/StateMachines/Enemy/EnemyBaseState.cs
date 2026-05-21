using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine _stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        _stateMachine.CharacterController.Move((movement + _stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FacePlayer()
    {
        if (_stateMachine.Player == null) { return; }

        Vector3 lookPos = _stateMachine.Player.transform.position - _stateMachine.transform.position;
        lookPos.y = 0;

        _stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);//TODO: implement smooth turning
    }

    protected void FaceTargetEscape()
    {
        if (_stateMachine.TargetEscape == null) { return; }

        Vector3 lookPos = _stateMachine.TargetEscape.transform.position - _stateMachine.transform.position;
        lookPos.y = 0;

        _stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);//TODO: implement smooth turning
    }

    protected bool IsInChaseRange()
    {
        if (_stateMachine.Player.GetComponent<Health>().IsDead) { return false; }

        float playerDistanceSqr = (_stateMachine.Player.transform.position - _stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= _stateMachine.PlayerChasingRange * _stateMachine.PlayerChasingRange;
    }

    protected bool IsInAttackRange()
    {
        if (_stateMachine.Player.GetComponent<Health>().IsDead) { return false; }

        float playerDistanceSqr = (_stateMachine.Player.transform.position - _stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= _stateMachine.PlayerAttackRange * _stateMachine.PlayerAttackRange;
    }

    protected bool CanSeePlayer( float viewDistance, float viewAngle )
    {
        if ( _stateMachine.Player == null ) { return false; }

        PlayerStateMachine player = _stateMachine.Player.GetComponent<PlayerStateMachine>();

        if (player.IsDisguised && !player.IsMoving) { return false; }

        Transform playerTransform = _stateMachine.Player.transform;

        Vector3 origin = _stateMachine.transform.position + Vector3.up * _stateMachine.EyeHeight;//TODO: Understand Vector math that creates the raycast
        Vector3 target = playerTransform.position + Vector3.up * _stateMachine.EyeHeight;

        Vector3 toPlayer = target - origin;

        float distanceSqr = toPlayer.sqrMagnitude;
        if ( distanceSqr > viewDistance * viewDistance ) { return false; }

        Vector3 forward = _stateMachine.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 toPlayerDir = toPlayer;
        toPlayerDir.y = 0;

        if ( toPlayerDir.sqrMagnitude < 0.0001f ) { return true; }

        toPlayerDir.Normalize();

        float angle = Vector3.Angle( forward, toPlayerDir );
        if ( angle > viewAngle * 0.5f ) { return false; }

        if ( Physics.Raycast( origin, toPlayerDir, out RaycastHit hit, viewDistance ) )
        {
            if ( hit.transform != playerTransform )
            {
                return false;
            }
        }

        return true;
    }

    protected void FaceMovementDirection( Vector3 direction, float deltaTime )
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

        _stateMachine.transform.rotation = Quaternion.Slerp(
            _stateMachine.transform.rotation,
            targetRotation,
            deltaTime * 10f
        );
    }
}