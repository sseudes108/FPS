using Unity.Cinemachine;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour{
    [SerializeField] private Transform _target;

    private float _startFOV, _targetFOV;
    private float _zoomSpeed = 5f;

    private CinemachineCamera _camera;

    private void Awake() {
        _camera = GetComponent<CinemachineCamera>();
    }

    private void Start() {
        _startFOV = _camera.Lens.FieldOfView;
        _targetFOV = _startFOV;
    }

    private void LateUpdate() {
        transform.SetPositionAndRotation(_target.position, _target.rotation);

        _camera.Lens.FieldOfView = Mathf.Lerp(_camera.Lens.FieldOfView, _targetFOV, _zoomSpeed * Time.deltaTime);
    }

    public void ZoomIn(float newZoom){
        _targetFOV = newZoom;
    }

    public void ZoomOut(){
        _targetFOV = _startFOV;
    }
}