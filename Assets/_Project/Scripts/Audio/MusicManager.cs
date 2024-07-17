using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    private bool _playMusic;
    [SerializeField] private AudioManagerSO _audioManager;
    [SerializeField] private PauseMenuManagerSO _pauseManager;
    
    private void OnEnable() {
        _audioManager.OnGameStart.AddListener(AudioManager_OnGameStart);
        _pauseManager.OnMusicVolumeChange.AddListener(PauseManager_OnMusicVolumeChange);
        _pauseManager.OnEffectVolumeChange.AddListener(PauseManager_OnEffectVolumeChange);
    }

    private void OnDisable() {
        _audioManager.OnGameStart.RemoveListener(AudioManager_OnGameStart);
        _pauseManager.OnMusicVolumeChange.RemoveListener(PauseManager_OnMusicVolumeChange);
        _pauseManager.OnEffectVolumeChange.RemoveListener(PauseManager_OnEffectVolumeChange);
    }

    private void PauseManager_OnEffectVolumeChange(float value){
        _audioManager.SetEffectVolume(value / 100);
    }

    private void PauseManager_OnMusicVolumeChange(float value){
        _audioManager.SetMusicVolume(value / 100);
    }

    private void Start() {
        _playMusic = true;
    }

    private void AudioManager_OnGameStart(){
        StartCoroutine(StartMusic());
    }

    public IEnumerator StartMusic(){
        do{
            SoundSO currentMusic = _audioManager.InGameMusics[Random.Range(0, _audioManager.InGameMusics.Count)];
            PlayMusic(currentMusic);
            yield return new WaitForSeconds(currentMusic.AudioClip.length - 10f);
            Debug.Log("currentMusic.AudioClip.length - 10f");
            _audioManager.MuteGameMusic(this, 10f);
            Debug.Log("AudioManager.MuteGameMusic(this)");
            yield return new WaitForSeconds(Random.Range(30f, 90f));
        }while(_playMusic);
    }

    public void PlayMusic(SoundSO musicToPlay){
        var newMusic = _audioManager.CreateAudioSource(musicToPlay);
        newMusic.volume = _audioManager.MusicVolume;
        _audioManager.SetMusicPlaying(newMusic);
        
        newMusic.transform.SetParent(transform);
        _audioManager.StartAudioSource(this, newMusic, musicToPlay);
    }
}