using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    readonly int DeathAnimHash = Animator.StringToHash("Death");

    const float CrossFadeDuration = 0.2f;

    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(DeathAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
    }


    public override void Exit()
    {
    }
}