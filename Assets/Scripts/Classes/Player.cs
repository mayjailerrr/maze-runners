using System.Collections.Generic;
using MazeRunners;
using UnityEngine;
public class Player
{
    public int ID { get; private set; }
    public List<Piece> Pieces { get; private set; }

    public Player (int id)
    {
        ID = id;
        Pieces = new List<Piece>();
    }

    public void AssignPieces(List<Piece> pieces)
    {
        if (pieces == null || pieces.Count == 0)
        {
            Debug.LogError("Cannot assign an empty or null piece list of pieces.");
            return;
        }

        foreach (var piece in pieces)
        {
            if (piece != null)
            {
                Pieces.Add(piece);
            }
        }

        Debug.Log($"Assigned {pieces.Count} pieces to Player {ID}.");
    }

    public Piece ChoosePiece(int index)
    {
        if (index >= 0 && index < Pieces.Count)
        {
            return Pieces[index];
        }

        Debug.LogWarning($"Invalid piece index {index} chosen by Player {ID}.");
        return null;
    }

    public void MovePiece(Piece piece, int newX, int newY, Board board)
    {
        if (piece == null)
        {
            Debug.LogError("Cannot move a null piece.");
            return;
        }

        if (!Pieces.Contains(piece))
        {
            Debug.LogError($"Piece {piece.Name} does not belong to Player {ID}.");
            return;
        }

        if (board.IsValidMove(piece, newX, newY))
        {
            piece.Move(newX, newY);
            Debug.Log($"Player {ID} moved piece {piece.Name} to ({newX}, {newY}).");
        }
        else
        {
            Debug.LogWarning($"Invalid move for piece {piece.Name} to ({newX}, {newY}).");
        }
    }

    public void UsePieceAbility(Piece piece)
    {
        if (piece == null)
        {
            Debug.LogError("Cannot use ability on a null piece.");
            return;
        }

        if (!Pieces.Contains(piece))
        {
            Debug.LogError($"Piece {piece.Name} does not belong to Player {ID}.");
            return;
        }

        if (piece.CanUseAbility())
        {
            piece.UseAbility();
            Debug.Log($"Player {ID} used {piece.Name}'s ability.");
        }
        else
        {
            Debug.LogWarning($"{piece.Name}'s ability is still on cooldown for Player {ID}.");
        }
    }

}

