using System.Collections;
using UnityEngine;

public class TitleMenuAudio : MonoBehaviour{
    private AudioManager Manager;
    private AudioSource AudioSource;

    private void OnEnable() {
        TitleScreen.OnGameStarted += TitleScreen_OnGameStarted;
    }

    private void OnDisable() {
        TitleScreen.OnGameStarted -= TitleScreen_OnGameStarted;
    }

    private void TitleScreen_OnGameStarted(){
        StartCoroutine(VolumeRoutine(AudioSource, 1f, 0f, 50f));
    }

    private void Awake() {
        Manager = GetComponent<AudioManager>();
    }
    
    private void Start() {
        RandomSound();
        StartCoroutine(VolumeRoutine(AudioSource, 0f, 1f, 50f));
    }

    private void RandomSound(){
        var rand = Random.Range(0, Manager.Database.MainMenuMusics.Count);
        var randomSoundSo = Manager.CreateAudioSource(Manager.Database.MainMenuMusics[rand]);
        randomSoundSo.volume = 0;
        AudioSource = randomSoundSo;
    }

    private IEnumerator VolumeRoutine(AudioSource audioSource, float start, float end, float duration){
        audioSource.Play();

        float elapsedTime = 0;
        do{
            elapsedTime += Time.deltaTime;
            float interpolation = Mathf.Clamp01(elapsedTime / duration);
            audioSource.volume = Mathf.Lerp(start, end, interpolation);
            yield return null;
        }while(elapsedTime < duration);
    }

}
