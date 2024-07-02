using UnityEngine;

[CreateAssetMenu(fileName = "GunSO", menuName = "FPS/GunSO", order = 0)]
public class GunSO : ScriptableObject {

    [Header("Settings")]
    public WeaponTypes WeaponType;
    public int Magazine;
    public float ReloadTime;

    [Header("Fire")]
    public float Firerate;
    public bool CanAutoFire;
    public int DamageValue;
    public float RecoilForce;

    [Header("Bullet")]
    public Material BulletMaterial;
    // public Bullet BulletPrefab;

    [Header("Aim")]
    public float ZoomAmount;
    public float AimSpeed;

    [Header("Sounds")]
    public SoundSO ShootSound;
    public SoundSO ReloadSound;
    public SoundSO HandlingSound;
}

public enum WeaponTypes{
        Pistol = 0,
        Sniper = 1,
        AssaultRifle = 2,
        RocketLauncher = 3
    }