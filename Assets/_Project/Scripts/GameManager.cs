using UnityEngine;

public class GameManager : MonoBehaviour{
    public static GameManager Instance { get; private set;}
    //**Apagar**//
    public Testing Testing;

    private void Awake() {
        Testing = GetComponent<Testing>();
        SetInstance();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetInstance(){
        if(Instance!=null){
            Debug.LogError("More Than One Instance of Game Manager");
            Destroy(Instance);
        }
        Instance = this;
    }
    
}