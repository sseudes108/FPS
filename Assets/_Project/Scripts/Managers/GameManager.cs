using UnityEngine;

public class GameManager : MonoBehaviour{
    public static GameManager Instance { get; private set;}
    [SerializeField] public UIManager UIManager;
    [SerializeField] public SpawnManager RespawnManager;
    [SerializeField] public AudioManager AudioManager;
    
    private void Awake() {
        SetInstance();
        SetManagers();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetInstance(){
        if(Instance != null){
            Debug.LogError("More Than One Instance of Game Manager");
            Destroy(Instance);
        }
        Instance = this;
    }

    private void SetManagers(){
        UIManager = GetComponentInChildren<UIManager>();
        RespawnManager = GetComponentInChildren<SpawnManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
    }    
}