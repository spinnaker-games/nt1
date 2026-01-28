using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings (in units & seconds)")]
    [SerializeField] float _minJumpHeight = 1.5f;   // Short hop
    [SerializeField] float _maxJumpHeight = 3f;     // Full jump
    [SerializeField] float _timeToApex = 0.4f;      // Time to reach apex for full jump
    [SerializeField] float _maxJumpHoldTime = 0.15f;// Max time player can hold jump
    [SerializeField] float _coyoteTime = 0.1f;
    [SerializeField] float _jumpBufferTime = 0.1f;

    [Header("Ground Check")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundRadius = 0.3f;
    [SerializeField] LayerMask _groundLayer;

    [Header("Player Animation")]
    [SerializeField] Animator _animator;

    Rigidbody _rb;
    InputActions _inputActions;

    float _coyoteTimer;
    float _jumpBufferTimer;
    float _jumpHoldTimer;
    bool _isGrounded;
    bool _isJumping;

    float _gravity;
    float _v0Min;
    float _v0Max;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false; // We'll handle gravity manually

        _inputActions = new InputActions();

        // Precompute velocities and gravity
        _gravity = -2f * _maxJumpHeight / (_timeToApex * _timeToApex);
        _v0Max = 2f * _maxJumpHeight / _timeToApex;
        float minApexTime = _timeToApex * (_minJumpHeight / _maxJumpHeight);
        _v0Min = 2f * _minJumpHeight / minApexTime;
    }

    void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Jump.performed += OnJumpPressed;
        _inputActions.Player.Jump.canceled += OnJumpReleased;
    }

    void OnDisable()
    {
        _inputActions.Player.Jump.performed -= OnJumpPressed;
        _inputActions.Player.Jump.canceled -= OnJumpReleased;
        _inputActions.Player.Disable();
    }

    void Update()
    {
        // Ground & coyote time
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundRadius, _groundLayer);
        if (_isGrounded) _coyoteTimer = _coyoteTime;

        _coyoteTimer -= Time.deltaTime;
        _jumpBufferTimer -= Time.deltaTime;

        // Start jump if buffered and allowed
        if (_jumpBufferTimer > 0f && _coyoteTimer > 0f)
        {
            StartJump();
            _jumpBufferTimer = 0f;
        }

        if (_isJumping)
        {
            _jumpHoldTimer -= Time.deltaTime;
            if (_jumpHoldTimer <= 0f)
                _isJumping = false;
        }

        _animator.SetBool("IsJumping", !_isGrounded);

    }

    void FixedUpdate()
    {
        Vector3 velocity = _rb.linearVelocity;

        // Apply jump force if still holding
        if (_isJumping && velocity.y > 0f)
        {
            float t = 1f - (_jumpHoldTimer / _maxJumpHoldTime);
            float currentV0 = Mathf.Lerp(_v0Min, _v0Max, t);
            velocity.y = currentV0;
        }

        // Manual gravity
        velocity.y += _gravity * Time.fixedDeltaTime;

        _rb.linearVelocity = velocity;
    }

    void OnJumpPressed(InputAction.CallbackContext ctx)
    {
        _jumpBufferTimer = _jumpBufferTime;
    }

    void OnJumpReleased(InputAction.CallbackContext ctx)
    {
        _isJumping = false;
    }

    void StartJump()
    {
        Vector3 v = _rb.linearVelocity;
        v.y = _v0Min;  // Start with minimum jump velocity
        _rb.linearVelocity = v;

        _coyoteTimer = 0f;
        _isJumping = true;
        _jumpHoldTimer = _maxJumpHoldTime;
    }

    void OnDrawGizmosSelected()
    {
        if (_groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundRadius);
    }
}