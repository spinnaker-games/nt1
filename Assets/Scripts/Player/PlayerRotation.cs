using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] bool _snapTurning = false;
    [SerializeField] float _turnRotationSpeed = 10f;

    InputActions _inputActions;
    Vector2 _moveInputValue;

    void Awake()
    {
        _inputActions = new InputActions();
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

        if ( horizontal.sqrMagnitude <= 0.001f )
            return;

        Quaternion targetRotation = Quaternion.LookRotation( horizontal );

        transform.rotation = _snapTurning
            ? targetRotation
            : Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _turnRotationSpeed * Time.fixedDeltaTime
            );
    }
}