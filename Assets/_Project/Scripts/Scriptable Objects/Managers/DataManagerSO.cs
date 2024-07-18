using UnityEngine;

[CreateAssetMenu(fileName = "DataManagerSO", menuName = "FPS/Managers/Data", order = 0)]
public class DataManagerSO : ScriptableObject {
    private readonly string _fileName = "data.json";
    private GameData _gameData;
    private FileDataHandler _dataHandler;

    private void OnEnable() {
        _dataHandler ??= new(Application.persistentDataPath, _fileName);

        LoadGame();
    }

    public void NewGame() { _gameData = new GameData(); }

    public void LoadGame(){
        _gameData = _dataHandler.Load();

        if(_gameData == null){
            Debug.Log("No saved data found. Initializing default values");
            NewGame();
        }
    }

    public void SaveGame() { _dataHandler.Save(_gameData); }

    public Vector3 LoadRespawnPoint() { return _gameData.RespawnPosition; }
    public void SaveRespawnPoint(Vector3 spawnPosition) { _gameData.RespawnPosition = spawnPosition; SaveGame(); }

    public int LoadCrosshair() { return _gameData.CrossHair; }
    public void SaveCrosshair(int crosshair) { _gameData.CrossHair = crosshair; SaveGame(); }

    public float LoadSensitivity() { return _gameData.Sensitivity; }
    public void SaveSensitivity(float sensitivity) { _gameData.Sensitivity = sensitivity; SaveGame(); }

    public float LoadMusicVolume() { return _gameData.MusicVolume; }
    public void SaveMusicVolume(float musicVolume) { _gameData.MusicVolume = musicVolume * 100; SaveGame(); }

    public float LoadEffectVolume() { return _gameData.EffectVolume; }
    public void SaveEffectVolume(float effectVolume) { _gameData.EffectVolume = effectVolume * 100; SaveGame(); }
}