using UnityEngine;

//VFXHelper is used for all particle effects to create and manager an object pool of the given effect
public class VFXHelper :MonoBehaviour {

    //Play the particle effect. Is called by the VFXManager at GET from pool
    public void Play(Material material){
        GetComponent<Renderer>().material = material;
        GetComponent<ParticleSystem>().Play();
    }
}