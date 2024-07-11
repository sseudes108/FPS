using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "AudioEventHandler", menuName = "FPS/EventHandlers/Audio", order = 0)]
public class AudioEventHandlerSO : ScriptableObject {
    public AudioSource AudioPrefab;

    public UnityEvent OnGameStart;
    public UnityEvent<SoundSO> OnStep;
    public UnityEvent<SoundSO> OnClick, OnCrossHairChange;
    public UnityEvent<SoundSO> OnShoot, OnReload, OnHandleGun;

    public ObjectPool<AudioSource> AudioPool;

    public SoundSO Click;
    public SoundSO Step;
    public SoundSO CrossHairChange;

    public List<SoundSO> MainMenuMusics;
    public List<SoundSO> InGameMusics;
    
    private void OnEnable() {
        AudioPool ??= CreateAudioSourcePool();

        //Events
        OnClick ??= new UnityEvent<SoundSO>();
        OnCrossHairChange ??= new UnityEvent<SoundSO>();
        OnStep ??= new UnityEvent<SoundSO>();
        OnShoot ??= new UnityEvent<SoundSO>();
        OnHandleGun ??= new UnityEvent<SoundSO>();
    }
    
    public void PlayClickSound() { OnClick?.Invoke(Click); }
    public void PlayStepSound() { OnClick?.Invoke(Step); }
    public void PlayCrossHairChangeSound() { OnClick?.Invoke(CrossHairChange); }
    public void PlayShootSound(SoundSO shootSound) { OnShoot?.Invoke(shootSound); }
    public void PlayReloadSound(SoundSO shootSound) { OnShoot?.Invoke(shootSound); }
    public void PlayHandleGunSound(SoundSO shootSound) { OnShoot?.Invoke(shootSound); }
    public void PlayStartGameMusic() { OnGameStart?.Invoke(); }

    public AudioSource GetRandomMainMenuMusic(){
        var rand = Random.Range(0, MainMenuMusics.Count);
        var randomSoundSo = CreateAudioSource(MainMenuMusics[rand]);
        randomSoundSo.volume = 0;
        return randomSoundSo;
    }

    public AudioSource CreateAudioSource(SoundSO soundSO){
        GameObject soundObject = new($"Tempo. Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = soundSO.AudioClip;
        audioSource.volume = soundSO.Volume;
        audioSource.loop = soundSO.Loop;

        if(soundSO.RandomizePitch){
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchModifier, soundSO.RandomPitchModifier);
            audioSource.pitch = soundSO.Pitch + randomPitchModifier;
        }

        return audioSource;
    }

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

    public void Init(AudioSource audioSource, SoundSO soundSO){
        audioSource.clip = soundSO.AudioClip;
        audioSource.volume = soundSO.Volume;
        audioSource.loop = soundSO.Loop;

        if(soundSO.RandomizePitch){
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchModifier, soundSO.RandomPitchModifier);
            audioSource.pitch = soundSO.Pitch + randomPitchModifier;
        }

        audioSource.Play();
        if(!audioSource.loop){
            GameController.Instance.StartCoroutine(ReleaseFromPool(audioSource, soundSO.AudioClip.length));
        }
    }

    public IEnumerator ReleaseFromPool(AudioSource audioSource, float audioLenght){
        yield return new WaitForSeconds(audioLenght);
        AudioPool.Release(audioSource);
    }

    public void PlayAudio(SoundSO soundSO){
        var newAudio = AudioPool.Get();
        Init(newAudio, soundSO);
    }

    public ObjectPool<AudioSource> CreateAudioSourcePool(){
        var audioPool = new ObjectPool<AudioSource>(()=>{
            return Instantiate(AudioPrefab);
        }, newAudio =>{
            if(newAudio != null){
                newAudio.gameObject.SetActive(true);
            }
        }, newAudio =>{
            newAudio.gameObject.SetActive(false);
        }, newAudio =>{
            Destroy(newAudio);
        }, false, 50, 70);

        return audioPool;
    }
}