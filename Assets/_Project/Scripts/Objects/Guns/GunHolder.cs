using UnityEngine;

public class GunHolder : MonoBehaviour{
    [SerializeField] private Transform _target;
    void LateUpdate(){
        Debug.Log($"GunHolder - Position: {transform.position}, Rotation: {transform.rotation}");
        transform.position = _target.position;
        transform.rotation = _target.rotation;
    }
}