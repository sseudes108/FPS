using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour{
    public static GameController Instance { get; private set;}

    [field:SerializeField] public AudioManagerSO AudioManager{ get; private set;}
    [field:SerializeField] public GameManagerSO GameManager { get; private set;}
    [field:SerializeField] public  DataManagerSO DataManager { get; private set;}
    

    //Managers
    public UIManager UIManager;
    public PauseManager PauseManager;
    private IEnumerator _startRoutine;


#region UnityMethods
    private void OnEnable() {
        GameManager.OnGameOver.AddListener(GameManager_OnGameEnd);
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
    }

    private void OnDisable() {
        GameManager.OnGameOver.RemoveListener(GameManager_OnGameEnd);
        GameManager.OnGamePaused.RemoveListener(GameManager_OnGamePaused);
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
        PauseManager = GetComponentInChildren<PauseManager>();
    }

    private void StartGame(){
        if(_startRoutine == null){
            _startRoutine = StartGameRoutine();
            StartCoroutine(_startRoutine);
        }
    }

    private IEnumerator StartGameRoutine(){
        DataManager.LoadGame();
        yield return new WaitForSeconds(1f);

        GameManager.GameStart();
        AudioManager.PlayStartGameMusic();
        yield return null;
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene("TitleScreen");
    }

#endregion

#region Events

    private void GameManager_OnGamePaused(bool paused){
        if(paused){
            Cursor.lockState = CursorLockMode.Confined;
        }else{
            Cursor.lockState = CursorLockMode.Locked;
            DataManager.SaveGame();
        }
    }

    private void GameManager_OnGameEnd(){
        _startRoutine = null;
        StartCoroutine(ReloadGameRoutine());
    }

    private IEnumerator ReloadGameRoutine(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }
    
#endregion

}