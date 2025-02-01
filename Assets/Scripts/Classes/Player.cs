
using System.Collections.Generic;
using UnityEngine;


public class Player
{
    public int ID { get; }
    public string Name { get; private set; }
    public List<Piece> Pieces => _pieces; 
    private List<Piece> _pieces;

    private bool abilityUsed;

    public List<Collectible> AssignedObjects { get; private set; }
    public HashSet<Collectible> CollectedObjects { get; private set; }

    public int Moves { get; set; } = 0;
    public int TrapsTriggered { get; set; } = 0;
    public int AbilitiesUsed { get; set; } = 0;

    public void RecordMove() => Moves++;
    public void RecordTrap() => TrapsTriggered++;
    public void RecordAbilityUse() => AbilitiesUsed++;
  

    public Player(int id, string name = "Player")
    {
        ID = id;
        Name = name;
        _pieces = new List<Piece>();
        CollectedObjects = new HashSet<Collectible>();
    }

    public void AssignPieces(IEnumerable<Piece> pieces)
    {
        if (pieces == null)
        {
            Debug.LogError($"Player {ID}: Cannot assign a null list of pieces.");
            return;
        }

        _pieces.Clear();
        foreach (var piece in pieces)
        {
            if (piece != null) _pieces.Add(piece);
        }

        Debug.Log($"Player {ID}: Assigned {_pieces.Count} pieces.");
    }

    public void AddPiece(Piece piece)
    {
        if (piece == null)
        {
            Debug.LogError($"Player {ID}: Cannot add a null piece.");
            return;
        }

        _pieces.Add(piece);
    }

    public bool MovePiece(Piece piece, int newX, int newY, Board board)
    {
        if (!ValidatePieceOwnership(piece))
            return false;
        
        piece.Move(newX, newY, board);
        this.RecordMove();
        
        return true;
        
    }

    public bool UsePieceAbility(Piece piece, Context context)
    {
        if (!ValidatePieceOwnership(piece))
            return false;

        if (piece.CanUseAbility)
        {
            abilityUsed = true;
            return piece.UseAbility(context);
        }

        else 
        {
            Debug.LogWarning($"Player {ID}: Ability of piece {piece.Name} is on cooldown.");
            return false;
        }
    }

    public bool HasUsedAbility()
    {
        return abilityUsed;
    }

    private bool ValidatePieceOwnership(Piece piece)
    {
        if (piece == null)
        {
            Debug.LogError($"Player {ID}: Cannot perform action on a null piece.");
            return false;
        }

        if (!_pieces.Contains(piece))
        {
            Debug.LogError($"Player {ID}: The piece {piece.Name} does not belong to this player.");
            return false;
        }

        return true;
    }

    public void AssignObjects(List<Collectible> objects)
    {
        AssignedObjects = objects;
    }

    public bool HasCollectedAllObjects()
    {
        return CollectedObjects.Count == AssignedObjects.Count;
    }

    public bool CollectObject(Collectible collectible)
    {
        if (AssignedObjects.Contains(collectible))
        {
            CollectedObjects.Add(collectible);
            Debug.Log($"Player {ID + 1} collected {collectible.Name}!");

            return true;
        }

        return false;
    }

    public void ResetTurn()
    {
        abilityUsed = false;

        foreach (var piece in _pieces)
        {
            piece.ResetTurn();
        }
    }

}
