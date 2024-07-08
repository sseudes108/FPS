using UnityEngine;

public class CheckPoint : MonoBehaviour, IDataPersistencer{
    [SerializeField] private int _checkPointID;
    public int CheckPointID => _checkPointID;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            Debug.Log($"Update position {transform.position}");
            SaveData(ref GameManager.Instance.DataManager.GameData);
            GameManager.Instance.DataManager.SaveGame();
        }
    }

    public void LoadData(GameData data){}

    public void SaveData(ref GameData data){
        data.RespawnPosition = transform.position;
    }
}
