using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour, IDataPersistencer{
    [SerializeField] private int _checkPointID;
    public int CheckPointID => _checkPointID;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            // PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name} - Checkpoint:", _checkPointID);
            SaveData(GameManager.Instance.DataManager.GameData);
            GameManager.Instance.DataManager.SaveGame();
        }
    }


    public void LoadData(GameData data){}

    public void SaveData(GameData data){
        data.RespawnPosition = transform.position;
    }
}
