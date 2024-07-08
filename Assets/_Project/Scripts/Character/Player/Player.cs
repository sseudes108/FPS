using System;
using System.Collections;
using UnityEngine;


public class Player : Character {
    public static Action<Gun> OnWeaponPickUp;

    //Public Acess
    public readonly int IDLE = Animator.StringToHash("Player_Idle");
    public readonly int WALK = Animator.StringToHash("Player_Walk");
    public readonly int RUN = Animator.StringToHash("Player_Run");
    public FrameInput Input => PlayerInput.FrameInput;

    //Ground Check Settings
    [SerializeField] private Transform checkGroundBox;
    [SerializeField] private Vector3 checkGroundBoxSize;

    //Components
    public PlayerInput PlayerInput {get; private set;}
    public Movement Movement {get; private set;}
    public PlayerCamera Camera {get; private set;}
    private PlayerGun _playerGun;

    private bool _canInteract;
    private MonoBehaviour _interactItem;

#region UnityMethods
    private void OnEnable() {
        SpawnManager.OnCheckPoint += SpawnManager_UpdatePosition;
        GameManager.OnGameStart += GameManager_OnGameStart;
        GameManager.OnGamePaused += GameManager_OnGamePause;
        Gun.OnPlayerCloseForPickUp += Gun_OnPlayerCloseForPickUp;
        Gun.OnPlayerMoveOutRange += Gun_OnPlayerMoveOutRange;
        Health.OnPlayerDied += Health_OnPlayerDied;
    }
    private void OnDisable() {
        SpawnManager.OnCheckPoint -= SpawnManager_UpdatePosition;
        GameManager.OnGameStart -= GameManager_OnGameStart;
        GameManager.OnGamePaused -= GameManager_OnGamePause;
        Gun.OnPlayerCloseForPickUp -= Gun_OnPlayerCloseForPickUp;
        Gun.OnPlayerMoveOutRange -= Gun_OnPlayerMoveOutRange;
        Health.OnPlayerDied -= Health_OnPlayerDied;
    }

    private void Health_OnPlayerDied(){
        PlayerInput.AllowInputs(false);
    }

    private void Gun_OnPlayerCloseForPickUp(Gun gun){
        _canInteract = true;
        _interactItem = gun;
    }

    private void Gun_OnPlayerMoveOutRange(bool interact){
        _canInteract = interact;
        _interactItem = null;
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
        HandleGun();
        HandlePause();
        HandleInteraction();
    }

#endregion

#region Custom Methods

    private void HandleInteraction(){
        if(!_canInteract){return;}
        if(!Input.Interact){return;}
        OnWeaponPickUp?.Invoke(_interactItem as Gun);
    }

    public override void SetStates(){
        StateMachine.SetStates(new StatesData{
            Idle = new PlayerIdle(),
            Move = new PlayerMove(),
            Jump = new PlayerJump(),
        });
    }

    private void HandleGun(){
        _playerGun.HandleAim();
        _playerGun.HandleShoot();
        _playerGun.HandleSwitchGun();
        _playerGun.HandleGunReload();
    }

    private void HandlePause(){
        if(!UnityEngine.Input.GetKeyDown(KeyCode.Escape)){return;}
        if(GameManager.Instance.PauseManager.IsPaused){
            GameManager.OnGamePaused?.Invoke(GameManager.Instance.DataManager.GameData, false);
        }else{
            GameManager.OnGamePaused?.Invoke(GameManager.Instance.DataManager.GameData, true);
        }
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

        GameManager.Instance.UpdateRotationInput(mouseX, mouseY); //Update the rotation to can be used in gun by the Sway.cs
    }

    public override void HandleJump(){
        if(Input.Jump){
            Movement.Jump();
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
#endregion

#region Events
    public void GameManager_OnGameStart(){
        StartCoroutine(GameStartAdjustments());
    }
    
    private IEnumerator GameStartAdjustments(){
        PlayerInput.AllowInputs(false);
        yield return new WaitForSeconds(0.5f);
        PlayerInput.AllowInputs(true);
        Movement.Controller.enabled = true;
        yield return null;
    }

    private void GameManager_OnGamePause(GameData data, bool isPaused){
        if(isPaused){
            PlayerInput.AllowInputs(false);
        }else{
            PlayerInput.AllowInputs(true);
        }
    }

    private void SpawnManager_UpdatePosition(Vector3 lastCheckPointPosition){
        transform.position = lastCheckPointPosition;
        Movement.Controller.enabled = true; //The Character Controller Component prevent the change in position when enabled
    }
#endregion

}