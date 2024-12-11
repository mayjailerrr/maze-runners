using MazeRunners;
using System.Collections.Generic;
using UnityEngine;
public class Player
{
    public int ID { get; }
    public string Name { get; private set; }
    public IReadOnlyList<Piece> Pieces => _pieces.AsReadOnly(); 
    public int ExitsReached { get; private set; }

    private List<Piece> _pieces;

    public Player(int id, string name = "Player")
    {
        ID = id;
        Name = name;
        _pieces = new List<Piece>();
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

    public Piece ChoosePiece(int index)
    {
        if (index < 0 || index >= _pieces.Count)
        {
            Debug.LogWarning($"Player {ID}: Invalid piece index {index}.");
            return null;
        }

        return _pieces[index];
    }

    public bool MovePiece(Piece piece, int newX, int newY, Board board)
    {
        if (!ValidatePieceOwnership(piece))
            return false;

        if (board.IsValidMove(piece, newX, newY))
        {
            piece.Move(newX, newY);
            Debug.Log($"Player {ID}: Moved piece {piece.Name} to ({newX}, {newY}).");
            return true;
        }

        Debug.LogWarning($"Player {ID}: Invalid move for piece {piece.Name} to ({newX}, {newY}).");
        return false;
    }

    public bool UsePieceAbility(Piece piece, Context context)
    {
        if (!ValidatePieceOwnership(piece))
            return false;

        if (piece.CanUseAbility)
        {
            return piece.UseAbility(context);
        }

        Debug.LogWarning($"Player {ID}: Ability of piece {piece.Name} is on cooldown.");
        return false;
    }

    public void ReachExit()
    {
        ExitsReached++;
        Debug.Log($"Player {ID}: Reached an exit! Total exits: {ExitsReached}");

        // GameManager/GameSession
        if (ExitsReached >= 3)
        {
            Debug.Log($"Player {ID}: Wins the game!");
        }
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

}
