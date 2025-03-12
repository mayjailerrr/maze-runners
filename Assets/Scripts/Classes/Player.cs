
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int ID { get; }
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

    public bool IsBlinded { get; set; } = false;
  

    public Player(int id)
    {
        ID = id;
        _pieces = new List<Piece>();
        CollectedObjects = new HashSet<Collectible>();
    }

    public void AssignPieces(IEnumerable<Piece> pieces)
    {
        _pieces.Clear();
        foreach (var piece in pieces)
        {
            if (piece != null) _pieces.Add(piece);
        }

        Debug.Log($"Player {ID}: Assigned {_pieces.Count} pieces.");
    }

    public void AddPiece(Piece piece)
    {
        if (piece == null) return;

        _pieces.Add(piece);
    }

    public bool MovePiece(Piece piece, int newX, int newY, Board board)
    {
        piece.Move(newX, newY, board);
        RecordMove();
        
        return true;
    }

    public bool UsePieceAbility(Piece piece, Context context)
    {
        if (piece.CanUseAbility)
        {
            abilityUsed = true;
            return piece.UseAbility(context);
        }

        else return false;
    }

    public bool HasUsedAbility()
    {
        return abilityUsed;
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
            Debug.Log($"Player {ID + 1} collected {collectible.Name}: {collectible.Description}.");

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
