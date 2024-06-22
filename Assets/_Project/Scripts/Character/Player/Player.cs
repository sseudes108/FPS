using UnityEngine;

public class Player : Character {
    public PlayerInput PlayerInput {get; private set;}
    public FrameInput Input => PlayerInput.FrameInput;

    public StateMachine StateMachine {get; private set;}
    public Movement Movement {get; private set;}
    public PlayerCamera Camera {get; private set;}



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

    private void SetStates(){
        StateMachine.SetStates(new StatesData{
            Idle = new PlayerIdle(),
            Move = new PlayerMove(),
        });
    }

    public void HandleMovement(){
        Movement.SetDirection(new Vector2(Input.Move.x, Input.Move.y));
    }

    public void HandleRotation(){
        float mouseX = Input.Look.x * Camera.Sensitivity;
        float mouseY = Input.Look.y * Camera.Sensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseX, transform.rotation.eulerAngles.z);
        Camera.UpdateRotationCamera(mouseY);
    }
}