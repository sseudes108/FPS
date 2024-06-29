using UnityEngine;

public class Player : Character {
    public readonly int IDLE = Animator.StringToHash("Player_Idle");
    public readonly int WALK = Animator.StringToHash("Player_Walk");
    public readonly int RUN = Animator.StringToHash("Player_Run");

    public PlayerInput PlayerInput {get; private set;}
    public FrameInput Input => PlayerInput.FrameInput;


    [SerializeField] private Transform checkGroundBox;
    [SerializeField] private Vector3 checkGroundBoxSize;


    public Movement Movement {get; private set;}
    public PlayerCamera Camera {get; private set;}
    private PlayerGun _playerGun;


    private void OnEnable() {
        SpawnManager.OnCheckPoint += CheckPoint_UpdatePosition;
    }

    private void OnDisable() {
        SpawnManager.OnCheckPoint -= CheckPoint_UpdatePosition;
    }

    public override void Start(){
        base.Start();
        if(!Movement.Controller.enabled){
            Movement.Controller.enabled = true;
        }
    }

    public override void Awake() {
        base.Awake();
        _playerGun = GetComponent<PlayerGun>();
        Movement = GetComponent<Movement>();
        Camera = GetComponent<PlayerCamera>();
        PlayerInput = GetComponent<PlayerInput>();
    }
    
    private void Update() {
        HandleRotation();
        HandlePlayerGun();      
    }

    public override void SetStates(){
        StateMachine.SetStates(new StatesData{
            Idle = new PlayerIdle(),
            Move = new PlayerMove(),
            Jump = new PlayerJump(),
        });
    }

    public override void HandleMovement(){
        Vector3 forward = transform.forward * Input.Move.y;
        Vector3 right = transform.right * Input.Move.x;
        Vector3 direction = (forward + right).normalized;
        Movement.SetCharacterDirection(direction);
    }

    public void HandleRotation(){
        Vector2 lookInput = Input.Look;
        float mouseX = lookInput.x * Camera.Sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * Camera.Sensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        Camera.CameraRotation(mouseY);
    }

    public override void HandleJump(){
        if(Input.Jump){
            Movement.Jump();
        }
    }

    private void HandlePlayerGun(){
        _playerGun.HandleAim();
        _playerGun.HandleShoot();
        _playerGun.HandleSwitchGun();
    }

    private void CheckPoint_UpdatePosition(Vector3 lastCheckPointPosition){
        transform.parent.transform.position = lastCheckPointPosition;
        Movement.Controller.enabled = true; //The Character Controller Component prevent the change in position when enabled
    }

    public bool IsGrounded(){
        Collider[] grounded = Physics.OverlapBox(checkGroundBox.position,checkGroundBoxSize, Quaternion.identity, LayerMask.GetMask("Ground"));
        if(grounded.Length > 0){
            return true;
        }else{
            return false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(checkGroundBox.position, checkGroundBoxSize);
    }
}