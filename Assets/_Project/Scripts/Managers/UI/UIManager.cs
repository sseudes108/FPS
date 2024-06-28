using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    public UIDocument UIDocument {get; private set;}
    public VisualElement Root {get; private set;}

    private void OnEnable() {
        UIDocument = GetComponent<UIDocument>();
        Root = UIDocument.rootVisualElement;
    }
}