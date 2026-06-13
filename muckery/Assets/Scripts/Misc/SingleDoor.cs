using UnityEngine;
using TMPro;

public class SingleDoor : MonoBehaviour
{
    [SerializeField] GameObject _doorButton;
    [SerializeField] TMP_Text _doorButtonText;
    [SerializeField] Animator _animator;
    [SerializeField] InputReader _inputReader;
    [SerializeField] bool _locked;

    readonly int DoorOpenInwardHash = Animator.StringToHash( "SingleDoorOpenInward" );
    readonly int DoorOpenOutwardHash = Animator.StringToHash( "SingleDoorOpenOutward" );
    readonly int DoorCloseHash = Animator.StringToHash( "SingleDoorClose" );

    bool _playerInRange;
    bool _playerLockPick;
    bool _isOpen;

    void OnTriggerEnter( Collider other )
    {
        if ( !other.CompareTag( "Player" ) ) return;

        PlayerStateMachine player = other.GetComponent<PlayerStateMachine>(); //TODO: Tight Coupling??? Can I do this better?

        _playerLockPick = player.IsLockPick;

        _playerInRange = true;

        if ( _doorButton != null )
        {
            _doorButton.SetActive( true );
            UpdateButtonText();
        }

        _inputReader.InteractActivateEvent += OnInteract;
    }

    void OnTriggerExit( Collider other )
    {
        if ( !other.CompareTag( "Player" ) ) return;

        _playerInRange = false;

        if ( _doorButton != null )
        {
            _doorButton.SetActive( false );
        }

        _inputReader.InteractActivateEvent -= OnInteract;
    }

    void OnInteract()
    {
        if ( !_playerInRange ) return;

        if (_locked && !_playerLockPick) return;

        _isOpen = !_isOpen;

        _locked = false;

        UpdateButtonText();

        if ( _isOpen )
        {
            _animator.Play( DoorOpenInwardHash );
        }
        else
        {
            _animator.Play( DoorCloseHash );
        }
    }

    void UpdateButtonText()
    {
        if ( _doorButtonText == null ) return;

        if (_locked && !_playerLockPick)
        {
            _doorButtonText.text = "Locked";
            return;
        }

        if (_locked && _playerLockPick)
        {
            _doorButtonText.text = "[E] Unlock";
            return;
        }

        _doorButtonText.text = _isOpen ? "[E] Close" : "[E] Open"; //TODO: Set this up better
    }
}