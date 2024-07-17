using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_VictoryScreen : MonoBehaviour {
    private VisualElement _victoryScreenText;
    [SerializeField] private AudioManagerSO _audioManager;

    private void SetElement(VisualElement root){
        _victoryScreenText = root.Q("VictoryScreenText");
        _victoryScreenText.style.opacity = 0;
        _victoryScreenText.visible = false;
    }

    public void ShowVictoryScreenText(VisualElement root){
        SetElement(root);
        StartCoroutine(OpacityRoutine());
    }

    private IEnumerator OpacityRoutine(){
        _victoryScreenText.visible = true;
        _audioManager.MuteGameMusic(this, 3f);
        yield return new WaitForSeconds(1.5f);
        float elapsedTime = 0f;
        var duration = 3f;
        while (elapsedTime < duration){
            float value = Mathf.Lerp(0, 1, elapsedTime / duration);
            _victoryScreenText.style.opacity = value;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _victoryScreenText.style.opacity = 1;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Locus");
    }
}