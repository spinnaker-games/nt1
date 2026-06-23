using UnityEngine;

public class PlayerMorphingState : PlayerBaseState
{
    float _morphTimer;
    int _morphSlot;

    public PlayerMorphingState( PlayerStateMachine stateMachine, int morphSlot = 0 ) : base( stateMachine )
    {
        _morphSlot = morphSlot;
    }

    public override void Enter()
    {
        _morphTimer = _stateMachine.MorphDuration;

        _stateMachine.MorphSFX.Play();
        _stateMachine.MorphVFX.Play();
    }

    public override void Tick( float deltaTime )
    {
        Vector3 movement = CalculateMovement();
        Move( movement * _stateMachine.FreeLookMovementSpeed, deltaTime );

        FaceMovementDirection( movement, deltaTime );

        _morphTimer -= deltaTime;

        if ( _morphTimer > 0f )
            return;

        CompleteMorph();
    }

    public override void Exit()
    {
    }

    void CompleteMorph()
    {
        if ( _stateMachine.Health.IsDead )
            return;

        Morphable target = GetMorphableTarget();

        if ( target == null )
        {
            _stateMachine.SwitchState( new PlayerFreeLookState( _stateMachine ) );
            return;
        }

        if ( _stateMachine.IsMorphed )
        {
            if ( _stateMachine.CurrentMorphInteractable == target )
            {
                _stateMachine.IsMorphed = false;
                _stateMachine.CurrentMorphInteractable = null;
                _stateMachine.SwitchState( new PlayerFreeLookState( _stateMachine ) );
                return;
            }
        }

        _stateMachine.IsMorphed = true;
        _stateMachine.CurrentMorphInteractable = target;

        switch ( target.Type )
        {
            case Morphable.MorphableType.Knife:
                _stateMachine.SwitchState( new PlayerPropKnifeState( _stateMachine ) );
                break;

            case Morphable.MorphableType.Spring:
                _stateMachine.SwitchState( new PlayerPropSpringState( _stateMachine ) );
                break;

            case Morphable.MorphableType.Barrel:
                _stateMachine.SwitchState( new PlayerPropBarrelState( _stateMachine ) );
                break;

            case Morphable.MorphableType.MetalDetector:
                _stateMachine.SwitchState( new PlayerPropMetalDetectorState( _stateMachine ) );
                break;

            case Morphable.MorphableType.SniperRifle:
                _stateMachine.SwitchState( new PlayerPropSniperRifleState( _stateMachine ) );
                break;

            case Morphable.MorphableType.Binoculars:
                _stateMachine.SwitchState( new PlayerPropBinocularsState( _stateMachine ) );
                break;

            case Morphable.MorphableType.Umbrella:
                _stateMachine.SwitchState( new PlayerPropUmbrellaState( _stateMachine ) );
                break;

            case Morphable.MorphableType.ScanCamera:
                _stateMachine.SwitchState( new PlayerPropScanCameraState( _stateMachine ) );
                break;

            case Morphable.MorphableType.LockPick:
                _stateMachine.SwitchState( new PlayerPropLockPickState( _stateMachine ) );
                break;

            case Morphable.MorphableType.GasCan:
                _stateMachine.SwitchState( new PlayerPropGasCanState( _stateMachine ) );
                break;

            case Morphable.MorphableType.BabyOil:
                _stateMachine.SwitchState( new PlayerPropBabyOilState( _stateMachine ) );
                break;
        }
    }

    Morphable GetMorphableTarget()
    {
        switch ( _morphSlot )
        {
            case 1: return _stateMachine.MorphableSlotA;
            case 2: return _stateMachine.MorphableSlotB;
            case 3: return _stateMachine.MorphableSlotC;
            default: return null;
        }
    }
}