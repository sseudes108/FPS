using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private float MoveSpeed;
    private bool _canMove = true;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Vector3 _direction;

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
        _controller.Move(MoveSpeed * Time.deltaTime * _direction);
    }

    public void SetDirection(Vector3 direction){
        _direction = direction;
    }
}