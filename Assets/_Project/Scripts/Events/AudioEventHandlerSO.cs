using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "AudioEventHandler", menuName = "FPS/EventHandlers/Audio", order = 0)]
public class AudioEventHandlerSO : ScriptableObject {
    public ObjectPoolSO ObjectPool;
    public ObjectPool<AudioSource> AudioPool;
    public AudioSource AudioPrefab;
    public AudioMixerGroup MusicMixer, EffectMixer;

    public UnityEvent OnGameStart;

    public SoundSO Click;
    public SoundSO Step;
    public SoundSO CrossHairChange;

    public List<SoundSO> MainMenuMusics;
    public List<SoundSO> InGameMusics;
    
    private void OnEnable() {
        AudioPool ??= ObjectPool.CreateAudioPool(AudioPrefab);
    }
    
    public void PlayClickSound(MonoBehaviour starter) {
        PlayAudioEffect(starter, Click);
    }

    public void PlayStepSound(MonoBehaviour starter) {
        PlayAudioEffect(starter, Step);
    }

    public void PlayCrossHairChangeSound(MonoBehaviour starter) {
        PlayAudioEffect(starter, CrossHairChange); 
    }
    public void PlayShootSound(MonoBehaviour starter, SoundSO shootSound) { 
        PlayAudioEffect(starter, shootSound);
    }

    public void PlayReloadSound(MonoBehaviour starter, SoundSO reloadSound) {
        PlayAudioEffect(starter, reloadSound);
    }

    public void PlayHandleGunSound(MonoBehaviour starter, SoundSO handleGunSound) {
        PlayAudioEffect(starter, handleGunSound);
    }

    public void PlayStartGameMusic() { OnGameStart?.Invoke(); }

    public void ReleaseFromPool(AudioSource audioSource) { AudioPool.Release(audioSource); }

    public IEnumerator VolumeRoutine(AudioSource audioSource, float start, float end, float duration){
        if(!audioSource.isPlaying){
            audioSource.Play();
        }
        
        float elapsedTime = 0;
        do{
            elapsedTime += Time.deltaTime;
            float interpolation = Mathf.Clamp01(elapsedTime / duration);
            audioSource.volume = Mathf.Lerp(start, end, interpolation);
            yield return null;
        }while(elapsedTime < duration);
    }

    public AudioSource CreateAudioSource(SoundSO soundSO){
        AudioSource newAudioSource;
        
        do{
            newAudioSource = AudioPool.Get();
        }while(newAudioSource == null);

        newAudioSource.clip = soundSO.AudioClip;
        newAudioSource.volume = soundSO.Volume;
        newAudioSource.loop = soundSO.Loop;

        if(soundSO.RandomizePitch){
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchModifier, soundSO.RandomPitchModifier);
            newAudioSource.pitch = soundSO.Pitch + randomPitchModifier;
        }

        if(soundSO.AudioType == SoundSO.AudioTypes.Music){
            newAudioSource.outputAudioMixerGroup = MusicMixer;
        }else{
            newAudioSource.outputAudioMixerGroup = EffectMixer;
        }

        return newAudioSource;
    }

    public IEnumerator ReleaseFromPool(AudioSource audioSource, float audioLenght){
        yield return new WaitForSeconds(audioLenght);
        AudioPool.Release(audioSource);
    }

    public void PlayAudioEffect(MonoBehaviour starter, SoundSO soundSO){
        var newSound = CreateAudioSource(soundSO);
        StartAudioSource(starter, newSound, soundSO);
    }

    public void StartAudioSource(MonoBehaviour starter, AudioSource newAudioSource, SoundSO soundSO){
        newAudioSource.Play();
        if(!newAudioSource.loop){
            starter.StartCoroutine(ReleaseFromPool(newAudioSource, soundSO.AudioClip.length));
        }
    }

    public void MuteMusicSound(MonoBehaviour starter, AudioSource audio){
        starter.StartCoroutine(VolumeRoutine(audio, 1f, 0f, 2f));
    }
}