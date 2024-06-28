using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour {

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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }
}