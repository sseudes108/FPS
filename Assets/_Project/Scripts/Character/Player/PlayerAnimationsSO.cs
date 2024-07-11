using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimationsSO", menuName = "FPS/PlayerAnimations", order = 0)]
public class PlayerAnimationsSO : ScriptableObject {
    public readonly int IDLE = Animator.StringToHash("Player_Idle");
    public readonly int WALK = Animator.StringToHash("Player_Walk");
    public readonly int RUN = Animator.StringToHash("Player_Run");
}