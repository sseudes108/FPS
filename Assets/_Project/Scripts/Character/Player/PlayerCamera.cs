using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    [SerializeField] private Transform CameraTransform;

    [Range(1,10)]
    [SerializeField] private int _sensitivity;
    public float Sensitivity => _sensitivity;
    private float _rotationValue = 0f;

    public void UpdateRotationCamera(float mouseInput){
        _rotationValue -= mouseInput;
        _rotationValue = Mathf.Clamp(_rotationValue, -60f, 60f);
        CameraTransform.localRotation = Quaternion.Euler(_rotationValue, 0f, 0f);
    }
}