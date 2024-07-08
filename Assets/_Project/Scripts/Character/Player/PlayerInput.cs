using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour{
    private InputSystem_Actions _inputActions;
    private InputAction _move,_aim,_look,_shoot,_jump,_run,_crouch,_previous,_next,_reload, _interact;
    public FrameInput FrameInput;
    private bool _allowInputs;

    private void OnEnable() {
        _inputActions.Enable();
        _move = _inputActions.Player.Move;
        _shoot = _inputActions.Player.Attack;
        _aim = _inputActions.Player.Aim;
        _look = _inputActions.Player.Look;
        _jump = _inputActions.Player.Jump;
        _crouch = _inputActions.Player.Crouch;
        _run = _inputActions.Player.Sprint;
        _previous = _inputActions.Player.Previous;
        _next = _inputActions.Player.Next;
        _reload = _inputActions.Player.Reload;
        _interact = _inputActions.Player.Interact;
    }

    private void OnDisable() { _inputActions.Disable(); }
    private void Awake() { _inputActions = new(); }
    private void Update() {
        if(!_allowInputs){return;}
        FrameInput = new FrameInput{
            Move = _move.ReadValue<Vector2>(),
            Aim = _aim.IsInProgress(),
            Look = _look.ReadValue<Vector2>(),
            Shoot = _shoot.IsPressed(),
            Jump = _jump.WasPressedThisFrame(),
            Crouch = _crouch.IsInProgress(),
            Run = _run.IsPressed(),
            Previous = _previous.WasPressedThisFrame(),
            Next = _next.WasPressedThisFrame(),
            Reload = _reload.WasPressedThisFrame(),
            Interact = _interact.WasPressedThisFrame(),
        };
    }

    public void AllowInputs(bool allow){
        _allowInputs = allow;
    }
}

[System.Serializable]
public struct FrameInput {
    public Vector2 Move;
    public Vector2 Look;
    public bool Shoot;
    public bool Aim;
    public bool Jump;
    public bool Crouch;
    public bool Run;
    public bool Previous;
    public bool Next;
    public bool Reload;
    public bool Interact;
}
