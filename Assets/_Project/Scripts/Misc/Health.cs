using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Health : MonoBehaviour{
    [field:SerializeField] public HealthManagerSO HealthManager { get; private set; }

    [SerializeField] private int _maxHealth = 100;
    public int CurrentHealth { get; private set; }
    private Character _character;

    private CinemachineImpulseSource _cinemachineImpulse;
    public bool IsDead = false;

    private void Awake() {
        _character = GetComponent<Character>();
        _cinemachineImpulse = GetComponent<CinemachineImpulseSource>();
    }

    private void Start() {
        IsDead = false;
        CurrentHealth = _maxHealth;
        Wait();
    }

    public void Wait(){
        StartCoroutine(WaitRoutine());
    }

    public IEnumerator WaitRoutine(){
        yield return new WaitForSeconds(0.5f);
        if(_character is Player){
            HealthManager.HealthChange(CurrentHealth);
        }
    }

    public void HealDamage(int value){
        CurrentHealth += value;
        if(CurrentHealth > _maxHealth){
            CurrentHealth = _maxHealth;
        }
        HealthManager.HealthChange(CurrentHealth);
    }

    public void TakeDamage(int value){
        if(IsDead) { return; }
        CurrentHealth -= value;

        if(_character is Player){
            HealthManager.HealthChange(CurrentHealth);
            HealthManager.PlayerDamaged();
            _cinemachineImpulse.GenerateImpulseWithVelocity(new Vector3(0.3f, 0.1f, 0.3f));
        }

        if( CurrentHealth <= 0 ){
            if(_character is Player){
                HealthManager.PlayerDied();
                IsDead = true;
            }else{
                Die();
            }
        }
    }

    public void Die(){
        Destroy(gameObject);
    }
}
