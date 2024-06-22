using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    [SerializeField] private Transform CameraTransform;

    [Range(0.1f,1f)]
    [SerializeField] private float _sensitivity;

    [Range(0.1f,0.3f)]
    [SerializeField] private float clamp;
    public float Sensitivity => _sensitivity;

    public void UpdateRotationCamera(float mouseInput){
        mouseInput = Mathf.Clamp(mouseInput, -clamp, clamp) * -1f;
        transform.localRotation = Quaternion.Euler(CameraTransform.rotation.eulerAngles + new Vector3(mouseInput, 0f, 0f));
    }
}