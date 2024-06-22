public abstract class AbstractState {
    public Character Character;
    protected Player Player;
    
    public abstract void Enter();
    public abstract void LogicUpdate();
    public abstract void Exit();

    public void SetCharacter(Character character){
        if(character is Player){
            Player = character as Player;
            Character = character;
        }
    }
}