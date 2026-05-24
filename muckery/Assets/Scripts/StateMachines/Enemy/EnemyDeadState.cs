using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Ragdoll.ToggleRagdoll(true);
        _stateMachine.Weapon.gameObject.SetActive(false);
        GameObject.Destroy(_stateMachine.Target); //TODO: Object pool

        GameSession.Instance.MarkTargetEliminated(); //TODO: Fix Tight Coupling

        Debug.Log("TARGET ELIMINATED!!! EXTRACTION NOW AVAILLABLE!!!");
        _stateMachine.IsDead = true;

        _stateMachine.DeathSFX.Play();
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}