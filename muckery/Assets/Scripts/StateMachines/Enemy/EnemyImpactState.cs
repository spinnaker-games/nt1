using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    readonly int ImpactAnimHash = Animator.StringToHash("Impact");

    const float CrossFadeDuration = 0.2f;

    float _duration = 1f;

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
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
            _stateMachine.SwitchState(new EnemyIdleState(_stateMachine));
        }
    }


    public override void Exit()
    {
    }
}