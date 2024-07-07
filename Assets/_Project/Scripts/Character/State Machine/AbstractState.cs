[System.Serializable]
public abstract class AbstractState {
    public Character Character {get; private set;}
    protected Player Player;
    protected Enemy Enemy;
    
    public abstract void Enter();
    public abstract void LogicUpdate();
    public abstract void Exit();

    public void SetCharacter(Character character){
        if(character is Player){
            Player = character as Player;
        }else if(character is Enemy){
            Enemy = character as Enemy;
        }
        Character = character;
    }
}