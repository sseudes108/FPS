using UnityEngine;

public class Player : Character {
    private Gun _gun;

    public PlayerInput PlayerInput {get; private set;}
    public FrameInput Input => PlayerInput.FrameInput;

    public StateMachine StateMachine {get; private set;}
    public Movement Movement {get; private set;}
    public PlayerCamera Camera {get; private set;}

    public IdleState Idle => StateMachine.IdleState;
    public JumpState Jump => StateMachine.JumpState;
    public MoveState Move => StateMachine.MoveState;

    public Transform checkGroundBox;
    public Vector3 checkGroundBoxSize;

    public AnimationController Anim;
    public readonly int IDLE = Animator.StringToHash("Player_Idle");
    public readonly int WALK = Animator.StringToHash("Player_Walk");
    public readonly int RUN = Animator.StringToHash("Player_Run");
    
    private void Awake() {
        _gun = GetComponent<Gun>();
        Anim = GetComponentInChildren<AnimationController>();
        Movement = GetComponent<Movement>();
        Camera = GetComponent<PlayerCamera>();
        PlayerInput = GetComponent<PlayerInput>();
        StateMachine = GetComponent<StateMachine>();
        SetStates();
    }
    
    private void Start() {
        StateMachine.ChangeState(StateMachine.IdleState);
    }

    private void Update() {
        HandleRotation();
        HandleShot();
    }

    public void ChangeAnimation(int newAnimation){
        Anim.ChangeAnimation(newAnimation);
    }

    public void ChangeState(AbstractState newState){
        StateMachine.ChangeState(newState);
        GameManager.Instance.Testing.UpdatePlayerState(newState.ToString());
        GameManager.Instance.Testing.UpdatePlayerGrounded(IsGrounded().ToString());
    }

    private void SetStates(){
        StateMachine.SetStates(new StatesData{
            Idle = new PlayerIdle(),
            Move = new PlayerMove(),
            Jump = new PlayerJump(),
        });
    }

    public void HandleMovement(){
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

    public void HandleJump(){
        if(Input.Jump){
            Movement.Jump(true);
        }
    }

    public void HandleShot(){
        if(Input.Shoot){
            _gun.Shoot();
        }
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