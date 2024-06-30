using UnityEngine;
using UnityEngine.UIElements;

public class UI_Locus : MonoBehaviour {
    public VisualElement _crossHair;
    [SerializeField] private Texture2D _crosshairTexture;
    [SerializeField] private Color _crosshairColor;

    private void Start() {
        _crossHair = GameManager.Instance.UIManager.Root.Q("Cross");
        Debug.Log(_crossHair);
        _crossHair.style.backgroundImage = _crosshairTexture;
        _crossHair.style.unityBackgroundImageTintColor = _crosshairColor;
    }
}