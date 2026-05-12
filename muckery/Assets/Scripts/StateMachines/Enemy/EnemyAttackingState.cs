using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    readonly int AttackAnimHash = Animator.StringToHash("Attack");

    const float CrossFadeDuration = 0.2f;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        FacePlayer(); //TODO: Explore adding this to Tick method to make enemy encounter harder

        _stateMachine.Weapon.SetAttack(_stateMachine.DamageAmount, _stateMachine.AttackKnockback);

        _stateMachine.Animator.CrossFadeInFixedTime(AttackAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(_stateMachine.Animator, "Attack") >= 1)
        {
            _stateMachine.SwitchState(new EnemyChasingState(_stateMachine));
        }
    }

    public override void Exit()
    {
    }
}