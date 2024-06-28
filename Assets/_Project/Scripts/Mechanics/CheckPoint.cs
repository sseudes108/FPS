using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour{
    [SerializeField] private int _checkPointID;
    public int CheckPointID => _checkPointID;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name} - Checkpoint:", _checkPointID);
        }
    }
}
