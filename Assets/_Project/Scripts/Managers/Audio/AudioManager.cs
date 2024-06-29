using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour{
    [Range(0, 2)]
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private AudioMixerGroup _musicMixerGroup, _SFXMixerGroup;

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