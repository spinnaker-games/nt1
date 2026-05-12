using System;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    readonly int SpeedHash = Animator.StringToHash("Speed");//TODO: Come up with beeter name for 'Speed' animator variable

    const float CrossFadeDuration = 0.1f;
    const float AnimatorDampTime = 0.1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange())
        {
            _stateMachine.SwitchState(new EnemyIdleState(_stateMachine));
            return;
        }
        else if(IsInAttackRange()) // leaving IsInAttackRange in EnemyBaseState because i want some enemies to attack the player from states other than chasing state 
        {
            _stateMachine.SwitchState(new EnemyAttackingState(_stateMachine));
            return;
        }
    
        MoveToPlayer(deltaTime);
        FacePlayer();
        _stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        _stateMachine.NavMeshAgent.ResetPath();
        _stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }

    void MoveToPlayer(float deltaTime)
    {
        if (_stateMachine.NavMeshAgent.isOnNavMesh)
        {
            /*This setup uses the NavMeshAgent for pathfinding while the CharacterController handles physics-based 
            movement, syncing velocities so the agent follows the path realistically with collision and slope handling.*/
            Move(_stateMachine.NavMeshAgent.desiredVelocity.normalized * _stateMachine.MovementSpeed, deltaTime);
            _stateMachine.NavMeshAgent.destination = _stateMachine.Player.transform.position;
        }
            _stateMachine.NavMeshAgent.velocity = _stateMachine.CharacterController.velocity; //This ensures that the NavMeshAgent and the CharacterController are in sync
    }
}