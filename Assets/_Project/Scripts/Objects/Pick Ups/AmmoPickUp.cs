using UnityEngine;

public class AmmoPickUp : MonoBehaviour {
    private readonly int ammoAmmount = 15;
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            other.GetComponent<PlayerGun>().PickUpAmmo(ammoAmmount);
            Destroy(gameObject);
        }
    }
}