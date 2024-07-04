using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour {
    public static Action<Vector3> OnCheckPoint;
    [SerializeField] private List<CheckPoint> _checkPoints;

    private void OnEnable() {
        Health.OnPlayerDied += Health_OnPlayerDied;
    }

    private void OnDisable() {
        Health.OnPlayerDied -= Health_OnPlayerDied;
    }

    private void Awake() {
        var cheks = FindObjectsByType(typeof(CheckPoint), FindObjectsSortMode.None);
        foreach (var item in cheks){
            _checkPoints.Add(item as CheckPoint);
        }
        CheckLastCheckPoint();
    }

    private void Health_OnPlayerDied(){
        StartCoroutine(PlayerDeadRoutine());
    }

    public IEnumerator PlayerDeadRoutine(){
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }

    private void CheckLastCheckPoint(){
        string playerPrefKey = $"{SceneManager.GetActiveScene().name} - Checkpoint:";
        if(PlayerPrefs.HasKey(playerPrefKey)){
            int key = PlayerPrefs.GetInt(playerPrefKey);
            foreach(var cp in _checkPoints){
                if(key == cp.CheckPointID){
                    OnCheckPoint?.Invoke(cp.transform.position);
                    break;
                }
            }
        }
    }
}