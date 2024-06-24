using UnityEngine;

public class AnimationController : MonoBehaviour {
    public Animator Anim;
    private void Awake() { 
        Anim = GetComponent<Animator>(); 
    }

    public void ChangeAnimation(int newAnimation){
        Anim.StopPlayback();
        Anim.Play(newAnimation);
    }
}