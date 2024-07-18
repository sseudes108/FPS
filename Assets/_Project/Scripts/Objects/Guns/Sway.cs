using UnityEngine;

public class Sway : MonoBehaviour{
    [field:SerializeField] public GameManagerSO GameManager { get; private set;}

    [SerializeField] private float intensity;
    [SerializeField] private float smooth;
    private Quaternion _originRotation;
    private Gun _gun;
    private float _defaultIntensity;
    
    private void Awake(){
        _gun = GetComponent<Gun>();
        _defaultIntensity = intensity;
    }

    private void Start() {
        _originRotation = transform.localRotation;
    }

    private void Update(){
        UpdateSway();
    }

    private void UpdateSway(){
        float mouseX = GameManager.RotationInput.x;
        float mouseY = GameManager.RotationInput.y * -1;

        if(_gun.IsAiming || _gun.isShooting){
            intensity  /= 3;
        }else{
            intensity = _defaultIntensity;
        }

        var adjustmentX = Quaternion.AngleAxis(-1 * intensity * mouseX, Vector3.up);
        var adjustmentY = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
        Quaternion _targetRotation = _originRotation * adjustmentX * adjustmentY;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, _targetRotation, smooth * Time.deltaTime);
    }
}
