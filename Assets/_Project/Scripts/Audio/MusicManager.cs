using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    private bool _playMusic;
    private AudioSource _musicPlaying;
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
            yield return StartCoroutine(AudioManager.VolumeRoutine(_musicPlaying, 0, 1f, 10f));
            yield return new WaitForSeconds(currentMusic.AudioClip.length - 12f);
        }while(_playMusic);
    }

    public void PlayMusic(SoundSO musicToPlay){
        if(this != null){
            AudioSource newMusic = null;
            do{
                newMusic = AudioManager.AudioPool.Get();
            }while(newMusic == null);
            // var newMusic = AudioManager.AudioPool.Get();
            _musicPlaying = newMusic;
            _musicPlaying.volume = 0f;
            newMusic.transform.SetParent(transform);

            AudioManager.Init(newMusic, musicToPlay);
            newMusic.Play();
            if(!newMusic.loop){
                StartCoroutine(AudioManager.ReleaseFromPool(newMusic, musicToPlay.AudioClip.length));
            }
        }
    }
}