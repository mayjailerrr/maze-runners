
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
    public (int x, int y)? PreviousPosition { get; private set; }
    public (int x, int y)? InitialPosition { get; set; }
    
    public event Action OnHealthChanged;
    private int health = 3;
    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Max(0, value);
            OnHealthChanged?.Invoke();
        }
    }
    
    private bool isShielded;
    public bool IsShielded
    {
        get => isShielded;
        set
        {
            isShielded = value;
        }
    }

    private bool isInvisible;
    public bool IsInvisible
    {
        get => isInvisible;
        set => isInvisible = value;
    }

    private bool _abilitiesBlocked = false;
    public bool AbilitiesBlocked
    {
        get => _abilitiesBlocked;
        set
        {
            _abilitiesBlocked = value;
            Debug.Log($"{Name} abilities are now {(value ? "blocked" : "unblocked")}.");
            NotifyAbilityStateChanged();
        }
    }

    public event Action OnAbilityStateChanged;
    public bool CanUseAbility => !_abilitiesBlocked && currentCooldown == 0;
    public bool HasUsedAbility { get; private set; }
    public IAbility Ability { get; set; }
   
    public PieceView View { get; set; }

    private int movesRemaining;
    public int MovesRemaining
    {
        get => movesRemaining;
        private set
        {
            movesRemaining = value;
        }
    }
    public bool HasMoved { get; private set; }
    public event Action OnMovesChanged;

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
        if (currentCooldown > 0)
        {
            currentCooldown--;
            NotifyAbilityStateChanged();
        }
    }

    private void NotifyAbilityStateChanged()
    {
        OnAbilityStateChanged?.Invoke();
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
        NotifyMovesChanged();
        
        if (!IsInvisible) Debug.Log($"{Name} moved to ({newX}, {newY})");
    }

    private void NotifyMovesChanged()
    {
        OnMovesChanged?.Invoke();
    }

    protected void ActivateAbility()
    {
        currentCooldown = Cooldown;
        NotifyAbilityStateChanged();
    }

    public Piece Clone()
    {
        Piece piece = new Piece(Name + "_Clone", Speed, Cooldown, null);
        piece.Position = Position;
        piece.PreviousPosition = PreviousPosition;
        piece.Health = Health;

        return piece;
    }
    
    public void TakeDamage(int damage, Context context)
    {
        Health -= damage;
        Debug.Log($"{Name} took {damage} damage. Health is now {Health}.");

        if (Health <= 0)
        {
            Health = 3;
            Debug.Log($"{Name} health reached 0. Resetting to initial position.");
            ResetPosition();
            context.BoardView.ResetPositionWithFeedback(this);
        }
    }
}


