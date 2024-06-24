public class PlayerIdle : IdleState{
    public override void LogicUpdate(){
        Player.HandleJump();
        if(Player.Input.Move.x != 0 || Player.Input.Move.y != 0){
            Player.ChangeState(Player.Move);
        }
        if(!Player.IsGrounded()){
            Player.ChangeState(Player.Jump);
        }
    }
}