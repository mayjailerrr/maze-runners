using UnityEngine;
using MazeRunners;
using System.Collections.Generic;

public enum ActionType
{
    Move,
    UseAbility
}

public class TurnManager
{
    private int currentPlayerIndex = 0;
    private List<Player> players;

    public TurnManager(List<Player> players)
    {
        if (players == null || players.Count < 2)
        {
            throw new System.ArgumentException("TurnManager requires at least two players.");
        }

        this.players = players;
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        Debug.Log($"It's now Player {players[currentPlayerIndex].ID}'s turn!");
    }

    public bool PerformAction(ActionType actionType, Piece piece, Board board, int targetX = 0, int targetY = 0, Context context = null)
    {
        var currentPlayer = GetCurrentPlayer();

        switch (actionType)
        {
            case ActionType.Move:
                if (board == null)
                {
                    Debug.LogError("Board is null. Cannot perform Move action.");
                    return false;
                }

                if (currentPlayer.MovePiece(piece, targetX, targetY, board))
                {
                    NextTurn();
                    return true;
                }
                return false;

            case ActionType.UseAbility:
                if (context == null)
                {
                    Debug.LogError("Context is required to use an ability.");
                    return false;
                }

                if (currentPlayer.UsePieceAbility(piece, context))
                {
                    NextTurn();
                    return true;
                }
                return false;

            default:
                Debug.LogWarning($"Invalid action type: {actionType}");
                return false;
        }
    }
}
