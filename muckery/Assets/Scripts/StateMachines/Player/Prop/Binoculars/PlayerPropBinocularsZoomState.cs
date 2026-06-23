using UnityEngine;

public class PlayerPropBinocularsZoomState : PlayerBaseState
{
    float _defaultFOV;

    public PlayerPropBinocularsZoomState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.AimCancelEvent += OnAimCancel;

        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.Binoculars.SetActive(true);

        _stateMachine.BinocularsScope.SetActive(true);
        _stateMachine.Binoculars.SetActive(false);

        Vector3 cameraForward = _stateMachine.MainCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        _stateMachine.transform.forward = cameraForward;
        
        _defaultFOV = _stateMachine.FreeLookVC.Lens.FieldOfView;
        _stateMachine.FreeLookVC.Lens.FieldOfView = _stateMachine.BinocularsFOV;
    }

    public override void Tick(float deltaTime)
    {    
    }

    public override void Exit()
    {
        _stateMachine.InputReader.AimActivateEvent -= OnAimCancel;

        _stateMachine.BinocularsScope.SetActive(false);
        _stateMachine.Binoculars.SetActive(true);

        _stateMachine.FreeLookVC.Lens.FieldOfView = _defaultFOV;
    }

    void OnAimCancel()
    {
        _stateMachine.SwitchState(new PlayerPropBinocularsState(_stateMachine));
    }
}