using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private int _moveSpeed;
    [SerializeField] private float _gravityModifier;
    private bool _canMove = true;
    private CharacterController _controller;
    private Vector3 _direction;

    private void Awake() {
        _controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate() {
        if(!_canMove) {
            return;
        }
        Move();
    }

    public void AllowMovement(bool canMove){
        _canMove = canMove;
    }

    private void Move(){
        _direction.y += Physics.gravity.y * _gravityModifier;
        _controller.Move(_moveSpeed * Time.deltaTime * _direction);
    }

    public void SetDirection(Vector3 direction){
        _direction = direction;
    }

    public bool IsGrounded(){
        return _controller.isGrounded;
    }

    public void ApplyExtraGravityForce(){
        _gravityModifier =+ 0.01f;
    }
    public void ResetGravityForce(){
        _gravityModifier = 0.3f;
    }
}