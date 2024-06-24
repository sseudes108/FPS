using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    public UIDocument UIDocument {get; private set;}
    public VisualElement Root {get; private set;}

    private VisualElement _crosshair;
    [SerializeField] private Texture2D _crosshairSprite;
    [SerializeField] private Color _crosshairColor;

    private Label stateDebug;
    private Label groundedDebug;

    private void OnEnable() {
        UIDocument = GetComponent<UIDocument>();
        Root = UIDocument.rootVisualElement;
        _crosshair = Root.Q("CrossHair");
        
        stateDebug = Root.Q<Label>("DebugState");
        groundedDebug = Root.Q<Label>("DebugGrounded");
    }

    private void Start() {
        CrosshairSettings();
    }

    private void CrosshairSettings(){
        _crosshair.style.backgroundImage = _crosshairSprite;
        _crosshair.style.unityBackgroundImageTintColor = _crosshairColor;
    }

    public void UpdateDebugStateLabel(string currentState){
        stateDebug.text = $"State: {currentState}";
    }

    public void UpdateDebugGroundedLabel(string grounded){
        groundedDebug.text = $"Grounded: {grounded}";;
    }

}