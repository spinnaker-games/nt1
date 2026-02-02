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
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight   = Camera.main.transform.right;

        // Flatten the camera vectors
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Camera-relative move direction
        Vector3 moveDir = camForward * _moveInputValue.y +
                        camRight * _moveInputValue.x;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            Vector3 direction = moveDir.normalized;

            // Move
            _rb.MovePosition(_rb.position + direction * _speed * Time.fixedDeltaTime);

            // Rotate instantly to match move direction
            _rb.rotation = Quaternion.LookRotation(direction);

            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
    }
}