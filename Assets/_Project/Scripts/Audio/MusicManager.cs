using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    private bool _playMusic;
    [SerializeField] private AudioManagerSO AudioManager;

    private AudioSource _currentMusicPlaying;

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
            yield return new WaitForSeconds(currentMusic.AudioClip.length - 3f);
            Debug.Log("currentMusic.AudioClip.length - 3f");
            AudioManager.MuteGameMusic(this);
            Debug.Log("AudioManager.MuteGameMusic(this)");
            yield return new WaitForSeconds(currentMusic.AudioClip.length + Random.Range(30f, 90f));
        }while(_playMusic);
    }

    public void PlayMusic(SoundSO musicToPlay){
        var newMusic = AudioManager.CreateAudioSource(musicToPlay);
        AudioManager.SetMusicPlaying(newMusic);
        newMusic.transform.SetParent(transform);
        AudioManager.StartAudioSource(this, newMusic, musicToPlay);
    }
}