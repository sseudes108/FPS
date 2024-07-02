using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField] private List<SoundSO> _musics = new();

    private bool playMusic;

    private void OnEnable() {
        GameManager.OnGameStart += GameManager_OnGameStart;
    }

    private void OnDisable() {
        GameManager.OnGameStart -= GameManager_OnGameStart;
    }

    private void GameManager_OnGameStart(){
        playMusic = true;
        StartCoroutine(MusicPlayRoutine());
    }

    public void PlayMusic(SoundSO musicToPlay){
        GameManager.Instance.AudioManager.SoundToPlay(musicToPlay);
    }

    public IEnumerator MusicPlayRoutine(){
        do{
            var randomIndex = Random.Range(0, _musics.Count);
            PlayMusic(_musics[randomIndex]);
            yield return new WaitForSeconds(_musics[randomIndex].AudioClip.length); //Add efect do low the volume near the end and up from the start
            yield return null;
        }while(playMusic);
    }
}