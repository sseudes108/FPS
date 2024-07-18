using UnityEngine;

public class HealthPickUp : MonoBehaviour {
    private readonly int healAmount = 15;
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            other.GetComponent<Health>().HealDamage(healAmount);
            Destroy(gameObject);
        }
    }
}