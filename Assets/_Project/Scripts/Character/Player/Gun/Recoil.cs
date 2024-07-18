using UnityEngine;

public class Recoil : MonoBehaviour{
    //Rotations
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;

    //Hip Fire Recoil
    [SerializeField] private float _recoilX;
    [SerializeField] private float _recoilY;
    [SerializeField] private float _recoilZ;

    //Settings
    [SerializeField] private float _snappiness;
    [SerializeField] private float _returnSpeed;

    private void Update(){
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void RecoilFire(){
        _targetRotation += new Vector3(_recoilX, Random.Range(-_recoilY, _recoilY), Random.Range(-_recoilZ, _recoilZ));
    }
}
