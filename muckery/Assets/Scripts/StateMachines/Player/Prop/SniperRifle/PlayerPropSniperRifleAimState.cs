using UnityEngine;

public class PlayerPropSniperRifleAimState : PlayerBaseState
{
    float _defaultFOV;

    public PlayerPropSniperRifleAimState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.MorphActivateEvent += OnMorph;
        _stateMachine.InputReader.AimCancelEvent += OnAimCancel;

        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.SniperRifle.SetActive(true);

        _stateMachine.IsMorphed = true;
        _stateMachine.SniperRifleScope.SetActive(true);
        _stateMachine.SniperRifle.SetActive(false);

        Vector3 cameraForward = _stateMachine.MainCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        _stateMachine.transform.forward = cameraForward;
        
        _defaultFOV = _stateMachine.FreeLookVC.Lens.FieldOfView;
        _stateMachine.FreeLookVC.Lens.FieldOfView = _stateMachine.SniperFOV;
    }

    public override void Tick(float deltaTime)
    {    
    }

    public override void Exit()
    {
        _stateMachine.InputReader.MorphActivateEvent -= OnMorph;
        _stateMachine.InputReader.AimActivateEvent -= OnAimCancel;

        _stateMachine.SniperRifleScope.SetActive(false);
        _stateMachine.SniperRifle.SetActive(true);

        _stateMachine.FreeLookVC.Lens.FieldOfView = _defaultFOV;
    }

    void OnMorph()
    {
        _stateMachine.SwitchState(new PlayerMorphingState(_stateMachine));
    }

    void OnAimCancel()
    {
        _stateMachine.SwitchState(new PlayerPropSniperRifleState(_stateMachine));
    }
}