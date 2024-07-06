using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour {
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
            var randomIndex = Random.Range(0, GameManager.Instance.AudioManager.Database.InGameMusics.Count);
            PlayMusic(GameManager.Instance.AudioManager.Database.InGameMusics[randomIndex]);
            yield return new WaitForSeconds(GameManager.Instance.AudioManager.Database.InGameMusics[randomIndex].AudioClip.length); //Add efect do low the volume near the end and up from the start
            yield return null;
        }while(playMusic);
    }
}