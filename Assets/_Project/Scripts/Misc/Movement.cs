using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private float _gravityModifier;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private int _jumpForce;
    private bool _canMove = true;
    private bool _jump = false;
    private CharacterController _controller;
    private Vector3 _direction;
    
    private void Awake() {
        _controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate() {
        if(_canMove){
            Move();
        }

        if(_jump){
            ApplyJumpForce();
        }
    }

    public void Jump(bool jump){
        _jump = jump;
    }

    private void ApplyJumpForce(){
        _direction.y = _jumpForce;
        _controller.Move(_direction * Time.deltaTime);
        _jump = false;
    }
    
    public void AllowMovement(bool canMove){
        _canMove = canMove;
    }

    private void Move(){
        _controller.Move(_moveSpeed * Time.deltaTime * _direction);
    }

    public void ApplyExtraGravity(){
        _direction.y += Physics.gravity.y * _gravityModifier;
    }

    public void ResetGravityForce(){
        _direction.y = Physics.gravity.y * _gravityModifier;
    }

    public void SetDirection(Vector3 direction){
        _direction = direction;
    }
}