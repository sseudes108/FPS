using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private float _moveSpeed;
    private float _defaultMoveSpeed;
    private float _runSpeed;
    private bool _canMove = true;

    private CharacterController _controller;
    private Rigidbody _rigidbody;
    private Vector3 _direction;

    private readonly float _gravityModifier = 0.1f;
    private readonly float _jumpForce = 0.5f;
    private bool _isJumping = false;
    private float _jumpTimeCounter;
    private readonly float _jumpTime = 0.2f;
    private readonly float _jumpForceMultiplier = 1f;
    private float _verticalVelocity = 0f;

    private void Awake() {
        _controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start(){
        _defaultMoveSpeed = _moveSpeed;
        _runSpeed = _defaultMoveSpeed + (_defaultMoveSpeed * 0.9f);
    }

    private void FixedUpdate() {
        //Rigidbody move the bullets
        if (_controller != null){
            if (_canMove){
                MoveCharacter();
            }
            if(_isJumping){
                ApplyJumpForce();
            }
        }else if (_rigidbody != null){
            MoveRigidbody();
        }
    }
   
    public void AllowMovement(bool canMove){ _canMove = canMove; }
    public float GetDefaultSpeed(){ return _defaultMoveSpeed; }
    public float GetRunSpeed(){ return _runSpeed; }
    public void SetSpeed(float speed){ _moveSpeed = speed;}

    public void Jump(){
        _isJumping = true;
        _jumpTimeCounter = _jumpTime;
    }

    private void ApplyJumpForce(){
        if (_isJumping){
            if (_jumpTimeCounter > 0){
                _direction.y = _jumpForce * _jumpForceMultiplier;
                _jumpTimeCounter -= Time.deltaTime;
            }else{
                _isJumping = false;
            }
        }

        _controller.Move(_direction * Time.deltaTime);
    }

    private void MoveCharacter(){
        if (_isJumping){
            _verticalVelocity = _jumpForce * _jumpForceMultiplier;
        }else{
            _verticalVelocity += Physics.gravity.y * _gravityModifier * Time.deltaTime;
        }

        _direction.y = _verticalVelocity;
        _controller.Move(_moveSpeed * Time.deltaTime * _direction);
    }

    public void SetCharacterDirection(Vector3 direction){ _direction = direction; }

    private void MoveRigidbody() {
        Vector3 movement = _moveSpeed * Time.deltaTime * _direction;
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }

    public void SetRigidbodyDirection(Vector3 direction) { _direction = direction.normalized; }
}