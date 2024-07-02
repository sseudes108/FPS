using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreen : MonoBehaviour{
    private UIDocument _uiDocument;
    private VisualElement _root;

    private Button _play;
    private Button _quit;

    private VisualElement _overLay;

    private void OnEnable() {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;

        _play = _root.Q<Button>("Play");
        _quit = _root.Q<Button>("Quit");

        _overLay = _root.Q("Overlay");

        _play.clicked += OnPlay;
        _quit.clicked += OnQuit;
    }

    private void OnDisable() {
        _play.clicked -= OnPlay;
        _quit.clicked -= OnQuit;
    }

    private void Start(){
        _overLay.style.display = DisplayStyle.None;
    }

    private void OnPlay(){
        Debug.Log("OnPlay");
        StartCoroutine(PlayRoutine());
    }

    private void OnOptions(){
        Debug.Log("OnOptions");
    }

    private void OnQuit(){
        Debug.Log("OnQuit");
    }

    private IEnumerator PlayRoutine(){
        StartCoroutine(FadeRoutine(0f, 1f, 1f));
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Locus");
    }
    
    private IEnumerator FadeRoutine(float start, float end, float duration){
        _overLay.style.display = DisplayStyle.Flex;
        float elapsedTime = 0;
        do{
            elapsedTime += Time.deltaTime;
            float interpolation = Mathf.Clamp01(elapsedTime / duration);
            _overLay.style.opacity = Mathf.Lerp(start, end, interpolation);
            yield return null;
        }while(elapsedTime < duration);
    }
}
