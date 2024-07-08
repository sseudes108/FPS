using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour {
    public VisualEffects Effects { get; private set; }
    [SerializeField] private List<Texture2D> _crossTextes = new();
    public List<Texture2D> CrossesTextures => _crossTextes;

    private void Awake() {
        Effects = GetComponent<VisualEffects>();
    }
}