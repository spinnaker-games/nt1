using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float _speed = 10f;

    [Header("Player Animation")]
    [SerializeField] Animator _animator;

    InputActions _inputActions;
    Vector2 _moveInputValue;

    Rigidbody _rb;

    void Awake()
    {
        _inputActions = new InputActions();
        _rb = GetComponent<Rigidbody>();

        _rb.freezeRotation = true;
    }

    void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
    }

    void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
        _inputActions.Player.Disable();
    }

    void OnMove( InputAction.CallbackContext context )
    {
        _moveInputValue = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 horizontal = new Vector3( _moveInputValue.x, 0f, _moveInputValue.y );

        if ( horizontal.sqrMagnitude > 0.001f )
        {
            Vector3 move = horizontal.normalized;

            _rb.MovePosition(
                _rb.position + move * _speed * Time.fixedDeltaTime
            );

            _animator.SetBool( "IsRunning", true );
        }
        else
        {
            _animator.SetBool( "IsRunning", false );
        }
    }
}