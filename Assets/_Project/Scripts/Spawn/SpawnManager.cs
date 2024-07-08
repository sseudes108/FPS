using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour{
    public static Action<Vector3> OnCheckPoint;

    private void OnEnable() {
        Health.OnPlayerDied += Health_OnPlayerDied;
    }

    private void OnDisable() {
        Health.OnPlayerDied -= Health_OnPlayerDied;
    }

    private void Health_OnPlayerDied(){
        StartCoroutine(PlayerDeadRoutine());
    }

    public IEnumerator PlayerDeadRoutine(){
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }
}