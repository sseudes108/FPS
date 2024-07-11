using UnityEngine;

public class AudioEffects : MonoBehaviour {
    [SerializeField] private AudioEventHandlerSO AudioManager;

    private void OnEnable() {
        AudioManager.OnClick.AddListener(PlayClickSound);
        AudioManager.OnShoot.AddListener(PlayShootSound);
        AudioManager.OnReload.AddListener(PlayReloadSound);
        AudioManager.OnHandleGun.AddListener(PlayHandleGunSound);
        AudioManager.OnStep.AddListener(PlayStepSound);
        AudioManager.OnCrossHairChange.AddListener(PlayCrossHairChangeSound);
    }

    private void OnDisable() {
        AudioManager.OnClick.RemoveListener(PlayClickSound);
        AudioManager.OnShoot.RemoveListener(PlayShootSound);
        AudioManager.OnReload.RemoveListener(PlayReloadSound);
        AudioManager.OnHandleGun.RemoveListener(PlayHandleGunSound);
        AudioManager.OnStep.RemoveListener(PlayStepSound);
        AudioManager.OnCrossHairChange.RemoveListener(PlayCrossHairChangeSound);
    }

    private void PlayHandleGunSound(SoundSO GunHandlingSound) { AudioManager.PlayAudio(GunHandlingSound); }
    private void PlayShootSound(SoundSO shootSound) { AudioManager.PlayAudio(shootSound); }
    private void PlayReloadSound(SoundSO reloadSound) { AudioManager.PlayAudio(reloadSound); }
    private void PlayStepSound(SoundSO stepSound) { AudioManager.PlayAudio(stepSound); }
    public void PlayCrossHairChangeSound(SoundSO CrossHairChangeSound) { AudioManager.PlayAudio(CrossHairChangeSound); }
    public void PlayClickSound(SoundSO clickSound) { AudioManager.PlayAudio(clickSound); }
}
