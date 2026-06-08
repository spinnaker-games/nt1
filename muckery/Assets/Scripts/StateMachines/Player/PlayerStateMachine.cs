using Microsoft.Unity.VisualStudio.Editor;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public Targeter Targeter { get; set; }
    [field: SerializeField] public GameObject PauseMenu { get; set; }

    [field: Header("INPUT")]
    [field: Space]
    [field: SerializeField] public InputReader InputReader { get; set; }
    [field: SerializeField] public CharacterController CharacterController { get; set; }

    [field: Header("VISUALS")]
    [field: Space]
    [field: SerializeField] public GameObject PlayerModel { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public GameObject SlimeTrail { get; set; }


    [field: Header("HEALTH")]
    [field: Space]
    [field: SerializeField] public Health Health { get; set; }
    [field: SerializeField] public string GameOverSceneName { get; set; }
    [field: SerializeField] public float PostDeathDelay { get; set; }

    [field: Header("MOVEMENT")]
    [field: Space]
    [field: SerializeField] public float FreeLookMovementSpeed { get; set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; set; }
    [field: SerializeField] public float RotationDamping { get; set; }
    [field: SerializeField] public float DodgeDuration { get; set; }
    [field: SerializeField] public float DodgeDistance { get; set; }
    [field: SerializeField] public LedgeDetector LedgeDetector { get; set; }

    [field: Header("PHYSICS")]
    [field: Space]
    [field: SerializeField] public ForceReceiver ForceReceiver { get; set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; set; }

    [field: Header("AUDIO")]
    [field: Space]
    [field: SerializeField] public AudioSource MoveSFX { get; set; }
    [field: SerializeField] public AudioSource MorphSFX { get; set; }
    [field: SerializeField] public AudioSource DeathSFX { get; set; }

    [field: Header("MORPHABLES")]
    [field: Space]
    [field: SerializeField] public VisualEffect MorphVFX { get; set; }
    [field: SerializeField] public float MorphDuration { get; set; }
    [field: Space]
    [field: SerializeField] public GameObject Knife { get; set; }
    [field: SerializeField] public int KnifeDamageAmount { get; set; }
    [field: SerializeField] public int KnifeKnockback { get; set; }
    [field: SerializeField] public AudioSource KnifeAttackSXF { get; set; }

    [field: Space]
    [field: SerializeField] public GameObject Spring { get; set; }
    [field: SerializeField] public AudioSource JumpSFX { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }

    [field: Space]
    [field: SerializeField] public GameObject Barrel { get; set; }

    [field: Space]
    [field: SerializeField] public GameObject MetalDetector { get; set; }
    
    [field: Space]
    [field: SerializeField] public GameObject SniperRifle { get; set; }
    [field: SerializeField] public GameObject SniperRifleScope { get; set; }
    [field: SerializeField] public float SniperFOV { get; set; } = 20;

    [field: Header("CAMERA")]
    [field: Space]
    [field: SerializeField] public CinemachineCamera FreeLookVC { get; set; }
    

    public bool IsDisguised { get; set; }
    public bool IsMoving { get; set; }
    public bool IsMorphed { get; set; }
    public Morphable CurrentMorphable { get; set; }
    public Morphable LastMorphable { get; set; }
    public float PreviousDodgeTime { get; set; } = Mathf.NegativeInfinity; //prevents the first dodge from having strange behaviour
    public Transform MainCameraTransform { get; set; }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false; //TODO: Find better way to handle mouse visibility

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

    public void PlayKnifeAttackSFX()
    {
        KnifeAttackSXF.Play();
    }
}