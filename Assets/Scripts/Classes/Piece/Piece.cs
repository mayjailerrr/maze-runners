
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
    public (int x, int y)? PreviousPosition { get; private set; }
    public (int x, int y)? InitialPosition { get; set; }
    
    public int Health { get; set; } = 3;

    private bool isShielded;
    public bool IsShielded
    {
        get => isShielded;
        set
        {
            isShielded = value;
            if (isShielded) View.ShowShield();
            else View.HideShield();
        }
    }
    private bool _abilitiesBlocked = false;
    public bool AbilitiesBlocked
    {
        get => _abilitiesBlocked;
        set
        {
            _abilitiesBlocked = value;
            Debug.Log($"{Name} abilities are now {(value ? "blocked" : "unblocked")}.");
        }
    }
    public bool CanUseAbility => !_abilitiesBlocked && currentCooldown == 0;
   
    public bool HasUsedAbility { get; private set; }
    public IAbility Ability { get; set; }
    public PieceView View { get; set; }

    private int movesRemaining;
    public bool HasMoved { get; private set; }

    public Piece (string name, int speed, int cooldown, IAbility ability)
    {
        Name = name;
        Speed = speed;
        Cooldown = cooldown;
        Ability = ability;
        Position = (0, 0);
        PreviousPosition = null;
        InitialPosition = null;
        ResetTurn();
    }

    public void UpdatePosition((int x, int y) newPosition)
    {
        PreviousPosition = Position; 
        Position = newPosition; 
    }

    public void ResetPosition()
    {
        Position = InitialPosition.Value;
    }

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

  
    public void Move(int newX, int newY, Board board)
    {
        if (board == null) throw new ArgumentNullException(nameof(board));

        Tile currentTile = board.TileGrid[Position.x, Position.y];
        Tile targetTile = board.TileGrid[newX, newY];

        currentTile.OccupyingPiece = null;
        targetTile.OccupyingPiece = this;

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

    public string GetDirection(Vector2 newPosition)
    {
        Vector2 current = new Vector2(Position.Item1, Position.Item2);
        Vector2 direction = newPosition - current;

        if (direction == Vector2.up) return "Right";
        if (direction == Vector2.down) return "Left";
        if (direction == Vector2.left) return "Up";
        if (direction == Vector2.right) return "Down";

        return "Idle";
    }

    public Piece Clone()
    {
        Piece piece = new Piece(Name + "_Clone", Speed, Cooldown, this.Ability);
        piece.Position = Position;
        piece.PreviousPosition = PreviousPosition;
        piece.Health = Health;
        return piece;
    }
    
    public void TakeDamage(int damage)
    {
        if (IsShielded)
        {
            Debug.Log($"{Name} is shielded and takes no damage!");
            return;
        } 

        Health -= damage;
        Debug.Log($"{Name} took {damage} damage. Health is now {Health}.");

        if (Health <= 0)
        {
            Health = 3;
            Debug.Log($"{Name} health reached 0. Resetting to initial position.");
            ResetPosition();
        }
    }
}


