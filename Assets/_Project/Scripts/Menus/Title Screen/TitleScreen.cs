using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreen : MonoBehaviour{
    [SerializeField] private VisualManagerSO VisualsManager;
    [SerializeField] private AudioManagerSO AudioManager;
    [SerializeField] private DataManagerSO DataManager;

    private UIDocument _uiDocument;
    private VisualElement _root;

    private Button _play;
    private Button _quit;

    private VisualElement _overLay;
    private VisualElement _buttonsBox;
    private VisualElement _mainContainer;

    private AudioSource _musicPlaying;
    private bool _playMusic;
    private bool _canclick;

    private void OnEnable() {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;

        _play = _root.Q<Button>("Play");
        _quit = _root.Q<Button>("Quit");

        _overLay = _root.Q("Overlay");
        _buttonsBox = _root.Q("ButtonsBox");
        _mainContainer = _root.Q("MainContainer");

        _play.clicked += OnPlay;
        _quit.clicked += OnQuit;
    }

    private void OnDisable() {
        _play.clicked -= OnPlay;
        _quit.clicked -= OnQuit;
    }

    private void Start(){
        SetVisuals();
        SetAudio();
    }

#region Button Clicks
    private void OnPlay(){
        if(!_canclick){return;}
        AudioManager.MuteTitleScreenMusic(this, _musicPlaying);
        StartCoroutine(OnPlay_StartGameRoutine());
        _canclick = false;
    }
    
    private IEnumerator OnPlay_StartGameRoutine(){
        StartCoroutine(FadeRoutine(_mainContainer, 1f, 0f, 0.7f));
        VisualsManager.FadeToBlack(3f);
        yield return new WaitForSeconds(4f);
        yield return null;

        SceneManager.LoadScene("Locus");
    }

    private void OnQuit(){
        Debug.Log("Quit!!");
        DataManager.SaveRespawnPoint(new Vector3(-20.15f, 1.2f, -23.4f));
        Application.Quit();
    }
#endregion

#region Audio
    public void SetAudio(){
        _playMusic = true;
        StartCoroutine(MusicRoutine());
    }

    public IEnumerator MusicRoutine(){
        do{
            SoundSO currentMusic = AudioManager.MainMenuMusics[Random.Range(0, AudioManager.MainMenuMusics.Count)];
            PlayMusic(currentMusic);
            yield return new WaitForSeconds(currentMusic.AudioClip.length);
        }while(_playMusic);
    }

    public void PlayMusic(SoundSO musicToPlay){
        var newMusic = AudioManager.CreateAudioSource(musicToPlay);
        _musicPlaying = newMusic;
        newMusic.transform.SetParent(transform);
        AudioManager.StartAudioSource(this, newMusic, musicToPlay);
    }
#endregion

#region Visuals
    public void SetVisuals(){
        _buttonsBox.style.opacity = 0;
        _mainContainer.style.opacity = 1;
        StartCoroutine(StartFadeOutRoutine());
    }

    private IEnumerator StartFadeOutRoutine(){
        StartCoroutine(FadeRoutine(_mainContainer, 0f, 1f, 2f));
        VisualsManager.FadeFromBlack(3f);
        yield return new WaitForSeconds(3f);
        yield return null;

        _overLay.style.display = DisplayStyle.None;
        StartCoroutine(FadeRoutine(_buttonsBox, 0f, 1f, 2f));
        yield return null;
        _canclick = true;
    }

    private IEnumerator FadeRoutine(VisualElement element, float start, float end, float duration){
        float elapsedTime = 0;
        do{
            elapsedTime += Time.deltaTime;
            float interpolation = Mathf.Clamp01(elapsedTime / duration);
            element.style.opacity = Mathf.Lerp(start, end, interpolation);
            yield return null;
        }while(elapsedTime < duration && element.style.opacity != end);
    }
#endregion
}
