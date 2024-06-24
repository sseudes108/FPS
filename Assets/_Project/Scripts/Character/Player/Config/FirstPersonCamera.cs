using UnityEngine;

public class FirstPersonCamera : MonoBehaviour{
    [SerializeField] private Transform _target;

    private void LateUpdate() {
        transform.SetPositionAndRotation(_target.position, _target.rotation);
    }
}