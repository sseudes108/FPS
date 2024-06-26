using UnityEngine;
using UnityEngine.UIElements;

public class Testing : MonoBehaviour{

    private Label stateDebug;
    private Label groundedDebug;

    private void Start() {
        stateDebug = GameManager.Instance.UIManager.Root.Q<Label>("DebugState");
        groundedDebug = GameManager.Instance.UIManager.Root.Q<Label>("DebugGrounded");
    }

    public void UpdateDebugStateLabel(string currentState){
        stateDebug.text = $"State: {currentState}";
    }

    public void UpdateDebugGroundedLabel(string grounded){
        groundedDebug.text = $"Grounded: {grounded}";
    }
}
