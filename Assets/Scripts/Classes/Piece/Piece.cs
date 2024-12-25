
using System;
using MazeRunners;
using UnityEngine;
using System.Linq;

public class Piece
{

    public string Name { get; protected set; }
    public int Speed { get; set; }
    public int Cooldown { get; set; }
    private int currentCooldown = 0;
    public (int x, int y) Position { get; set; }
    
    public bool HasUsedAbility { get; private set; }
    public IAbility Ability { get; private set; }

    private int movesRemaining;
    public bool HasMoved { get; private set; }

    public Piece (string name, int speed, int cooldown, IAbility ability)
    {
        Name = name;
        Speed = speed;
        Cooldown = cooldown;
        Ability = ability;
        ResetTurn();
    }

    public bool CanUseAbility => currentCooldown == 0;

    public bool UseAbility(Context context)
    {
        if (!CanUseAbility)
        {
            Debug.Log($"{Name} ability is on cooldown for {currentCooldown} more turn(s).");
            return false;
        }

        if (Ability?.Execute(context) == true)
        {
            Debug.Log($"Piece {Name} used its ability!");
            ActivateAbility();
            HasUsedAbility = true;

            ResetTurn();
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
        movesRemaining = Speed;
        HasUsedAbility = false;
    }

    public bool CanMoveMoreTiles() => movesRemaining > 0;

  
    public void Move(int newX, int newY)
    {
        Position = (newX, newY);
        movesRemaining--;
        HasMoved = true;
        
        Debug.Log($"{Name} moved to ({newX}, {newY})");
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


