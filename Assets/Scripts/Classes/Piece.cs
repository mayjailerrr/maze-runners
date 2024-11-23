

using MazeRunners;
using UnityEngine;
public class Piece
{

    public string Name { get; protected set; }
    public int Speed { get; set; }
    public int Cooldown { get; set; }
    public int CurrentCooldown { get; set; }
    public (int x, int y) Position { get; set; }
  //  public Movie Movie { get; private set; }

    private int tilesMovedThisTurn;

    public Piece (string name, int speed, int cooldown)
    {
        Name = name;
        Speed = speed;
        Cooldown = cooldown;
        CurrentCooldown = 0;
        //Movie = movie;
    }

    public virtual void UseAbility()
    {
        Debug.Log($"{Name} used their ability!");
    }

     public void ResetTurn()
    {
        tilesMovedThisTurn = 0;
    }

    public bool CanMoveMoreTiles() => tilesMovedThisTurn < Speed;

  
    public void Move(int newX, int newY)
    {
        Position = (newX, newY);
        tilesMovedThisTurn++;
        Debug.Log($"{Name} moved to ({newX}, {newY})");

    }

    public void UpdateCooldown()
    {
        if (CurrentCooldown > 0) CurrentCooldown--;
    }

    public bool CanUseAbility()
    {
        return CurrentCooldown == 0;
    }

    protected void ActivateAbility()
    {
        CurrentCooldown = Cooldown;
    }

}


