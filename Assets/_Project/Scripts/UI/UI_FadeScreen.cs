using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_FadeScreen : MonoBehaviour {
    private VisualElement _overLay;
    private VisualElement _youDied;

    private void OnEnable() {
        GameManager.OnGameStart += GameManager_OnGameStart;
        GameManager.OnGameEnd += GameManager_OnGameEnd;
        Health.OnPlayerDied -= Health_OnPlayerDie;
    }

    private void OnDisable() {
        GameManager.OnGameStart += GameManager_OnGameStart;
        GameManager.OnGameEnd -= GameManager_OnGameEnd;
        Health.OnPlayerDied -= Health_OnPlayerDie;
    }

    public void Start() {
        SetElements();
    }

    private void Health_OnPlayerDie(){
        Debug.Log("Health_OnPlayerDie()");
        StartCoroutine(DeathRoutine());
    }

    private void GameManager_OnGameStart(){
        SetElements();
        FadeFromBlack();
    }

    private void GameManager_OnGameEnd() { 
        FadeToBlack();
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine() {
        Debug.Log("DeathRoutine");
        _overLay = GameManager.Instance.UIManager.Root.Q("Overlay");
        _youDied = GameManager.Instance.UIManager.Root.Q("YouDied");
        FadeToBlack();
        StartCoroutine(FadeRoutine(_youDied, 0, 1, 1f));
        yield return null;
    }

    private void SetElements() {
        Debug.Log("SetElements()");
        _overLay = GameManager.Instance.UIManager.Root.Q("Overlay");
        _youDied = GameManager.Instance.UIManager.Root.Q("YouDied");
        _overLay.style.backgroundColor = Color.black;
        _overLay.style.opacity = 1f;
        _youDied.style.opacity = 0f;
    }

    public void FadeToBlack(){
        Debug.Log("FadeToBlack");
        StartCoroutine(FadeRoutine(_overLay, 1, 0f, 1f));
    }

    public void FadeFromBlack(){
        Debug.Log("FadeFromBlack");
        if(this != null){
            StartCoroutine(FadeRoutine(_overLay, 1, 0f, 1f));
        }
    }

    private IEnumerator FadeRoutine(VisualElement element, float start, float end, float duration){
        Debug.Log("FadeRoutine");
        float elapsedTime = 0;
        yield return null;
        do{
            elapsedTime += Time.deltaTime;
            float interpolation = Mathf.Clamp01(elapsedTime / duration);
            element.style.opacity = Mathf.Lerp(start, end, interpolation);
            yield return null;
        }while(elapsedTime < duration);
    }
}