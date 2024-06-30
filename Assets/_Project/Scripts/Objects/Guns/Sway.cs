using UnityEngine;

public class Sway : MonoBehaviour{
    public float intensity;
    public float smooth;

    private Quaternion _originRotation;

    private void Start() {
        _originRotation = transform.localRotation;
    }

    private void Update(){
        UpdatSway();
    }

    private void UpdatSway(){
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        var adjustmentX = Quaternion.AngleAxis(-1 * intensity * mouseX, Vector3.up);
        var adjustmentY = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
        Quaternion _targetRotation = _originRotation * adjustmentX * adjustmentY;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, _targetRotation, smooth * Time.deltaTime);
    }
}
