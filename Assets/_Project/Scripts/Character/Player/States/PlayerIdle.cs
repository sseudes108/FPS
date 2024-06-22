using UnityEngine;

public class PlayerIdle : IdleState{
    public override void Enter(){
        Debug.Log("Enter Player Idle State");
    }

    public override void LogicUpdate(){
        Player.HandleMovement();
        Player.HandleRotation();
    }

    public override void Exit(){
        
    }
}