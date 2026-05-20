using UnityEngine;

public class PlayerPropPropaneFallingState : PlayerBaseState
{
    readonly int JumpEndAnimHash = Animator.StringToHash("JumpEnd");

    const float CrossFadeDuration = 0.2f;

    public PlayerPropPropaneFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {

        _stateMachine.Animator.CrossFadeInFixedTime(JumpEndAnimHash, CrossFadeDuration);

        _stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        
        //Call Move Twice: One for jumping and one for directional motion
        //Move(_momentum, deltaTime);
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);
        FaceMovementDirection(movement, deltaTime);


        if (_stateMachine.CharacterController.isGrounded)
        {
            if (_stateMachine.Targeter.CurrentTarget != null)
            {
                //_stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
            }
            else
            {
                _stateMachine.SwitchState(new PlayerPropPropaneState(_stateMachine));//TODO: Add support for returning to other camera states by caching lastKnownCameraState
            }
        }

        FaceTarget();
    }

    public override void Exit()
    {
        _stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        //_stateMachine.SwitchState(new PlayerHangingState(_stateMachine, ledgeForward, closestPoint));
    }
}