using System;
using UnityEngine;

[Serializable]
public abstract class AbstractState {
    public Character Character {get; private set;}
    protected Player Player;
    // protected Enemy Enemy;
    protected Turret Turret;
    
    public abstract void Enter();
    public abstract void LogicUpdate();
    public abstract void Exit();

    public void SetCharacter(Character character){
        switch(character){
            case Player playerCharacter:
                Player = playerCharacter;
                Character = Player;
            break;
            case Turret turretCharacter:
                Turret = turretCharacter;
                Character = Turret;
            break;
        }
        Character = character;
    }
}