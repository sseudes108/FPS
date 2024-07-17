using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Turret : Enemy{
    public TurretGun TurretGun;
    public int RotateSpeed;
    public TurretAttack Attack { get; private set; }
    private bool _isAttacking = false;
    private bool _isIdle = false;

    public Transform TurretWeapon { get; private set; }
    private bool _checkPlayer;
    
    public override void Awake() {
        base.Awake();
        _checkPlayer = false;
        TurretGun = GetComponent<TurretGun>();
        TurretWeapon = transform.Find("turret_exclusive/turretWeapon");
    }

    public override void Start() {
        base.Start();
        StartCoroutine(WaitRoutine());
    }

    public override void SetStates(){
        Attack = new();
        StateMachine.SetStates(new StatesData{
            Idle = new TurretIdle(),
        });
    }

    public void Update() {
        if(!_checkPlayer) { return; }
        PlayerDetection(); 
    }

    public IEnumerator WaitRoutine(){
        yield return new WaitForSeconds(2f);
        _checkPlayer = true;
        yield return null;
    }

    private void PlayerDetection(){
        if(PlayerDetected()){
            if(_isAttacking){
                return; 
            }
            InitAttack();
            return;
        }

        if(_isIdle) { return; }
        InitIdle();
    }

    private void InitAttack(){
        ChangeState(Attack);
        _isAttacking = true;
        _isIdle = false;
    }

    private void InitIdle(){
        ChangeState(Idle);
        _isIdle = true;
        _isAttacking = false;
    }

}