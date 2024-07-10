using System.Collections;
using UnityEngine;

public class FadeScreenShaderController : MonoBehaviour {
    [SerializeField] private Material _fadeScreenMaterial;
    private IEnumerator _fadeScreenTask;
    private float _currentRadius;
    [SerializeField] private Color _redDefaultColor;

    private void OnEnable() {
        Health.OnPlayerDamaged += Health_OnPlayerDamaged;
        Health.OnPlayerDied += Health_OnPlayerDie;
        GameManager.OnGameStart += GameManager_OnGameStart;
    }

    private void OnDisable() {
        Health.OnPlayerDamaged -= Health_OnPlayerDamaged;
        Health.OnPlayerDied -= Health_OnPlayerDie;
        GameManager.OnGameStart -= GameManager_OnGameStart;

        ResetVignete();
    }

    private void GameManager_OnGameStart(){
        StartCoroutine(FadeFromBlackRoutine(2f));
    }
    
    private void Health_OnPlayerDamaged(){
        DamageEffect(Random.Range(0.1f, 1));
    }

    private void Health_OnPlayerDie(){
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
        _currentRadius = 1;
        _fadeScreenMaterial.SetFloat("_VigneteRadius", _currentRadius);
        _fadeScreenMaterial.SetColor("_Tint", _redDefaultColor);
    }

    private IEnumerator ScreenDamageRoutine(float intensity){
        ChangeColor(_redDefaultColor);
        var targetRadius = Remap(intensity, 0, 1, 0.4f, -0.15f);
        _currentRadius = 1f; //No Dagame

        for(float t = 0; _currentRadius != targetRadius; t += Time.deltaTime * 3f){
            _currentRadius = Mathf.Lerp(1, targetRadius, t);
            _fadeScreenMaterial.SetFloat("_VigneteRadius", _currentRadius);
            yield return null;
        }

        for(float t = 0; _currentRadius < 1 ; t += Time.deltaTime * 3f){
            _currentRadius = Mathf.Lerp(targetRadius, 1, t);
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
        yield return new WaitForSeconds(1f);
        GameManager.Instance.UIManager.DeathScreen.PlayerDied();
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
            _currentRadius = Mathf.Lerp(1, targetRadius, t);
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
        var targetRadius = 1;
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

    private void ChangeColor(Color newColor){
        Debug.Log("ChangeColor Called");
        _fadeScreenMaterial.SetColor("_Tint", newColor);
    }

    public void FadeScreenToBlack(float duration){ // Made for UI_TitleScreen.cs
        ChangeColor(Color.black);
        FadeToBlack(duration);
    }

    public void FadeScreenFromBlack(float duration){ // Made for UI_TitleScreen.cs
        ChangeColor(Color.black);
        FadeFromBlack(duration);
    }
}