using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private float _moveSpeed;
    private float _defaultMoveSpeed;
    private float _runSpeed;
    private bool _canMove = true;

    [SerializeField] private float _gravityModifier;
    [SerializeField] private int _jumpForce;
    private bool _jump = false;

    private CharacterController _controller;
    private Rigidbody _rigidbody;
    private Vector3 _direction;

    private void Awake() {
        _controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start(){
        _defaultMoveSpeed = _moveSpeed;
        _runSpeed = _defaultMoveSpeed + (_defaultMoveSpeed * 0.9f);
    }

    private void FixedUpdate() {
        //The NPCS and the Player use the Character Controller the bullet use Rigidbody
        if(_controller != null){
            if(_canMove){
                MoveCharacter();
            }

            if(_jump){
                ApplyJumpForce();
            }
        }else if (_rigidbody != null){
            MoveRigidbody();
        }
    }

    public void Jump(bool jump){_jump = jump; }

    private void ApplyJumpForce(){
        _direction.y += _jumpForce;
        _controller.Move(_direction * Time.deltaTime);
        _jump = false;
    }
    
    public void AllowMovement(bool canMove){ _canMove = canMove; }

    public float GetDefaultSpeed(){ return _defaultMoveSpeed; }
    public float GetRunSpeed(){ return _runSpeed; }
    public void SetSpeed(float speed){ _moveSpeed = speed;}

    private void MoveCharacter(){
        _direction.y = Physics.gravity.y * _gravityModifier;
        _controller.Move(_moveSpeed * Time.deltaTime * _direction);
    }

    public void SetCharacterDirection(Vector3 direction){ _direction = direction; }

    private void MoveRigidbody() {
        Vector3 movement = _moveSpeed * Time.deltaTime * _direction;
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }

    public void SetRigidbodyDirection(Vector3 direction) { _direction = direction.normalized; }
}