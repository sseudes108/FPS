using UnityEngine;

public class Player : Character {
    public readonly int IDLE = Animator.StringToHash("Player_Idle");
    public readonly int WALK = Animator.StringToHash("Player_Walk");
    public readonly int RUN = Animator.StringToHash("Player_Run");

    public PlayerInput PlayerInput {get; private set;}
    public FrameInput Input => PlayerInput.FrameInput;

    public PlayerCamera Camera {get; private set;}
    private Transform _firstPersonCameraTransform;

    [SerializeField] private Transform checkGroundBox;
    [SerializeField] private Vector3 checkGroundBoxSize;

    public Movement Movement {get; private set;}

    public override void Awake() {
        base.Awake();
        Movement = GetComponent<Movement>();
        Camera = GetComponent<PlayerCamera>();
        PlayerInput = GetComponent<PlayerInput>();
    }
    
    private void Update() {
        HandleRotation();
        HandleShot();
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

    public override void HandleShot(){
        if(Input.Shoot){

            if(_firstPersonCameraTransform == null){
                _firstPersonCameraTransform = Camera.GetCameraTransform();
            }

            if(Physics.Raycast(_firstPersonCameraTransform.position, _firstPersonCameraTransform.forward, out RaycastHit hit)){
                Gun.GetFirePoint().LookAt(hit.point);
            }else{
                Gun.GetFirePoint().LookAt(_firstPersonCameraTransform.transform.position + (_firstPersonCameraTransform.forward * 30f));
            }

            Gun.Shoot();
        }
    }

    public bool IsGrounded(){
        Collider[] grounded = Physics.OverlapBox(checkGroundBox.position,checkGroundBoxSize, Quaternion.identity, LayerMask.GetMask("Ground"));

        if(grounded.Length > 0){
            GameManager.Instance.Testing.UpdateDebugGroundedLabel("true");
            return true;
        }else{
            GameManager.Instance.Testing.UpdateDebugGroundedLabel("false");
            return false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(checkGroundBox.position, checkGroundBoxSize);
    }
}