using UnityEngine;

public class PlayerPropScanCameraZoomState : PlayerBaseState
{
    float _defaultFOV;

    public PlayerPropScanCameraZoomState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.MorphSlotActivateEvent += OnMorphSlot;
        _stateMachine.InputReader.AbilityActivateEvent += OnAbilityActivate;
        _stateMachine.InputReader.ZoomCancelEvent += OnZoomCancel;

        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.ScanCamera.SetActive(true);

        _stateMachine.ScanCameraScope.SetActive(true);
        _stateMachine.ScanCamera.SetActive(false);

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
        _stateMachine.InputReader.AbilityActivateEvent -= OnAbilityActivate;
        _stateMachine.InputReader.MorphSlotActivateEvent -= OnMorphSlot;
        _stateMachine.InputReader.ZoomCancelEvent -= OnZoomCancel;

        _stateMachine.ScanCameraScope.SetActive(false);
        _stateMachine.ScanCamera.SetActive(true);

        _stateMachine.FreeLookVC.Lens.FieldOfView = _defaultFOV;
    }

    void OnMorphSlot( int slot )
    {
        _stateMachine.SwitchState( new PlayerMorphingState( _stateMachine, slot + 1 ) );
    }

    void OnZoomCancel()
    {
        _stateMachine.SwitchState(new PlayerPropScanCameraState(_stateMachine));
    }

    void OnAbilityActivate()
    {
        Camera camera = Camera.main;

        _stateMachine.CameraSnapSFX.Play();

        Ray ray = camera.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 0f ) );

        if ( Physics.Raycast( ray, out RaycastHit hit, 1000f ) )
        {
            CodexEntry codexEntry = hit.collider.GetComponentInParent<CodexEntry>();

            if ( codexEntry != null )
            {
                Debug.Log(codexEntry.GetCodexEntry());
                //TODO: Apply Force to enemy being hit
            }
            else
            {
                Debug.Log("NO CODEX ENTRY AVAILABLE");
            }
        }
    }
}