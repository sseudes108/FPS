using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    private UIDocument _uIDocument;
    private VisualElement _root;
    private VisualElement _crosshair;
    [SerializeField] private Texture2D _crosshairSprite;
    [SerializeField] private Color _crosshairColor;

    private void OnEnable() {
        _uIDocument = GetComponent<UIDocument>();
        _root = _uIDocument.rootVisualElement;
        _crosshair = _root.Q("CrossHair");
    }

    private void Start() {
        CrosshairSettings();
    }

    private void CrosshairSettings(){
        _crosshair.style.backgroundImage = _crosshairSprite;
        _crosshair.style.unityBackgroundImageTintColor = _crosshairColor;
    }

}