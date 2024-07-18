using UnityEngine;

public class KillPlayer : MonoBehaviour{
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            other.gameObject.GetComponent<Health>().TakeDamage(8000);
        }
    }
}
