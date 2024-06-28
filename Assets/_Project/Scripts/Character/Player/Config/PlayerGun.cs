using UnityEngine;

public class PlayerGun : MonoBehaviour {
    [SerializeField] private Gun _activeGun;

    public Gun GetActiveGun(){
        return _activeGun;
    }

    public void SetActiveGun(Gun newGun){
        _activeGun = newGun;
    }
}