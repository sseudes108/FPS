using UnityEngine;

public class SFXManager : MonoBehaviour {
    [SerializeField] private SoundSO _footStep;

    private void OnEnable() {
        PlayerGun.OnShootFired += PlayerGun_OnShootFired;
        PlayerGun.OnGunReload += PlayerGun_OnGunReload;
        PlayerMove.OnStep += PlayerMove_OnStep;
    }

    private void OnDisable() {
        PlayerGun.OnShootFired -= PlayerGun_OnShootFired;
        PlayerGun.OnGunReload -= PlayerGun_OnGunReload;
        PlayerMove.OnStep -= PlayerMove_OnStep;
    }

    private void PlayerGun_OnShootFired(SoundSO shootSound){
        GameManager.Instance.AudioManager.SoundToPlay(shootSound);
    }
    
    private void PlayerGun_OnGunReload(SoundSO reloadSound){
        GameManager.Instance.AudioManager.SoundToPlay(reloadSound);
    }

    private void PlayerMove_OnStep(){
        GameManager.Instance.AudioManager.SoundToPlay(_footStep);
    }
}