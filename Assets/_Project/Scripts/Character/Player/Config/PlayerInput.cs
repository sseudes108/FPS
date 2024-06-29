using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour{
    private InputSystem_Actions _inputActions;
    private InputAction _move,_aim,_look,_shoot,_jump,_run,_crouch,_previous,_next;
    public FrameInput FrameInput{get; private set;}

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
    }

    private void OnDisable() { _inputActions.Disable(); }
    private void Awake() { _inputActions = new(); }

    private void Update() {
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
        };
    }
}

[Serializable]
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
}
