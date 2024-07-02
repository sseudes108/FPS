using UnityEngine;
using UnityEngine.UIElements;

public class UI_CrossHair : MonoBehaviour {

    private VisualElement _crosshair;
    [SerializeField] private Texture2D _crosshairTexture;
    [SerializeField] private Color _crosshairColor;

    private void Start() {
        _crosshair = GameManager.Instance.UIManager.Root.Q("CrossHair");
        CrosshairSettings();
    }

    private void CrosshairSettings(){
        _crosshair.style.backgroundImage = _crosshairTexture;
        _crosshair.style.unityBackgroundImageTintColor = _crosshairColor;
    }
}