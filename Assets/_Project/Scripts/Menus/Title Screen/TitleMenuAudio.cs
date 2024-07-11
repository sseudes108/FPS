using UnityEngine;

public class TitleMenuAudio : MonoBehaviour{
    // [SerializeField] private AudioManager _manager;
    private AudioSource AudioSource;
    [SerializeField] private AudioEventHandlerSO AudioManager;

    private void Start() {
        RandomSound();
        StartCoroutine(AudioManager.VolumeRoutine(AudioSource, 0f, 1f, 3f));
    }

    public void MuteSound(){
        StartCoroutine(AudioManager.VolumeRoutine(AudioSource, 1f, 0f, 2f));
    }

    private void RandomSound(){
        AudioSource = AudioManager.GetRandomMainMenuMusic();
        // var rand = Random.Range(0, AudioManager.MainMenuMusics.Count);

        // var randomSoundSo = CreateAudioSource(AudioManager.MainMenuMusics[rand]);
        // randomSoundSo.volume = 0;
        // AudioSource = randomSoundSo;
    }

    // private IEnumerator VolumeRoutine(AudioSource audioSource, float start, float end, float duration){
    //     if(!audioSource.isPlaying){
    //         audioSource.Play();
    //     }
        
    //     float elapsedTime = 0;
    //     do{
    //         elapsedTime += Time.deltaTime;
    //         float interpolation = Mathf.Clamp01(elapsedTime / duration);
    //         audioSource.volume = Mathf.Lerp(start, end, interpolation);
    //         yield return null;
    //     }while(elapsedTime < duration);
    // }
}
