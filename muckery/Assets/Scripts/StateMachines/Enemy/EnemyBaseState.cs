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

    protected bool IsPlayerDead()
    {
        return _stateMachine.PlayerHealth.IsDead;
    }

    protected float GetDistanceToPlayerSqr()
    {
        if ( _stateMachine.Player == null )
        {
            return float.MaxValue;
        }

        Vector3 origin = _stateMachine.transform.position + Vector3.up;
        Vector3 target = _stateMachine.Player.transform.position + Vector3.up;

        Vector3 toPlayer = target - origin;

        return toPlayer.sqrMagnitude;
    }

    protected bool IsPlayerInChaseRange()
    {
        if (IsPlayerDead()) { return false; }

        return GetDistanceToPlayerSqr() <= _stateMachine.PlayerChasingRange * _stateMachine.PlayerChasingRange;
    }

    protected bool IsPlayerInAttackRange()
    {
        if (IsPlayerDead()) { return false; }

        return GetDistanceToPlayerSqr() <= _stateMachine.PlayerAttackRange * _stateMachine.PlayerAttackRange;
    }

    bool IsPlayerInHorizontalFOV()
    {
        Vector3 forward = _stateMachine.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 toPlayer = _stateMachine.Player.transform.position - _stateMachine.transform.position;
        toPlayer.y = 0f;

        float sqrMag = toPlayer.sqrMagnitude;
        if ( sqrMag < 0.0001f ) { return true; }

        Vector3 dir = toPlayer / Mathf.Sqrt( sqrMag );

        float halfAngleRad = _stateMachine.HorizontalFOV * 0.5f * Mathf.Deg2Rad;
        float cosThreshold = Mathf.Cos( halfAngleRad );

        float dot = Vector3.Dot( forward, dir ); //TODO: understand Vector3.Dot

        return dot >= cosThreshold;
    }

    bool IsPlayerInVerticalFOV()
    {
        if (_stateMachine.Player == null) return false;

        Vector3 toPlayer = _stateMachine.Player.transform.position - _stateMachine.transform.position;

        toPlayer = Vector3.ProjectOnPlane( toPlayer, _stateMachine.transform.right );

        Vector3 forward = Vector3.ProjectOnPlane( _stateMachine.transform.forward, _stateMachine.transform.right );

        float distance = toPlayer.magnitude;

        if (distance < 0.0001f)
            return true;

        forward.Normalize();
        toPlayer.Normalize();

        float angle = Vector3.Angle( forward, toPlayer );

        float extraCloseRangeAngle = Mathf.Lerp( //TODO: Understand how this math makes the beginning more narrow
            25f,
            0f,
            Mathf.InverseLerp( 0f, 5f, distance )
        );

        float allowedAngle = (_stateMachine.VerticalFOV * 0.5f) + extraCloseRangeAngle;

        return angle <= allowedAngle;
    }

    protected bool HasLineOfSightToPlayer()
    {
        if ( _stateMachine.Player == null ) { return false; }

        Transform playerTransform = _stateMachine.Player.transform;

        Vector3 origin = _stateMachine.transform.position + Vector3.up;
        Vector3 target = playerTransform.position + Vector3.up;

        Vector3 rayDir = target - origin;
        float rayDist = rayDir.magnitude;

        if ( rayDist < 0.0001f ) { return true; }

        rayDir /= rayDist;

        if ( Physics.Raycast( origin, rayDir, out RaycastHit hit, rayDist ) )
        {
            return hit.transform == playerTransform;
        }

        return true;
    }

    bool IsPlayerDisguised()
    {
        return _stateMachine.PlayerStateMachine != null && _stateMachine.PlayerStateMachine.IsDisguised;
    }

    bool IsPlayerMoving()
    {
        return _stateMachine.PlayerStateMachine != null && _stateMachine.PlayerStateMachine.IsMoving;
    }

    protected bool CanSeePlayer()
    {
        if ( _stateMachine.PlayerStateMachine == null ) { return false; }

        if ( IsPlayerDisguised() && !IsPlayerMoving() ) { return false; }
        if ( !IsPlayerInChaseRange() ) { return false; }
        if ( !IsPlayerInHorizontalFOV() ) { return false; }
        if ( !IsPlayerInVerticalFOV() ) { return false; }
        if ( !HasLineOfSightToPlayer() ) { return false; }

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