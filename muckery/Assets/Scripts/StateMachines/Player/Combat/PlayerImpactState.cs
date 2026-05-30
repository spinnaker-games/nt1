using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    readonly int ImpactAnimHash = Animator.StringToHash("Impact");

    const float CrossFadeDuration = 0.2f;

    float _duration = 1f;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(ImpactAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        _duration -= deltaTime;

        if (_duration <= 0f)
        {
            ReturnToLocomotion();
        }
    }


    public override void Exit()
    {
    }
}