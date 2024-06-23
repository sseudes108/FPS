using UnityEngine;

public class Player : Character {
    public PlayerInput PlayerInput {get; private set;}
    public FrameInput Input => PlayerInput.FrameInput;

    public StateMachine StateMachine {get; private set;}
    public Movement Movement {get; private set;}
    public PlayerCamera Camera {get; private set;}

    public IdleState Idle => StateMachine.IdleState;
    public JumpState Jump => StateMachine.JumpState;
    public MoveState Move => StateMachine.MoveState;

    private void Awake() {
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
    }

    public void ChangeState(AbstractState newState){
        StateMachine.ChangeState(newState);
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
        Movement.SetDirection(direction);
    }

    public void HandleRotation(){
        Vector2 lookInput = Input.Look;
        float mouseX = lookInput.x * Camera.Sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * Camera.Sensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        Camera.CameraRotation(mouseY);
    }
}