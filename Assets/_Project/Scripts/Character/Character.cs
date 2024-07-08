using UnityEngine;

public abstract class Character : MonoBehaviour {
    public AnimationController Anim {get; private set;}

    public StateMachine StateMachine {get; private set;}
    public IdleState Idle => StateMachine.IdleState;
    public JumpState Jump => StateMachine.JumpState;
    public MoveState Move => StateMachine.MoveState;

    [HideInInspector] public Gun Gun;

    public virtual void Awake() {
        Anim = GetComponent<AnimationController>();
        StateMachine = GetComponent<StateMachine>();
        SetStates();
    }

    public virtual void Start() {
        StateMachine.ChangeState(StateMachine.IdleState);
    }
    
    public abstract void SetStates();

    public void ChangeAnimation(int newAnimation){
        Anim.ChangeAnimation(newAnimation);
    }

    public void ChangeState(AbstractState newState){
        StateMachine.ChangeState(newState);
    }

    public virtual void HandleMovement(){}
    public virtual void HandleJump(){}
    public virtual void HandleShot(){}
}