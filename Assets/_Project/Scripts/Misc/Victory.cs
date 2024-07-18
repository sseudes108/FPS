using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour {
    [SerializeField] private int _portalIndex;
    [SerializeField] private string _loadTo;
    [SerializeField] private GameManagerSO _gameManager;
    [SerializeField] private VisualManagerSO _visualManager;
    [SerializeField] private AudioManagerSO _audioManager;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(_portalIndex == 0){
                _gameManager.GameFinished();
                return;
            }
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine(){
        _visualManager.FadeScreenShader.FadeToBlackChePoint(1.5f);
        _audioManager.MuteGameMusic(this, 1.5f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(_loadTo);
    }
}