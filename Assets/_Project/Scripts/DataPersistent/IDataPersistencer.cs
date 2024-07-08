
public interface IDataPersistencer {
    void LoadData(GameData data);

    void SaveData(ref GameData data);
    // void SaveData(ref GameData data);
}


// public interface IDataLoader{
//     public void Load(GameData gameData);
// }

// public interface IDataSaver{
//     public void Save();
// }