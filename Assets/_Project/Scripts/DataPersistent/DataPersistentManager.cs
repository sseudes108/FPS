using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DataPersistentManager : MonoBehaviour{
    public string FileName;
    public GameData GameData;
    public List<IDataPersistencer> dataPersistentObjects = new();
    private FileDataHandler DataHandler;

    private void Awake() {
        DataHandler = new(Application.persistentDataPath, FileName);
        dataPersistentObjects.Clear();
        dataPersistentObjects = FindAllDataPersistenceObjects();
        // LoadGame();
    }

    // private void Start() {
    //     LoadGame();
    // }

    public void NewGame(){
        GameData = new GameData();
    }

    public void LoadGame(){
        GameData = DataHandler.Load();

        if(GameData == null){
            Debug.Log("No saved data found. Initializing default values");
            NewGame();
        }

        foreach(var obj in dataPersistentObjects){
            obj.LoadData(GameData);
        }
    }

    public void SaveGame(){
        // Debug.Log("SaveGame Called");
        foreach(var obj in dataPersistentObjects){
            obj.SaveData(ref GameData);
        }
        
        DataHandler.Save(GameData);
    }

    private List<IDataPersistencer> FindAllDataPersistenceObjects(){
        MonoBehaviour[] allObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var updatedList = allObjects.OfType<IDataPersistencer>().ToList();
        return updatedList;
    }
}