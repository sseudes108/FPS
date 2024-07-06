using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreen : MonoBehaviour{
    public static Action OnGameStarted;

    private UIDocument _uiDocument;
    private VisualElement _root;

    private Button _play;
    private Button _quit;

    private VisualElement _overLay;
    private VisualElement _buttonsBox;


    private void OnEnable() {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;

        _play = _root.Q<Button>("Play");
        _quit = _root.Q<Button>("Quit");

        _overLay = _root.Q("Overlay");
        _buttonsBox = _root.Q("ButtonsBox");

        _play.clicked += OnPlay;
        _quit.clicked += OnQuit;
    }

    private void OnDisable() {
        _play.clicked -= OnPlay;
        _quit.clicked -= OnQuit;
    }

    private void Start(){
        _overLay.style.opacity = 1;
        _buttonsBox.style.opacity = 0;
        
        StartCoroutine(StartFadeOutRoutine());
    }

    private void OnPlay(){
        Debug.Log("OnPlay");
        StartCoroutine(StartGameRoutine());
    }

    private void OnOptions(){
        Debug.Log("OnOptions");
    }

    private void OnQuit(){
        Debug.Log("OnQuit");
    }

    private IEnumerator StartFadeOutRoutine(){
        StartCoroutine(FadeRoutine(_overLay, 1f, 0f, 6f));
        yield return new WaitForSeconds(6f);
        yield return null;
        _overLay.style.display = DisplayStyle.None;
        
        StartCoroutine(FadeRoutine(_buttonsBox, 0f, 1f, 2f));
        yield return null;
    }

    private IEnumerator StartGameRoutine(){
        OnGameStarted?.Invoke();
        yield return null;

        StartCoroutine(FadeRoutine(_overLay, 0f, 1f, 4f));
        yield return new WaitForSeconds(4f);
        yield return null;
        
        SceneManager.LoadScene("Locus");
    }
    
    private IEnumerator FadeRoutine(VisualElement element, float start, float end, float duration){
        float elapsedTime = 0;
        do{
            elapsedTime += Time.deltaTime;
            float interpolation = Mathf.Clamp01(elapsedTime / duration);
            element.style.opacity = Mathf.Lerp(start, end, interpolation);
            yield return null;
        }while(elapsedTime < duration);
    }

}
