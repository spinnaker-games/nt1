using UnityEngine;

public class EnemyPlayerKilledState : EnemyBaseState
{
    readonly int PlayerKilledAnimHash = Animator.StringToHash("PlayerKilled");

    const float CrossFadeDuration = 0.2f;

    float _duration = 5f; //TODO: Expose or redo duration relative to animation length

    public EnemyPlayerKilledState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        FacePlayer();
        _stateMachine.Animator.CrossFadeInFixedTime(PlayerKilledAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        _duration -= deltaTime;

        if (_duration <= 0f)
        {
            _stateMachine.SwitchState(new EnemyPatrolState(_stateMachine));
        }
    }


    public override void Exit()
    {
    }
}