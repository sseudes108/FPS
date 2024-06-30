using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_FadeScreen : MonoBehaviour {
    private VisualElement _overLay;
    private readonly float _fadeDuration = 1f;

    private void OnEnable() {
        GameManager.OnGameStart += GameManager_OnGameStart;
        GameManager.OnGameEnd += GameManager_OnGameEnd;
    }

    private void OnDisable() {
        GameManager.OnGameStart -= GameManager_OnGameStart;
        GameManager.OnGameEnd -= GameManager_OnGameEnd;
    }

    private void Awake() { SetOverlayElement(); }

    private void Start() { _overLay.style.opacity = 1f; }

    private void GameManager_OnGameStart() { FadeOut(); }
    private void GameManager_OnGameEnd() { 
        SetOverlayElement();
        FadeIn(); 
    }

    private void SetOverlayElement() { _overLay = GameManager.Instance.UIManager.Root.Q("Overlay"); }
    
    private void FadeIn() { StartCoroutine(FadeRoutine(0f, 1f)); }

    private void FadeOut() { StartCoroutine(FadeRoutine(1f, 0f)); }

    private IEnumerator FadeRoutine(float start, float end){
        float elapsedTime = 0;
        do{
            elapsedTime += Time.deltaTime;
            float interpolation = Mathf.Clamp01(elapsedTime / _fadeDuration);
            _overLay.style.opacity = Mathf.Lerp(start, end, interpolation);
            yield return null;
        }while(elapsedTime < _fadeDuration);
    }
}