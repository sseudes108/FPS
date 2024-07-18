using System.Collections;
using UnityEngine;

public class PlayerMove : MoveState{
    public bool _needReset = false;
    private IEnumerator _stepRoutine;

    public override void Enter(){
        _needReset = false;
        Player.ChangeAnimation(Player.Animations.WALK);
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
        Player.ChangeAnimation(Player.Animations.IDLE);
        Player.StopCoroutine(_stepRoutine);
        _stepRoutine = null;
    }

    private void HandleRun(){
        if(Player.Input.Run){
            if(_needReset){ return; }
            Player.Movement.SetSpeed(Player.Movement.GetRunSpeed());
            Player.Anim.ChangeAnimation(Player.Animations.RUN);
            _needReset = true;
        }else{
            if(!_needReset){ return; }
            Player.Movement.SetSpeed(Player.Movement.GetDefaultSpeed());
            Player.Anim.ChangeAnimation(Player.Animations.WALK);
            _needReset = false;
        }
    }

    private IEnumerator HandleSteps(){
        do{
            float _stepTime;

            if(Player.Anim.CurrentAnimation == Player.Animations.RUN){
                _stepTime = 0.3f;
            }else{
                _stepTime = 0.5f;
            }

            yield return new WaitForSeconds(_stepTime);
            // OnStep?.Invoke();
            Player.AudioManager.PlayStepSound(Player);
            yield return null;
        }while(_stepRoutine != null);
    }
}