using UnityEngine;

public class PlayerPropSniperRifleZoomState : PlayerBaseState
{
    float _defaultFOV;

    public PlayerPropSniperRifleZoomState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.AbilityActivateEvent += OnAbilityActivate;
        _stateMachine.InputReader.MorphSlotActivateEvent += OnMorphSlot;
        _stateMachine.InputReader.ZoomCancelEvent += OnZoomCancel;

        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.SniperRifle.SetActive(true);

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
        _stateMachine.InputReader.AbilityActivateEvent -= OnAbilityActivate;
        _stateMachine.InputReader.MorphSlotActivateEvent -= OnMorphSlot;
        _stateMachine.InputReader.ZoomCancelEvent -= OnZoomCancel;

        _stateMachine.SniperRifleScope.SetActive(false);
        _stateMachine.SniperRifle.SetActive(true);

        _stateMachine.FreeLookVC.Lens.FieldOfView = _defaultFOV;
    }

    void OnMorphSlot( int slot )
    {
        _stateMachine.SwitchState( new PlayerMorphingState( _stateMachine, slot + 1 ) );
    }

    void OnZoomCancel()
    {
        _stateMachine.SwitchState(new PlayerPropSniperRifleState(_stateMachine));
    }

    void OnAbilityActivate()
    {
        Camera camera = Camera.main;

        _stateMachine.SniperShotSFX.Play();

        Ray ray = camera.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 0f ) );

        if ( Physics.Raycast( ray, out RaycastHit hit, 1000f ) )
        {
            Health health = hit.collider.GetComponentInParent<Health>();

            if ( health != null )
            {
                health.DealDamage(_stateMachine.SniperDamageAmount);
                //TODO: Apply Force to enemy being hit
            }
        }
    }
}