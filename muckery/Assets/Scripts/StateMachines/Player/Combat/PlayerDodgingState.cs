using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    
    readonly int DodgingBlendTreeHash = Animator.StringToHash("DodgingBlendTree");
    readonly int DodgingForwardBlendSpeedHash = Animator.StringToHash("DodgingForwardBlendSpeed");
    readonly int DodgingRightBlendSpeedHash = Animator.StringToHash("DodgingRightBlendSpeed");

    float _remainingDodgeTime;
    Vector3 _dodgingDirectionInput;

    const float CrossFadeDuration = 0.2f;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
    {
        this._dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter()
    {
        _remainingDodgeTime = _stateMachine.PlayerConfig.DodgeDuration;

        _stateMachine.Animator.SetFloat(DodgingForwardBlendSpeedHash, _dodgingDirectionInput.y);
        _stateMachine.Animator.SetFloat(DodgingRightBlendSpeedHash, _dodgingDirectionInput.x);
        _stateMachine.Animator.CrossFadeInFixedTime(DodgingBlendTreeHash, CrossFadeDuration);

        _stateMachine.Health.SetInvulnerable(true); //TODO: create a bool in state machine for this
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += _stateMachine.transform.right * _dodgingDirectionInput.x * _stateMachine.PlayerConfig.DodgeDistance / _stateMachine.PlayerConfig.DodgeDuration;
        movement += _stateMachine.transform.forward * _dodgingDirectionInput.y * _stateMachine.PlayerConfig.DodgeDistance / _stateMachine.PlayerConfig.DodgeDuration;

        Move(movement, deltaTime);

        FaceTarget();

        _remainingDodgeTime -= deltaTime;

        if(_remainingDodgeTime <= 0f)
        {
            _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
        }
    }

    public override void Exit()
    {
        _stateMachine.Health.SetInvulnerable(false);
    }
}