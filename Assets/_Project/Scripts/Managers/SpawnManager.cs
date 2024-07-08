using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour, IDataPersistencer{
    public static Action<Vector3> OnCheckPoint;
    [SerializeField] private List<CheckPoint> _checkPoints;

    public Vector3 _lastCheckPointPosition;

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
        OnCheckPoint?.Invoke(_lastCheckPointPosition);
    }

    public void LoadData(GameData data){
        _lastCheckPointPosition = data.RespawnPosition;
    }

    public void SaveData(ref GameData data){ }
}