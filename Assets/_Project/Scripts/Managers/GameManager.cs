using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    public static GameManager Instance; // { get; private set;}
    public static Action OnGameStart;
    public static Action OnGameEnd;
    public static Action<bool> OnGamePaused;

    public UIManager UIManager; // { get; private set;}
    public SpawnManager SpawnManager; // { get; private set;}
    public AudioManager AudioManager; // { get; private set;}
    public VFXManager VFXManager; // { get; private set;}
    public PauseManager PauseManager; // { get; private set;}

    public Testing Testing;

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

        Testing = GetComponent<Testing>();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        OnGameStart?.Invoke();
    }
#endregion

#region Custom Methods
    private void SetInstance(){
        if(Instance != null){
            Debug.LogError("More Than One Instance of Game Manager");
            Destroy(Instance);
        }
        Instance = this;
    }
#endregion

#region Events
    private void SetManagers(){
        UIManager = GetComponentInChildren<UIManager>();
        SpawnManager = GetComponentInChildren<SpawnManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        VFXManager = GetComponentInChildren<VFXManager>();
        PauseManager = GetComponentInChildren<PauseManager>();
    }

    private void GameManager_OnGamePaused(bool paused){
        if(paused){
            Cursor.lockState = CursorLockMode.Confined;
        }else{
            Cursor.lockState = CursorLockMode.Locked;
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