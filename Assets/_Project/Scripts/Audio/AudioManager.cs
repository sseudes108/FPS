using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioDatabase), typeof(SFXManager), typeof(MusicManager))]
public class AudioManager : MonoBehaviour{
    public AudioDatabase Database;
    [SerializeField] private AudioMixerGroup _musicMixerGroup, _SFXMixerGroup;

    private void Awake() {
        Database = GetComponent<AudioDatabase>();
    }

    public void SoundToPlay(SoundSO soundSO) {
        GameObject soundObject = new("Tempo. Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = soundSO.AudioClip;
        audioSource.volume = soundSO.Volume;
        audioSource.loop = soundSO.Loop;
        audioSource.outputAudioMixerGroup = DetermineAudioMixerGroup(soundSO);

        if(soundSO.RandomizePitch){
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchModifier, soundSO.RandomPitchModifier);
            audioSource.pitch = soundSO.Pitch + randomPitchModifier;
        }

        audioSource.Play();
        if(!audioSource.loop){Destroy(soundObject, soundSO.AudioClip.length);}
    }

    public AudioSource CreateAudioSource(SoundSO soundSO){
        GameObject soundObject = new("Tempo. Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = soundSO.AudioClip;
        audioSource.volume = soundSO.Volume;
        audioSource.loop = soundSO.Loop;
        audioSource.outputAudioMixerGroup = DetermineAudioMixerGroup(soundSO);

        if(soundSO.RandomizePitch){
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchModifier, soundSO.RandomPitchModifier);
            audioSource.pitch = soundSO.Pitch + randomPitchModifier;
        }

        return audioSource;
    }

    private AudioMixerGroup DetermineAudioMixerGroup(SoundSO soundSO){
        AudioMixerGroup audioMixerGroup;
        switch (soundSO.AudioType){
            case SoundSO.AudioTypes.SFX:
                audioMixerGroup = _SFXMixerGroup;
                break;
            case SoundSO.AudioTypes.Music:
                audioMixerGroup = _musicMixerGroup;
                break;
            default:
                audioMixerGroup = null;
                break;
        }
        return audioMixerGroup;
    }
}