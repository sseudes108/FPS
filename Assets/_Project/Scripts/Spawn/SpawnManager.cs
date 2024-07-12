using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour{
    [field:SerializeField] public HealthEventHandlerSO HealthManager { get; private set;}

    private void OnEnable() {
        HealthManager.OnPlayerDied.AddListener(HealthManager_OnPlayerDied);
    }

    private void OnDisable() {
        HealthManager.OnPlayerDied.RemoveListener(HealthManager_OnPlayerDied);
    }

    private void HealthManager_OnPlayerDied(){
        StartCoroutine(PlayerDeadRoutine());
    }

    public IEnumerator PlayerDeadRoutine(){
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }
}