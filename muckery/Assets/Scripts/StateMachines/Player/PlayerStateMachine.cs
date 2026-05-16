using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; set; }// [field: SerializeField] exposes the propetries of this getter to the unity inspector.
    [field: SerializeField] public CharacterController CharacterController { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public Targeter Targeter { get; set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; set; }
    [field: SerializeField] public Health Health { get; set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; set; }
    [field: SerializeField] public GameObject PlayerModel { get; set; }
    [field: SerializeField] public LedgeDetector LedgeDetector { get; set; }
    [field: SerializeField] public WeaponDamage Weapon { get; set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; set; }
    [field: SerializeField] public float RotationDamping { get; set; }
    [field: SerializeField] public float DodgeDuration { get; set; }
    [field: SerializeField] public float DodgeDistance { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
    [field: SerializeField] public Attack[] Attacks { get; set; }

    [Header("Prop Settings")]
    public Interactable CurrentInteractable { get; set; }
    public Interactable LastInteractable { get; set; }
    [field: SerializeField] public GameObject Knife { get; set; }
    [field: SerializeField] public GameObject PropaneTank { get; set; }
    [field: SerializeField] public GameObject Safe { get; set; }


    public float PreviousDodgeTime { get; set; } = Mathf.NegativeInfinity; //prevents the first dodge from having strange behaviour
    public Transform MainCameraTransform { get; set; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));
    }

    void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDeath;
    }

    void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDeath;
    }

    void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    void HandleDeath()
    {
        SwitchState(new PlayerDeadState(this));
    }
}