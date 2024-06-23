using UnityEngine;

public class StateMachine : MonoBehaviour {
    public AbstractState CurrentState {get; private set;}

    public IdleState IdleState;
    public MoveState MoveState;
    public JumpState JumpState;

    public void ChangeState(AbstractState newState){
        CurrentState?.Exit();
        CurrentState = newState;
        if(CurrentState.Character == null){
            CurrentState.SetCharacter(GetComponent<Character>());
        }
        CurrentState.Enter();
    }

    private void Update() {
        CurrentState.LogicUpdate();
    }

    public void SetStates(StatesData states){
        IdleState = states.Idle;
        MoveState = states.Move;
        JumpState = states.Jump;
    }
}