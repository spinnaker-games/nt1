using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] float _dashDistance = 5f;
    [SerializeField] float _dashDuration = 0.2f;
    [SerializeField] float _dashCooldown = 1f;

    Rigidbody _rb;
    InputActions _inputActions;

    Vector2 _latestMoveInput;
    bool _dashPressed;
    bool _canDash = true;
    bool _isDashing = false;

    float _dashTime;
    float _dashCooldownTimer;
    Vector3 _dashDirection;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _inputActions = new InputActions();
    }

    void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Dash.performed += OnDashPerformed;
        _inputActions.Player.Move.performed += OnMovePerformed;
        _inputActions.Player.Move.canceled += OnMovePerformed;
    }

    void OnDisable()
    {
        _inputActions.Player.Dash.performed -= OnDashPerformed;
        _inputActions.Player.Move.performed -= OnMovePerformed;
        _inputActions.Player.Move.canceled -= OnMovePerformed;
        _inputActions.Player.Disable();
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    {
        _latestMoveInput = context.ReadValue<Vector2>();
    }

    void OnDashPerformed(InputAction.CallbackContext context)
    {
        _dashPressed = true;
    }

    void Update()
    {
        // Handle cooldown
        if (!_canDash)
        {
            _dashCooldownTimer -= Time.deltaTime;
            if (_dashCooldownTimer <= 0f)
                _canDash = true;
        }

        // Start dash if pressed
        if (_dashPressed && _canDash && !_isDashing)
        {
            StartDash();
        }

        // Perform dash
        if (_isDashing)
        {
            _dashTime += Time.deltaTime;
            float dashSpeed = _dashDistance / _dashDuration;

            // Only apply horizontal movement
            Vector3 horizontalMove = new Vector3(_dashDirection.x, 0f, _dashDirection.z) * dashSpeed * Time.deltaTime;

            // Move Rigidbody manually; Y velocity unaffected
            _rb.MovePosition(_rb.position + horizontalMove);

            if (_dashTime >= _dashDuration)
            {
                _isDashing = false;
            }
        }

        _dashPressed = false;
    }

    void StartDash()
    {
        // Build direction relative to player orientation
        Vector3 inputDir =
            transform.right * _latestMoveInput.x +
            transform.forward * _latestMoveInput.y;

        // Remove vertical influence just in case the player is tilted
        inputDir.y = 0f;

        _dashDirection = inputDir.sqrMagnitude > 0.01f
            ? inputDir.normalized
            : transform.forward;

        _isDashing = true;
        _dashTime = 0f;
        _canDash = false;
        _dashCooldownTimer = _dashCooldown;
    }

    public bool IsDashing()
    {
        return _isDashing;
    }
}