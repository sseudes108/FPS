using System;
using System.Collections;
using UnityEngine;

public class PlayerMove : MoveState{
    public static Action OnStep;
    public bool _needReset = false;
    private IEnumerator _stepRoutine;

    public override void Enter(){
        Player.ChangeAnimation(Player.WALK);
        _stepRoutine = HandleSteps();
        Player.StartCoroutine(_stepRoutine);
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
        Player.StopCoroutine(_stepRoutine);
        _stepRoutine = null;
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

    private IEnumerator HandleSteps(){
        do{
            float _stepTime;

            if(Player.Anim.CurrentAnimation == Player.RUN){
                _stepTime = 0.3f;
            }else{
                _stepTime = 0.5f;
            }

            yield return new WaitForSeconds(_stepTime);
            OnStep?.Invoke();
            yield return null;
        }while(_stepRoutine != null);
    }
}