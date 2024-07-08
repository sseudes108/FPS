using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    public static GameManager Instance { get; private set;}
    //Events
    public static Action OnGameStart;
    public static Action OnGameEnd;
    public static Action<GameData, bool> OnGamePaused;

    //Global variables
    // public Vector2 RotationInput {get; private set;}

    //Managers
    public UIManager UIManager { get; private set;}
    public SpawnManager SpawnManager { get; private set;}
    public AudioManager AudioManager { get; private set;}
    public PauseManager PauseManager { get; private set;}
    public ObjectPoolManager Pooling { get; private set;}
    public DataPersistentManager DataManager { get; private set;}
    public VisualManager Visual { get; private set;}

#region UnityMethods
    private void OnEnable() {
        OnGamePaused += GameManager_OnGamePaused;
        OnGameEnd += GameManager_OnGameEnd;
    }

    private void OnDisable() {
        OnGamePaused -= GameManager_OnGamePaused;
        OnGameEnd -= GameManager_OnGameEnd;
    }
        
    private void Awake() {
        SetInstance();
        SetManagers();
    }

    private void Start() {
        if(SceneManager.GetActiveScene().name != "TitleScreen"){
            Cursor.lockState = CursorLockMode.Locked;
            StartGame();
        }
    }

    // public void UpdateRotationInput(float mouseX, float mouseY){
    //     RotationInput = new Vector2(mouseX, mouseY);
    // }

#endregion

#region Custom Methods
    private void SetInstance(){
        if(Instance != null){
            Debug.LogError("More Than One Instance of Game Manager");
            Destroy(Instance);
        }
        Instance = this;
    }

    private void SetManagers(){
        UIManager = GetComponentInChildren<UIManager>();
        SpawnManager = GetComponentInChildren<SpawnManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        PauseManager = GetComponentInChildren<PauseManager>();
        Pooling = GetComponentInChildren<ObjectPoolManager>();
        DataManager = GetComponentInChildren<DataPersistentManager>();
        Visual = GetComponentInChildren<VisualManager>();
    }

    private void StartGame(){
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine(){
        DataManager.LoadGame();
        yield return new WaitForSeconds(1f);

        OnGameStart?.Invoke();
        yield return null;
    }

#endregion

#region Events

    private void GameManager_OnGamePaused(GameData data, bool paused){
        if(paused){
            Cursor.lockState = CursorLockMode.Confined;
        }else{
            Cursor.lockState = CursorLockMode.Locked;
            DataManager.SaveGame();
        }
    }

    private void GameManager_OnGameEnd(){
        StartCoroutine(ReloadGameRoutine());
    }

    private IEnumerator ReloadGameRoutine(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }
    
#endregion

}