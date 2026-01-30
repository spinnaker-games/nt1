using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerHover : MonoBehaviour
{
    [Header("Hover Settings")]
    [SerializeField] float _hoverGravityScale = 0.3f;
    [SerializeField] float _hoverDelay = 0.2f;

    [Header("Ground Check")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius = 0.2f;
    [SerializeField] LayerMask _groundLayer;

    [Header("Visuals")]
    [SerializeField] GameObject _hoverVisualObject;

    Rigidbody _rb;
    InputActions _inputActions;

    bool _hoverHeld;
    bool _jumpHeld;
    bool _isGrounded;
    bool _isHovering;

    float _timeSinceLeftGround;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _inputActions = new InputActions();

        if (_hoverVisualObject != null)
            _hoverVisualObject.SetActive(false);
    }

    void OnEnable()
    {
        _inputActions.Player.Enable();

        _inputActions.Player.Hover.performed += OnHoverPressed;
        _inputActions.Player.Hover.canceled += OnHoverReleased;

        _inputActions.Player.Jump.performed += OnJumpPressed;
        _inputActions.Player.Jump.canceled += OnJumpReleased;
    }

    void OnDisable()
    {
        _inputActions.Player.Hover.performed -= OnHoverPressed;
        _inputActions.Player.Hover.canceled -= OnHoverReleased;

        _inputActions.Player.Jump.performed -= OnJumpPressed;
        _inputActions.Player.Jump.canceled -= OnJumpReleased;

        _inputActions.Player.Disable();
    }

    void OnHoverPressed(InputAction.CallbackContext ctx)
    {
        _hoverHeld = true;
    }

    void OnHoverReleased(InputAction.CallbackContext ctx)
    {
        _hoverHeld = false;
    }

    void OnJumpPressed(InputAction.CallbackContext ctx)
    {
        _jumpHeld = true;
    }

    void OnJumpReleased(InputAction.CallbackContext ctx)
    {
        _jumpHeld = false;
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(
            _groundCheck.position,
            _groundCheckRadius,
            _groundLayer
        );

        if (_isGrounded)
        {
            _timeSinceLeftGround = 0f;
            SetHovering(false);
            return;
        }

        _timeSinceLeftGround += Time.deltaTime;

        bool shouldHover =
            _hoverHeld &&
            !_jumpHeld &&
            _timeSinceLeftGround >= _hoverDelay;

        SetHovering(shouldHover);
    }

    void FixedUpdate()
    {
        if (_isHovering)
        {
            Vector3 antiGravity =
                -Physics.gravity * (1f - _hoverGravityScale);

            _rb.AddForce(antiGravity, ForceMode.Acceleration);
        }
    }

    void SetHovering(bool hovering)
    {
        if (_isHovering == hovering)
            return;

        _isHovering = hovering;

        if (_hoverVisualObject != null)
            _hoverVisualObject.SetActive(_isHovering);
    }

    public bool IsHovering()
    {
        return _isHovering;
    }
}