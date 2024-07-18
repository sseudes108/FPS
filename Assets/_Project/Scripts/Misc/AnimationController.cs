using UnityEngine;

public class AnimationController : MonoBehaviour {
    public Animator Anim {get; private set;}
    public int CurrentAnimation {get; private set;}
    private void Awake() { 
        Anim = GetComponentInChildren<Animator>(); 
    }

    public void ChangeAnimation(int newAnimation){
        Anim.StopPlayback();
        Anim.Play(newAnimation);
        CurrentAnimation = newAnimation;
    }
}