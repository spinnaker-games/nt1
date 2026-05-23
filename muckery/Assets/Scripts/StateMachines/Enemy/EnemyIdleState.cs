using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    readonly int LocomotionBlendTreeHash = Animator.StringToHash( "Locomotion" );
    readonly int SpeedHash = Animator.StringToHash( "Speed" );

    const float CrossFadeDuration = 0.2f;
    const float AnimatorDampTime = 0.1f;

    float _idleTimer;


    public EnemyIdleState( EnemyStateMachine stateMachine ) : base( stateMachine )
    {
    }

    public override void Enter()
    {
        _idleTimer = _stateMachine.IdleDuration;

        _stateMachine.Animator.CrossFadeInFixedTime( LocomotionBlendTreeHash, CrossFadeDuration );
        _stateMachine.Animator.SetFloat( SpeedHash, 0 );
    }

    public override void Tick( float deltaTime )
    {
        Move( deltaTime );

        _idleTimer -= deltaTime;

        if ( CanSeePlayer() && _stateMachine.IsTarget )
        {
            _stateMachine.SwitchState( new EnemyEscapeState( _stateMachine ) );
            return;
        }

        if ( CanSeePlayer() )
        {
            _stateMachine.SwitchState( new EnemyAlertState( _stateMachine ) );
            return;
        }

        if ( _idleTimer <= 0f && !_stateMachine.EndlessIdle)
        {
            _stateMachine.SwitchState( new EnemyPatrolState( _stateMachine ) );
            return;
        }

        _stateMachine.Animator.SetFloat( SpeedHash, 0, AnimatorDampTime, deltaTime );
    }

    public override void Exit()
    {
    }
}