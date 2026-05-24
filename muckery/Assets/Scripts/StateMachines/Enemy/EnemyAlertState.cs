using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    readonly int AlertAnimHash = Animator.StringToHash("Alert");

    const float CrossFadeDuration = 0.2f;

    float _duration = 1.8f; //TODO: Expose or redo duration relative to animation length

    public EnemyAlertState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _stateMachine.AlertSXF.Play();
    }

    public override void Enter()
    {
        FacePlayer();
        _stateMachine.Animator.CrossFadeInFixedTime(AlertAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer();
        Move(deltaTime);

        _duration -= deltaTime;

        if (_duration <= 0f)
        {
            if (_stateMachine.IsTarget)
            {
                _stateMachine.SwitchState(new EnemyEscapeState(_stateMachine));
                return;
            }
            _stateMachine.SwitchState(new EnemyChasingState(_stateMachine));
        }
    }


    public override void Exit()
    {
    }
}