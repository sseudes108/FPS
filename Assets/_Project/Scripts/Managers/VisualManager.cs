using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class VisualManager : MonoBehaviour {
    [SerializeField] private List<Texture2D> _crossTextes = new();
    public List<Texture2D> CrossesTextures => _crossTextes;
}