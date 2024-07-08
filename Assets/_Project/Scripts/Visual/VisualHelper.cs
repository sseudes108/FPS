using UnityEngine;

//VisualHelper is used for all particle effects to create and manager an object pool of the given effect
public class VisualHelper :MonoBehaviour {

    //Play the particle effect. Is called by the EffectManager at GET from pool
    public void Play(Material material){
        GetComponent<Renderer>().material = material;
        GetComponent<ParticleSystem>().Play();
    }
}