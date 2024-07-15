using UnityEngine;

public class Victory : MonoBehaviour {
    [SerializeField] private GameManagerSO _gameManager;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            _gameManager.GameFinished();
        }
    }
}