using UnityEngine;

public class PlayerPropKnifeAttackingState : PlayerBaseState
{
    readonly int KnifeAttackAnimHash = Animator.StringToHash("KnifeAttack");

    const float CrossFadeDuration = 0.2f;

    float _duration = 1f;

    public PlayerPropKnifeAttackingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        FaceCameraDirection();
        _stateMachine.Knife.SetActive(true);

        _stateMachine.Animator.CrossFadeInFixedTime(KnifeAttackAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        _duration -= deltaTime;

        if (_duration <= 0f)
        {
            _stateMachine.SwitchState(new PlayerPropKnifeState(_stateMachine));//TODO: Add support for returning to other camera states by caching lastKnownCameraState
        }
    }


    public override void Exit()
    {
    }

    void FaceCameraDirection() //TODO: Add to base state
    {
        Vector3 cameraForward = Camera.main.transform.forward;

        cameraForward.y = 0f;
        cameraForward.Normalize();

        if (cameraForward.sqrMagnitude > 0f)
        {
            _stateMachine.transform.rotation = Quaternion.LookRotation(cameraForward);
        }
    }
}