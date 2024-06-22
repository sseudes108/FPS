public class IdleState : AbstractState{
    public override void Enter(){}

    public override void Exit(){}

    public override void LogicUpdate(){}
}

public class MoveState : AbstractState{
    public override void Enter(){}

    public override void Exit(){}

    public override void LogicUpdate(){}
}

public struct StatesData{
    public IdleState Idle;
    public MoveState Move;
}