using UnityEngine;

[CreateAssetMenu(fileName = "GunSO", menuName = "FPS/Misc/Gun", order = 0)]
public class GunSO : ScriptableObject {

    [Header("Settings")]
    public EWeaponTypes WeaponType;
    public int Magazine;
    public float ReloadTime;

    [Header("Fire")]
    public float Firerate;
    public bool CanAutoFire;
    public int DamageValue;
    public float RecoilForce;

    [Header("Bullet")]
    public Material BulletMaterial;

    [Header("Aim")]
    public float ZoomAmount;
    public float AimSpeed;

    [Header("Sounds")]
    public SoundSO ShootSound;
    public SoundSO ReloadSound;
    public SoundSO HandlingSound;
}

public enum EWeaponTypes{
    Pistol = 0,
    Sniper = 1,
    AssaultRifle = 2,
    RocketLauncher = 3
}