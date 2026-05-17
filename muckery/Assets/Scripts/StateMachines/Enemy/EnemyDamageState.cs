using UnityEngine;

public class EnemyDamageState : EnemyBaseState
{
    readonly int ImpactAnimHash = Animator.StringToHash("Impact");

    const float CrossFadeDuration = 0.2f;

    float _duration = 1f;

    public EnemyDamageState(EnemyStateMachine stateMachine) : base(stateMachine)
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
            if (_stateMachine.IsTarget)
            {
                _stateMachine.SwitchState(new EnemyEscapeState(_stateMachine));
                return;
            }
            _stateMachine.SwitchState(new EnemyIdleState(_stateMachine));
        }
    }


    public override void Exit()
    {
    }
}