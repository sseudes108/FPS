using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_FadeScreen : MonoBehaviour {
    private VisualElement _overLay;
    // private readonly float _fadeDuration = 1f;

    private void OnEnable() {
        GameManager.OnGameStart += GameManager_OnGameStart;
        GameManager.OnGameEnd += GameManager_OnGameEnd;
        Health.OnPlayerDamaged += Health_OnPlayerDamaged;
    }

    private void OnDisable() {
        GameManager.OnGameStart -= GameManager_OnGameStart;
        GameManager.OnGameEnd -= GameManager_OnGameEnd;
        Health.OnPlayerDamaged -= Health_OnPlayerDamaged;
    }

    private void Awake() { SetOverlayElement(); }

    private void Start() { _overLay.style.opacity = 1f; }

    private void GameManager_OnGameStart() { FadeOut(); }
    private void GameManager_OnGameEnd() { 
        SetOverlayElement();
        FadeIn(); 
    }

    private void SetOverlayElement() { _overLay = GameManager.Instance.UIManager.Root.Q("Overlay"); }
    
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
        _overLay.style.backgroundColor = color;

        do{
            elapsedTime += Time.deltaTime;
            float interpolation = Mathf.Clamp01(elapsedTime / duration);
            _overLay.style.opacity = Mathf.Lerp(start, end, interpolation);
            yield return null;
        }while(elapsedTime < duration);
    }

    private void Health_OnPlayerDamaged(){
        SetOverlayElement();
        StartCoroutine(DamageOverlayRoutine());
    }

    private IEnumerator DamageOverlayRoutine(){
        float start = 0;
        float end = 0.2f;
        float duration = 0.2f;

        StartCoroutine(FadeRoutine(start, end, duration, Color.red));
        yield return new WaitForSeconds(duration);
        StartCoroutine(FadeRoutine(end, start, duration, Color.red));

        yield return null;
    }
}