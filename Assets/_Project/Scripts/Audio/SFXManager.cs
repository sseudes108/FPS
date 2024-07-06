using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SFXManager : MonoBehaviour {
    [SerializeField] private SoundSO _footStep;
    [SerializeField] private ObjectPool<AudioSource> _SFXPool;

    private void OnEnable() {
        PlayerGun.OnHandleGun += PlayerGun_OnHandleGun;
        PlayerGun.OnShootFired += PlayerGun_OnShootFired;
        PlayerGun.OnGunReload += PlayerGun_OnGunReload;
        PlayerMove.OnStep += PlayerMove_OnStep;
    }

    private void OnDisable() {
        PlayerGun.OnHandleGun -= PlayerGun_OnHandleGun;
        PlayerGun.OnShootFired -= PlayerGun_OnShootFired;
        PlayerGun.OnGunReload -= PlayerGun_OnGunReload;
        PlayerMove.OnStep -= PlayerMove_OnStep;    
    }

    private void Start() {
        _SFXPool = GameManager.Instance.ObjectPoolManager.AudioPool;
    }

    private void PlayerGun_OnHandleGun(SoundSO GunHandlingSound){
        PlayAudio(GunHandlingSound);
    }

    private void PlayerGun_OnShootFired(SoundSO shootSound){
        PlayAudio(shootSound);
    }
    
    private void PlayerGun_OnGunReload(SoundSO reloadSound){
        PlayAudio(reloadSound);
    }
 
    private void PlayerMove_OnStep(){
        PlayAudio(_footStep);
    }

    private void PlayAudio(SoundSO soundSO){
        var newEffect = _SFXPool.Get();
        newEffect.transform.SetParent(transform);
        Init(newEffect, soundSO);
    }

    private IEnumerator ReleaseFromPool(AudioSource audioSource, float audioLenght){
        yield return new WaitForSeconds(audioLenght);
        _SFXPool.Release(audioSource);
    }

    private void Init(AudioSource audioSource, SoundSO soundSO){
        audioSource.clip = soundSO.AudioClip;
        audioSource.volume = soundSO.Volume;
        audioSource.loop = soundSO.Loop;

        if(soundSO.RandomizePitch){
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchModifier, soundSO.RandomPitchModifier);
            audioSource.pitch = soundSO.Pitch + randomPitchModifier;
        }

        audioSource.Play();
        if(!audioSource.loop){
            StartCoroutine(ReleaseFromPool(audioSource, soundSO.AudioClip.length));
        }
    }
}