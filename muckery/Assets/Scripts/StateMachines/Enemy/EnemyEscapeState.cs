using UnityEngine;

public class EnemyEscapeState : EnemyBaseState
{
    readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    readonly int SpeedHash = Animator.StringToHash("Speed");//TODO: Come up with beeter name for 'Speed' animator variable

    const float CrossFadeDuration = 0.1f;
    const float AnimatorDampTime = 0.1f;

    public EnemyEscapeState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        Debug.Log("TARGET IS ESCAPING");
    }

    public override void Tick(float deltaTime)
    {    
        MoveToEscape(deltaTime);
        FaceTargetEscape();
        _stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        _stateMachine.NavMeshAgent.ResetPath();
        _stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }

    void MoveToEscape(float deltaTime)
    {
        if (_stateMachine.NavMeshAgent.isOnNavMesh)
        {
            Move(_stateMachine.NavMeshAgent.desiredVelocity.normalized * _stateMachine.MovementSpeed, deltaTime);
            _stateMachine.NavMeshAgent.destination = _stateMachine.TargetEscape.transform.position;
        }
            _stateMachine.NavMeshAgent.velocity = _stateMachine.CharacterController.velocity; //This ensures that the NavMeshAgent and the CharacterController are in sync
    }
}