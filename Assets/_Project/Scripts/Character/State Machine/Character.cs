using UnityEngine;

public abstract class Character : MonoBehaviour {
    public AnimationController Anim {get; private set;}

    public StateMachine StateMachine {get; private set;}
    public IdleState Idle => StateMachine.IdleState;
    public JumpState Jump => StateMachine.JumpState;
    public MoveState Move => StateMachine.MoveState;

    public Gun Gun {get; private set;}

    public virtual void Awake() {
        Gun = GetComponent<Gun>();
        Anim = GetComponent<AnimationController>();
        StateMachine = GetComponent<StateMachine>();
        SetStates();
    }

    private void Start() {
        StateMachine.ChangeState(StateMachine.IdleState);
    }
    
    public abstract void SetStates();

    public void ChangeAnimation(int newAnimation){
        Anim.ChangeAnimation(newAnimation);
    }

    public void ChangeState(AbstractState newState){
        StateMachine.ChangeState(newState);
        if(this is Player){
            GameManager.Instance.Testing.UpdateDebugStateLabel(newState.ToString());
        }
    }

    public abstract void HandleMovement();
    public abstract void HandleJump();
    public abstract void HandleShot();
}