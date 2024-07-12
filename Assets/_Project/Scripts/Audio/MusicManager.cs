using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    private bool _playMusic;
    // private AudioSource _musicPlaying;
    [SerializeField] private AudioEventHandlerSO AudioManager;

    private void OnEnable() {
        AudioManager.OnGameStart.AddListener(AudioManager_OnGameStart);
    }

    private void OnDisable() {
        AudioManager.OnGameStart.RemoveListener(AudioManager_OnGameStart);
    }
    
    private void Start() {
        _playMusic = true;
    }

    private void AudioManager_OnGameStart(){
        StartCoroutine(StartMusic());
    }

    public IEnumerator StartMusic(){
        do{
            SoundSO currentMusic = AudioManager.InGameMusics[Random.Range(0, AudioManager.InGameMusics.Count)];
            PlayMusic(currentMusic);
            //Create routine to volume down
            yield return new WaitForSeconds(currentMusic.AudioClip.length + Random.Range(30f, 90f));
        }while(_playMusic);
    }

    public void PlayMusic(SoundSO musicToPlay){
        var newMusic = AudioManager.CreateAudioSource(musicToPlay);
        // _musicPlaying = newMusic;
        newMusic.transform.SetParent(transform);
        AudioManager.StartAudioSource(this, newMusic, musicToPlay);
    }
}