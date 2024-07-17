using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "AudioManagerSO", menuName = "FPS/Managers/Audio", order = 0)]
public class AudioManagerSO : ScriptableObject {
    [SerializeField] private DataManagerSO _dataManager;

    [HideInInspector] public UnityEvent OnGameStart;
    
    public ObjectPool<AudioSource> AudioPool { get; private set; }
    [SerializeField] private ObjectPoolManagerSO _objectPool;
    [SerializeField] private AudioSource _audioPrefab;

    [SerializeField] private AudioMixerGroup _musicMixer, _gameEffectMixer, _uIEffectMixer;

    [SerializeField] private SoundSO Click, Step, CrossHairChange;

    public List<SoundSO> MainMenuMusics, InGameMusics;
    public AudioSource MusicPlaying { get; private set; }

    public float EffectVolume;
    public float MusicVolume;
    
    private void OnEnable() {
        AudioPool ??= _objectPool.CreateAudioPool(_audioPrefab);

        OnGameStart ??= new UnityEvent();
        EffectVolume = _dataManager.LoadEffectVolume() /100;
        MusicVolume = _dataManager.LoadMusicVolume() /100;
    }

    public void PlayClickSound(MonoBehaviour caller) {
        PlayAudioEffect(caller, Click);
    }

    public void PlayStepSound(MonoBehaviour caller) {
        PlayAudioEffect(caller, Step);
    }

    public void PlayCrossHairChangeSound(MonoBehaviour caller) {
        PlayAudioEffect(caller, CrossHairChange); 
    }
    
    public void PlayShootSound(MonoBehaviour caller, SoundSO shootSound) { 
        PlayAudioEffect(caller, shootSound);
    }

    public void PlayReloadSound(MonoBehaviour caller, SoundSO reloadSound) {
        PlayAudioEffect(caller, reloadSound);
    }

    public void PlayHandleGunSound(MonoBehaviour caller, SoundSO handleGunSound) {
        PlayAudioEffect(caller, handleGunSound);
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

        switch(soundSO.AudioType){

            case SoundSO.AudioTypes.Music:
                newAudioSource.outputAudioMixerGroup = _musicMixer;
            break;

            case SoundSO.AudioTypes.GameEffect:
                newAudioSource.outputAudioMixerGroup = _gameEffectMixer;
            break;

            case SoundSO.AudioTypes.UIEffect:
                newAudioSource.outputAudioMixerGroup = _uIEffectMixer;
            break;
        }

        return newAudioSource;
    }

    public IEnumerator ReleaseFromPool(AudioSource audioSource, float audioLenght){
        yield return new WaitForSeconds(audioLenght);
        AudioPool.Release(audioSource);
    }

    public void PlayAudioEffect(MonoBehaviour caller, SoundSO soundSO){
        var newSound = CreateAudioSource(soundSO);
        newSound.volume = EffectVolume;
        StartAudioSource(caller, newSound, soundSO);
    }

    public void StartAudioSource(MonoBehaviour caller, AudioSource newAudioSource, SoundSO soundSO){
        newAudioSource.Play();
        if(!newAudioSource.loop){
            caller.StartCoroutine(ReleaseFromPool(newAudioSource, soundSO.AudioClip.length));
        }
    }

    public void MuteTitleScreenMusic(MonoBehaviour caller, AudioSource audio){
        caller.StartCoroutine(VolumeRoutine(audio, 1f, 0f, 2f));
    }

    public void SetMusicPlaying(AudioSource musicPlaying){
        MusicPlaying = musicPlaying;
    }

    public void MuteGameMusic(MonoBehaviour caller, float duration){
        caller.StartCoroutine(VolumeRoutine(MusicPlaying, MusicPlaying.volume, 0, duration));
    }

    public void SetMusicVolume(float value){
        MusicVolume = value;
        MusicPlaying.volume = MusicVolume;
        _dataManager.SaveMusicVolume(MusicVolume);
    }

    public void SetEffectVolume(float value){
        EffectVolume = value;
        _dataManager.SaveEffectVolume(EffectVolume);
    }
}