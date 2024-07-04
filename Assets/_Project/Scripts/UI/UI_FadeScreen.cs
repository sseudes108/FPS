using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_FadeScreen : MonoBehaviour {
    private VisualElement _overLay;
    private VisualElement _youDied;
    // private readonly float _fadeDuration = 1f;

    private void OnEnable() {
        GameManager.OnGameStart += GameManager_OnGameStart;
        GameManager.OnGameEnd += GameManager_OnGameEnd;
        Health.OnPlayerDamaged += Health_OnPlayerDamaged;
        Health.OnPlayerDied += Health_OnPlayerDie;
    }

    private void OnDisable() {
        GameManager.OnGameStart -= GameManager_OnGameStart;
        GameManager.OnGameEnd -= GameManager_OnGameEnd;
        Health.OnPlayerDamaged -= Health_OnPlayerDamaged;
        Health.OnPlayerDied -= Health_OnPlayerDie;
    }

    private void Awake() { SetOverlayElement(); }

    private void Start() { 
        _overLay.style.opacity = 1f; 
    }

    private void GameManager_OnGameStart() { FadeOut(); }
    private void GameManager_OnGameEnd() { 
        SetOverlayElement();
        FadeIn(); 
    }

    private void SetOverlayElement() { 
        _overLay = GameManager.Instance.UIManager.Root.Q("Overlay"); 
        _youDied = GameManager.Instance.UIManager.Root.Q("YouDied");
    }
    
    private void FadeIn() { StartCoroutine(FadeRoutine(0f, 1f, 1f, Color.black)); }

    private void FadeOut() { StartCoroutine(FadeRoutine(1f, 0f, 1f, Color.black)); }

    /// <summary>
    ///  Fade the screen from start to end during duration with the desired color
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="duration"></param>
    private IEnumerator FadeRoutine(float start, float end, float duration, Color color){
        float elapsedTime = 0;
        yield return null;
        _overLay.style.backgroundColor = color;
        do{
            elapsedTime += Time.deltaTime;
            float interpolation = Mathf.Clamp01(elapsedTime / duration);
            _overLay.style.opacity = Mathf.Lerp(start, end, interpolation);
            yield return null;
        }while(elapsedTime < duration);
    }

    private void Health_OnPlayerDie(){
        SetOverlayElement();
        _youDied.style.opacity = 1f;
        StartCoroutine(DeadOverlayRoutine(0f, 1f, 2f));
    }

    private void Health_OnPlayerDamaged(){
        SetOverlayElement();
        StartCoroutine(DamageOverlayRoutine(0f, 0.2f, 0.2f));
    }

    private IEnumerator DamageOverlayRoutine(float start, float end, float duration){
        yield return null;
        StartCoroutine(FadeRoutine(start, end, duration, new Color(125,16,16,255)));
        yield return new WaitForSeconds(duration);
        StartCoroutine(FadeRoutine(end, start, duration, Color.red));

        yield return null;
    }

    private IEnumerator DeadOverlayRoutine(float start, float end, float duration){
        yield return StartCoroutine(FadeRoutine(start, end, duration, new Color(125,16,16,255)));
        yield return new WaitForSeconds(duration);
        yield return null;
    }
}