using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float _speed = 10f;

    [Header("Rotation Settings")]
    [SerializeField] bool _snapTurning = false;
    [SerializeField] float _turnRotationSpeed = 10f;

    [Header("Player Animation")]
    [SerializeField] Animator _animator;

    InputActions _inputActions;
    Vector2 _moveInputValue;

    Rigidbody _rb;

    void Awake()
    {
        _inputActions = new InputActions();
        _rb = GetComponent<Rigidbody>();

        // Lock rotation so physics doesn't tip the player
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
        _inputActions.Player.Disable();
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        _moveInputValue = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Convert 2D input to 3D horizontal movement
        Vector3 horizontal = new Vector3(_moveInputValue.x, 0f, _moveInputValue.y);

        // Handle rotation
        if (horizontal.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontal);

            transform.rotation = _snapTurning
                ? targetRotation
                : Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    _turnRotationSpeed * Time.fixedDeltaTime
                );
        }

        // Apply movement
        if (horizontal.sqrMagnitude > 0.001f)
        {
            // Transform direction relative to player rotation
            Vector3 move = transform.forward * horizontal.magnitude;

            // Move the Rigidbody
            _rb.MovePosition(_rb.position + move * _speed * Time.fixedDeltaTime);

            // Animate Player
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
    }
}