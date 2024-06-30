using UnityEngine;

public class SFXManager : MonoBehaviour {
    [SerializeField] private SoundSO _footStep;

    private void OnEnable() {
        PlayerGun.OnShootFired += PlayerGun_OnShootFired;
        PlayerMove.OnStep += PlayerMove_OnStep;
    }

    private void OnDisable() {
        PlayerGun.OnShootFired -= PlayerGun_OnShootFired;
        PlayerMove.OnStep -= PlayerMove_OnStep;
    }

    private void PlayerGun_OnShootFired(SoundSO shootSound){
        GameManager.Instance.AudioManager.SoundToPlay(shootSound);
    }

    private void PlayerMove_OnStep(){
        GameManager.Instance.AudioManager.SoundToPlay(_footStep);
    }
}