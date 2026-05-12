using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    readonly int SpeedHash = Animator.StringToHash("Speed");//TODO: Come up with beeter name for 'Speed' animator variable

    const float CrossFadeDuration = 0.2f;
    const float AnimatorDampTime = 0.1f;


    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        _stateMachine.Animator.SetFloat(SpeedHash, 0);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
    
        if (IsInChaseRange())
        {
            _stateMachine.SwitchState(new EnemyChasingState(_stateMachine));
            return;
        }

        FacePlayer();

        _stateMachine.Animator.SetFloat(SpeedHash, 0, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
    }
}