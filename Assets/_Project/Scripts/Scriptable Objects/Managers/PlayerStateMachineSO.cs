using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateMachineSO", menuName = "FPS/Manager/Player", order = 0)]
public class PlayerStateMachineSO : ScriptableObject {
    public Player Player { get; private set; }

    public void SetPlayer(Player player){
        Player = player;
    }
}