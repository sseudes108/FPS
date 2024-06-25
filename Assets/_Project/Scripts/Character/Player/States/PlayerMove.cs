public class PlayerMove : MoveState{
    public bool _needReset = false;
    public override void Enter(){
        Player.ChangeAnimation(Player.WALK);
    }

    public override void LogicUpdate(){
        Player.HandleMovement();
        Player.HandleJump();
        HandleRun();
        if(Player.Input.Move.x == 0 && Player.Input.Move.y == 0){
            Player.ChangeState(Player.Idle);
        }
        if(!Player.IsGrounded()){
            Player.ChangeState(Player.Jump);
        }
    }

    public override void Exit(){
        Player.ChangeAnimation(Player.IDLE);
    }

    private void HandleRun(){
        if(Player.Input.Run){
            if(_needReset){ return; }
            Player.Movement.SetSpeed(Player.Movement.GetRunSpeed());
            Player.Anim.ChangeAnimation(Player.RUN);
            _needReset = true;
        }else{
            if(!_needReset){ return; }
            Player.Movement.SetSpeed(Player.Movement.GetDefaultSpeed());
            Player.Anim.ChangeAnimation(Player.WALK);
            _needReset = false;
        }
    }

    public override string ToString(){
        return "Move";
    }
}