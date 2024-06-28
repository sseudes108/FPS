using Unity.Cinemachine;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour{
    [SerializeField] private Transform _target;
    [SerializeField] private CinemachineImpulseListener _impulse;

    private void LateUpdate() {
        transform.SetPositionAndRotation(_target.position, _target.rotation);
    }
}