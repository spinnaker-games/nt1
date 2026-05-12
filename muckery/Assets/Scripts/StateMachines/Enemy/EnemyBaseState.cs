using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine _stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        _stateMachine.CharacterController.Move((movement + _stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FacePlayer()
    {
        if (_stateMachine.Player == null) { return; }

        Vector3 lookPos = _stateMachine.Player.transform.position - _stateMachine.transform.position;
        lookPos.y = 0;

        _stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);//TODO: implement smooth turning
    }

    protected bool IsInChaseRange()
    {
        if (_stateMachine.Player.GetComponent<Health>().IsDead) { return false; }

        float playerDistanceSqr = (_stateMachine.Player.transform.position - _stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= _stateMachine.PlayerChasingRange * _stateMachine.PlayerChasingRange;
    }

    protected bool IsInAttackRange()
    {
        if (_stateMachine.Player.GetComponent<Health>().IsDead) { return false; }

        float playerDistanceSqr = (_stateMachine.Player.transform.position - _stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= _stateMachine.PlayerAttackRange * _stateMachine.PlayerAttackRange;
    }
}