using UnityEngine;

public class StateMachine : MonoBehaviour {
    private Character _character;
    public AbstractState CurrentState {get; private set;}

    public IdleState IdleState;
    public MoveState MoveState;
    public JumpState JumpState;

    private void Awake() { _character = GetComponent<Character>(); }

    private void Update() { 
        CurrentState.LogicUpdate(); 
    }

    public void ChangeState(AbstractState newState){
        CurrentState?.Exit();
        CurrentState = newState;
        if(CurrentState.Character == null){
            CurrentState.SetCharacter(_character);
        }
        CurrentState.Enter();
    }

    public void SetStates(StatesData states){
        IdleState = states.Idle;
        MoveState = states.Move;
        JumpState = states.Jump;
    }
}