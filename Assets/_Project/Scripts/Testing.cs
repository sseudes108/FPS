using TMPro;
using UnityEngine;

public class Testing : MonoBehaviour{
    public TMP_Text PlayerStateText;
    public TMP_Text PlayerIsGroundedText;

    public void UpdatePlayerState(string state){
        PlayerStateText.text = state;
    }
    public void UpdatePlayerGrounded(string grounded){
        PlayerIsGroundedText.text = $"Grounded: {grounded}";
    }
}
