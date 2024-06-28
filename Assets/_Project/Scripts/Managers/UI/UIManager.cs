using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    public UIDocument UIDocument {get; private set;}
    public VisualElement Root {get; private set;}

    public UI_HealthBar HealthBar;
    public UI_CrossHair CrossHair;
    
    private void OnEnable() {
        UIDocument = GetComponent<UIDocument>();
        Root = UIDocument.rootVisualElement;

    }
    
    private void Awake() {
        SetComponents();
    }

    private void SetComponents(){
        CrossHair = GetComponent<UI_CrossHair>();
        HealthBar = GetComponent<UI_HealthBar>();
    }

}