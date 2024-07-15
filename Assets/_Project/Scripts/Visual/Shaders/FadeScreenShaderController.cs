using System.Collections;
using UnityEngine;

public class FadeScreenShaderController : MonoBehaviour {
    [field:SerializeField] public VisualManagerSO VisualsManager { get; private set;}
    [field:SerializeField] public HealthManagerSO HealthManager { get; private set;}
    [field:SerializeField] public GameManagerSO GameManager { get; private set;}
    [SerializeField] private Material _fadeScreenMaterial;
    private IEnumerator _fadeScreenTask;
    private float _currentRadius;
    [SerializeField] private Color _redDefaultColor;

    private void OnEnable() {
        VisualsManager.OnFadeFromBlack.AddListener(VisualsManager_OnFadeFromBlack);
        VisualsManager.OnFadeToBlack.AddListener(VisualsManager_OnfadeToBlack);
        HealthManager.OnPlayerDamaged.AddListener(HealthManager_OnPlayerDamaged);
        HealthManager.OnPlayerDied.AddListener(HealthManager_OnPlayerDied);
        GameManager.OnGameStart.AddListener(GameManager_OnGameStart);
        GameManager.OnGameFinished.AddListener(GameManager_OnGameFinished);
    }

    private void OnDisable() {
        VisualsManager.OnFadeFromBlack.RemoveListener(VisualsManager_OnFadeFromBlack);
        VisualsManager.OnFadeToBlack.RemoveListener(VisualsManager_OnfadeToBlack);
        HealthManager.OnPlayerDamaged.RemoveListener(HealthManager_OnPlayerDamaged);
        HealthManager.OnPlayerDied.RemoveListener(HealthManager_OnPlayerDied);
        GameManager.OnGameStart.RemoveListener(GameManager_OnGameStart);
        GameManager.OnGameFinished.RemoveListener(GameManager_OnGameFinished);

        ResetVignete();
    }

    private void GameManager_OnGameFinished(){
        FadeToWhite(1f);
    }

    private void GameManager_OnGameStart(){
        StartCoroutine(FadeFromBlackRoutine(2f));
    }
    
    private void HealthManager_OnPlayerDamaged(){
        DamageEffect(Random.Range(0.1f, 1));
    }

    private void HealthManager_OnPlayerDied(){
        DeathEffect();
    }

    private void Start() {
        ChangeColor(Color.black);
        _currentRadius = -1;
        _fadeScreenMaterial.SetFloat("_VigneteRadius", _currentRadius);
    }

    public void DamageEffect(float intensity){
        if(_fadeScreenTask == null){
            _fadeScreenMaterial.SetFloat("_ApplyNoise", 1f);
            _fadeScreenMaterial.SetFloat("_VigneteRadius", _currentRadius);
            _fadeScreenTask = ScreenDamageRoutine(intensity);
            StartCoroutine(_fadeScreenTask);
        }
    }

    public void ResetVignete(){
        _currentRadius = 2;
        _fadeScreenMaterial.SetFloat("_VigneteRadius", _currentRadius);
        _fadeScreenMaterial.SetColor("_Tint", _redDefaultColor);
    }

    private IEnumerator ScreenDamageRoutine(float intensity){
        ChangeColor(_redDefaultColor);
        var targetRadius = Remap(intensity, 0, 1, 0.4f, -0.15f);
        _currentRadius = 2f; //No Dagame

        for(float t = 0; _currentRadius != targetRadius; t += Time.deltaTime * 3f){
            _currentRadius = Mathf.Lerp(1, targetRadius, t);
            _fadeScreenMaterial.SetFloat("_VigneteRadius", _currentRadius);
            yield return null;
        }

        for(float t = 0; _currentRadius < 1 ; t += Time.deltaTime * 3f){
            _currentRadius = Mathf.Lerp(targetRadius, 2, t);
            _fadeScreenMaterial.SetFloat("_VigneteRadius", _currentRadius);
            yield return null;
        }

        _fadeScreenTask = null;
    }

    public float Remap(float value, float fromMin, float fromMax, float toMin, float toMax){
        return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
    }

    public void DeathEffect(){
        StartCoroutine(DeathEffectRoutine());
    }

    private IEnumerator DeathEffectRoutine(){
        StartCoroutine(LerpColorToBlack(2f, 1f));
        StartCoroutine(FadeToBlackRoutine(2f));
        yield return null;
    }

    private IEnumerator LerpColorToBlack(float wait, float duration){
        yield return new WaitForSeconds(wait);
        Color startColor = _redDefaultColor;
        Color endColor = Color.black;

        float time = 0f;
        do{
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            Color currentColor = Color.Lerp(startColor, endColor, t);
            _fadeScreenMaterial.SetColor("_Tint", currentColor);
            yield return null;
        }while(startColor != endColor);
    }

    public void FadeToBlack(float duration){
        StartCoroutine(FadeToBlackRoutine(duration));
    }

    private IEnumerator FadeToBlackRoutine(float duration){
        var targetRadius = -1;
        
        float time = 0f;
        do{
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            _currentRadius = Mathf.Lerp(2, targetRadius, t);
            _fadeScreenMaterial.SetFloat("_VigneteRadius", _currentRadius);
            yield return null;
        }while(_currentRadius > targetRadius);
    }

    public void FadeFromBlack(float duration){
        StartCoroutine(FadeFromBlackRoutine(duration));
    }

    private IEnumerator FadeFromBlackRoutine(float duration){
        _fadeScreenMaterial.SetFloat("_ApplyNoise", 0);
        _fadeScreenMaterial.SetFloat("_VigneteRadius", 0);
        _fadeScreenMaterial.SetColor("_Tint", Color.black);
        var targetRadius = 2;
        float time = 0;
        do{
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            _currentRadius = Mathf.Lerp(0, targetRadius, t);
            _fadeScreenMaterial.SetFloat("_VigneteRadius", _currentRadius);
            yield return null;
        }while(_currentRadius < targetRadius);

        ChangeColor(_redDefaultColor);
    }

    private void FadeToWhite(float duration){
        ChangeColor(Color.white);
        StartCoroutine(FadeToWhiteRoutine(duration));
    }

    private IEnumerator FadeToWhiteRoutine(float duration){
        var targetRadius = -1;
        
        float time = 0f;
        do{
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            _currentRadius = Mathf.Lerp(2, targetRadius, t);
            _fadeScreenMaterial.SetFloat("_VigneteRadius", _currentRadius);
            yield return null;
        }while(_currentRadius > targetRadius);
    }

    private void ChangeColor(Color newColor){
        _fadeScreenMaterial.SetColor("_Tint", newColor);
    }

    public void VisualsManager_OnfadeToBlack(float duration){ // Made for UI_TitleScreen.cs
        ChangeColor(Color.black);
        FadeToBlack(duration);
    }

    public void VisualsManager_OnFadeFromBlack(float duration){ // Made for UI_TitleScreen.cs
        ChangeColor(Color.black);
        FadeFromBlack(duration);
    }
}