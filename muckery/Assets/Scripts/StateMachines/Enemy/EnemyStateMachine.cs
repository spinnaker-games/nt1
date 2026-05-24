using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public CharacterController CharacterController { get; set; }//TODO: use category headers
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; set; }
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] public Health Health { get; set; }
    [field: SerializeField] public Target Target { get; set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; set; }
    [field: SerializeField] public float FOV { get; set; }
    [field: SerializeField] public WeaponDamage Weapon { get; set; }
    [field: SerializeField] public int DamageAmount { get; set; }
    [field: SerializeField] public int AttackKnockback { get; set; }
    [field: SerializeField] public float MovementSpeed { get; set; }
    [field: SerializeField] public float ChaseSpeed { get; set; }
    [field: SerializeField] public float IdleDuration { get; set; }
    [field: SerializeField] public bool EndlessIdle { get; set; } //TODO: Find better name
    [field: SerializeField] public bool ChasePlayer { get; set; }
    [field: SerializeField] public float PlayerChasingRange { get; set; }
    [field: SerializeField] public float PlayerAttackRange { get; set; }
    [field: SerializeField] public Transform[] Waypoints { get; set; }
    [field: SerializeField] public float WaypointIdleTime { get; set; }
    [field: SerializeField] public bool IsTarget { get; set; } = false;
    [field: SerializeField] public Transform TargetEscape { get; set; }
    [field: SerializeField] public AudioSource DeathSFX { get; set; }
    [field: SerializeField] public AudioSource AlertSXF { get; set; }
    [field: SerializeField] public AudioSource FootstepSXF { get; set; }

    public bool IsDead { get; set; } //TODO: expose????
    public int CurrentWaypointIndex { get; set; }
    public int WaypointDirection { get; set; } = 1;
    public PlayerStateMachine PlayerStateMachine { get; private set; }
    public Health PlayerHealth { get; private set; }

    public GameObject Player { get; set; }

    void Start()
    {
        IsDead = false;
        Player = GameObject.FindGameObjectWithTag("Player"); //TODO: Evaluate if there is a better way to get player component
        PlayerStateMachine = Player.GetComponent<PlayerStateMachine>();
        PlayerHealth = Player.GetComponent<Health>();
        NavMeshAgent.updatePosition = false;//by default, we do not want the agent to move
        NavMeshAgent.updateRotation = false;
        SwitchState(new EnemyPatrolState(this));
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
        SwitchState(new EnemyDamageState(this));
    }

    void HandleDeath()
    {
        SwitchState(new EnemyDeadState(this));
    }

    
    public void PlayFootstepSFX()
    {
        FootstepSXF.Play();
    }

    void OnDrawGizmosSelected() //Will draw only when enemy is selected
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }
}