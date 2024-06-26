using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character {
    public readonly int IDLE = Animator.StringToHash("Player_Idle");
    public readonly int WALK = Animator.StringToHash("Player_Walk");
    public readonly int RUN = Animator.StringToHash("Player_Run");

    public NavMeshAgent NavmeshAgent {get; private set;}
    public Vector3 InitialPosition {get; private set;}

    public EnemyPatrol PatrolState;
    public Player Target {get; private set;}


    //The _aggroRadius is the distance at which the enemy starts chasing the player.
    [SerializeField] private float _aggroRadius;
    //The _deAggroRadius is the distance at which the enemy stops chasing the player.
    [SerializeField] private float _deAggroRadius;
    public float DeAggroRadius => _deAggroRadius;

    public override void Start(){
        base.Start();
        InitialPosition = transform.position;
    }

    public override void Awake(){
        base.Awake();
        NavmeshAgent = GetComponent<NavMeshAgent>();
    }

    public override void HandleShot(){}

    public void HandlePlayerDetection(){
        if(PlayerDetected()){
            ChangeState(Move);
        }
    }

    public override void SetStates(){
        PatrolState = new();
        StateMachine.SetStates(new StatesData{
            Idle = new EnemyIdle(),
            Move = new EnemyMove(),
            Jump = null,
        });
    }

    public bool PlayerDetected(){
        Collider[] target = new Collider[1];
        var targetInRange = Physics.OverlapSphereNonAlloc(transform.position, _aggroRadius, target, LayerMask.GetMask("Player"));

        if(targetInRange > 0 && target[0] != null){
            if(Target == null){
                Target = target[0].GetComponent<Player>();
            }
            return true;
        }else{
            return false;
        }
    }
}