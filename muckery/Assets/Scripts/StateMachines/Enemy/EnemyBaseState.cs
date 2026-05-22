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

    protected bool IsInChaseRange() //TODO: Create a GetDistanceToPlayerSqr function
    {
        if ( _stateMachine.Player.GetComponent<Health>().IsDead ) { return false; }

        Vector3 origin = _stateMachine.transform.position + Vector3.up * _stateMachine.EyeHeight;
        Vector3 target = _stateMachine.Player.transform.position + Vector3.up * _stateMachine.EyeHeight;

        Vector3 toPlayer = target - origin;

        float distanceSqr = toPlayer.sqrMagnitude;

        return distanceSqr <= _stateMachine.PlayerChasingRange * _stateMachine.PlayerChasingRange;
    }

    protected bool IsInAttackRange() //TODO: Create a GetDistanceToPlayerSqr function
    {
        if ( _stateMachine.Player.GetComponent<Health>().IsDead ) { return false; }

        Vector3 origin = _stateMachine.transform.position + Vector3.up * _stateMachine.EyeHeight;
        Vector3 target = _stateMachine.Player.transform.position + Vector3.up * _stateMachine.EyeHeight;

        Vector3 toPlayer = target - origin;

        float distanceSqr = toPlayer.sqrMagnitude;

        return distanceSqr <= _stateMachine.PlayerAttackRange * _stateMachine.PlayerAttackRange;
    }

    protected bool CanSeePlayer( float viewDistance, float viewAngle ) //TODO: Make Raycast Feild of View component
    //TODO: Create a GetDistanceToPlayerSqr function
    {
        if ( _stateMachine.Player == null ) { return false; }

        PlayerStateMachine player = _stateMachine.Player.GetComponent<PlayerStateMachine>();

        if ( player.IsDisguised && !player.IsMoving ) { return false; }

        Transform playerTransform = _stateMachine.Player.transform;

        Vector3 origin = _stateMachine.transform.position + Vector3.up * _stateMachine.EyeHeight;
        Vector3 target = playerTransform.position + Vector3.up * _stateMachine.EyeHeight;

        Vector3 toPlayer = target - origin;

        float distanceSqr = toPlayer.sqrMagnitude;
        if ( distanceSqr > viewDistance * viewDistance ) { return false; }

        Vector3 forward = _stateMachine.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 toPlayerDirFlat = toPlayer;
        toPlayerDirFlat.y = 0f;

        if ( toPlayerDirFlat.sqrMagnitude < 0.0001f ) { return true; }

        toPlayerDirFlat.Normalize();

        float angle = Vector3.Angle( forward, toPlayerDirFlat );
        if ( angle > viewAngle * 0.5f ) { return false; }

        Vector3 rayDir = ( target - origin );
        float rayDist = rayDir.magnitude;

        if ( rayDist < 0.0001f ) { return true; }

        rayDir /= rayDist;

        if ( Physics.Raycast( origin, rayDir, out RaycastHit hit, rayDist ) )
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