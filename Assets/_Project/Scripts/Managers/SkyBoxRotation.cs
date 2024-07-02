using UnityEngine;

public class SkyBoxRotation : MonoBehaviour {
    public Material _skyboxMaterial;
    public float Speed = 1f;

    private void Awake() {
        _skyboxMaterial = RenderSettings.skybox;
    }

    private void Update() {
        float rotation = Time.deltaTime * Speed;

        _skyboxMaterial.SetFloat("_Rotation", rotation);
    }
}