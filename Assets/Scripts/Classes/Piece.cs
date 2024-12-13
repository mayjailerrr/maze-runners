
using System;
using MazeRunners;
using UnityEngine;

public class Piece
{

    public string Name { get; protected set; }
    public int Speed { get; set; }
    public int Cooldown { get; set; }
    private int currentCooldown = 0;
    public (int x, int y) Position { get; set; }

    public Func<Context, bool> Ability { get; private set; }	

    private int tilesMovedThisTurn;

    public bool HasMoved { get; private set; }

    public Piece (string name, int speed, int cooldown, Func<Context, bool> ability)
    {
        Name = name;
        Speed = speed;
        Cooldown = cooldown;
        Ability = ability;
    }

    public bool CanUseAbility => currentCooldown == 0;

    public bool UseAbility(Context context)
    {
        if (!CanUseAbility)
        {
            Debug.Log($"{Name} ability is on cooldown for {currentCooldown} more turn(s).");
            return false;
        }

        if (Ability?.Invoke(context) == true)
        {
            Debug.Log($"{Name} used ability.");
            ActivateAbility();
            return true;
        }

        Debug.LogWarning($"{Name} ability failed.");
        return false;
       
    }

    public void ReduceCooldown()
    {
        if (currentCooldown > 0) currentCooldown--;
    }

     public void ResetTurn()
    {
        tilesMovedThisTurn = 0;
        HasMoved = false;
    }

    public bool CanMoveMoreTiles() => tilesMovedThisTurn < Speed;

  
    public void Move(int newX, int newY)
    {
        Position = (newX, newY);
        tilesMovedThisTurn++;
        Debug.Log($"{Name} moved to ({newX}, {newY})");

        HasMoved = true;

    }

    public void UpdateCooldown()
    {
        if (currentCooldown > 0) currentCooldown--;
    }

    protected void ActivateAbility()
    {
        currentCooldown = Cooldown;
    }

    

}


