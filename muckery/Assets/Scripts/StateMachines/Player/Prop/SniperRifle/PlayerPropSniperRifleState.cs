using UnityEngine;

public class PlayerPropSniperRifleState : PlayerBaseState
{
    readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    public PlayerPropSniperRifleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.TargetEvent += OnTarget;
        _stateMachine.InputReader.MorphActivateEvent += OnMorph;
        _stateMachine.InputReader.AimActivateEvent += OnAim;

        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.SniperRifle.SetActive(true);

        _stateMachine.Animator.Play(FreeLookBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {    
        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        _stateMachine.InputReader.TargetEvent -= OnTarget;
        _stateMachine.InputReader.MorphActivateEvent -= OnMorph;
        _stateMachine.InputReader.AimActivateEvent -= OnAim;

        _stateMachine.SniperRifle.SetActive(false);
    }

    void OnTarget()
    {
        if (!_stateMachine.Targeter.SelectTarget()) { return; }

        _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
    }

    void OnMorph()
    {
        _stateMachine.SwitchState(new PlayerMorphingState(_stateMachine));
    }

    void OnAim()
    {
        _stateMachine.SwitchState(new PlayerPropSniperRifleAimState(_stateMachine));
    }
}