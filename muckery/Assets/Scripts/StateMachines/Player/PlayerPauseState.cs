using UnityEngine;

public class PlayerPauseState : PlayerBaseState
{
    public PlayerPauseState( PlayerStateMachine stateMachine ) : base( stateMachine )
    {
    }

    public override void Enter()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _stateMachine.InputReader.PauseActivateEvent += OnPause;

        _stateMachine.PauseMenu.SetActive(true);
    }

    public override void Tick( float deltaTime )
    {
    }

    public override void Exit()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _stateMachine.InputReader.PauseActivateEvent -= OnPause;

        _stateMachine.PauseMenu.SetActive(false);
    }

    void OnPause()
    {
        _stateMachine.SwitchState( new PlayerFreeLookState(_stateMachine) );
    }
}