using System.Collections;
using Unity.Cinemachine;
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
    private CinemachineImpulseSource _impulseSource;

    private PlayerGun _playerGun;

    private void OnEnable() {
        SpawnManager.OnCheckPoint += CheckPoint_UpdatePosition;
    }

    private void OnDisable() {
        SpawnManager.OnCheckPoint -= CheckPoint_UpdatePosition;
    }

    public override void Awake() {
        base.Awake();
        UpdateCurrentGun();
        Movement = GetComponent<Movement>();
        Camera = GetComponent<PlayerCamera>();
        PlayerInput = GetComponent<PlayerInput>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public override void Start() {
        base.Start();
    }
    
    private void Update() {
        HandleRotation();
        HandleShot();
    }

    private void UpdateCurrentGun(){
        _playerGun = GetComponent<PlayerGun>();
        Gun = _playerGun.GetActiveGun();
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
        if (Input.Shoot){
            if (_firstPersonCameraTransform == null){
                _firstPersonCameraTransform = Camera.GetCameraTransform();
            }

            int layerMask = ~LayerMask.GetMask("NoHitLayer"); // Cria uma máscara para todas as camadas exceto a "NoHitLayer"

            if (Physics.Raycast(_firstPersonCameraTransform.position, _firstPersonCameraTransform.forward, out RaycastHit hit, Mathf.Infinity, layerMask)){
                Gun.GetFirePoint().LookAt(hit.point);
            }else{
                Gun.GetFirePoint().LookAt(_firstPersonCameraTransform.position + (_firstPersonCameraTransform.forward * 30f));
            }

            Gun.Shoot();
            ShakeCamera();
        }
    }

    // private void HandleChangeGun(){

    // }

    private void CheckPoint_UpdatePosition(Vector3 lastCheckPointPosition){
        transform.parent.transform.position = lastCheckPointPosition;
        Movement.Controller.enabled = true; //The Character Controller Component prevent the change in position when enabled
    }

    private void ShakeCamera(){
        _impulseSource.GenerateImpulseWithForce(Gun.RecoilForce);
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