using UnityEngine;

public class CheckPoint : MonoBehaviour{
    private bool _checkPointUpdated;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(!_checkPointUpdated){
                other.GetComponent<Player>().SaveSpawnPosition(transform.position);
                _checkPointUpdated = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            _checkPointUpdated = false;
        }
    }
}
