// using UnityEngine;

// public class UIGamePlay : MonoBehaviour {
//     private VisualElement _crosshair;
//     [SerializeField] private Texture2D _crosshairSprite;
//     [SerializeField] private Color _crosshairColor;

//     private void Start() {
//         _crosshair = GameManager.Instance.UIManager.Root.Q("CrossHair");
//         CrosshairSettings();
//     }

//     private void CrosshairSettings(){
//         _crosshair.style.backgroundImage = _crosshairSprite;
//         _crosshair.style.unityBackgroundImageTintColor = _crosshairColor;
//     }
// }