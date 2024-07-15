using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimationsSO", menuName = "FPS/Animation/Player Animations", order = 0)]
public class PlayerAnimationsSO : AnimationsSO {
    public readonly int IDLE = Animator.StringToHash("Player_Idle");
    public readonly int WALK = Animator.StringToHash("Player_Walk");
    public readonly int RUN = Animator.StringToHash("Player_Run");
}