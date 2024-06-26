using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character {
    public readonly int IDLE = Animator.StringToHash("Enemy_Idle");
    public readonly int RUN = Animator.StringToHash("Enemy_Run");
    public readonly int SHOOT = Animator.StringToHash("Enemy_Shoot");

    public NavMeshAgent NavmeshAgent {get; private set;}
    public Vector3 InitialPosition {get; private set;}

    public EnemyPatrol PatrolState;
    public Player Target; //{get; private set;}


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

    public override void HandleShot(){
        Gun.GetFirePoint().LookAt(Target.transform.position);
        Gun.Shoot();
    }

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
                Debug.Log($"Target {Target}");
                Debug.Log($"target[0].GetComponent<Player>(); {target[0].GetComponent<Player>().name}");
            }
            return true;
        }else{
            return false;
        }
    }
}