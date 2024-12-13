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
    private Context gameContext;

    public TurnManager(List<Player> players, Context context)
    {
        if (players == null || players.Count < 2)
        {
            throw new System.ArgumentException("TurnManager requires at least two players.");
        }

        this.players = players;
        this.gameContext = context;
        StartTurn();
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

        StartTurn();
    }

    public void StartTurn()
    {
        Player currentPlayer = GetCurrentPlayer();

        gameContext.ResetContextForNewTurn(currentPlayer);

        foreach (var piece in currentPlayer.Pieces)
        {
                piece.ResetTurn();
                piece.ReduceCooldown();
        }

        Debug.Log($"Player {currentPlayer.ID + 1}, it's your turn!");


        //to-do: add logic for preparing the player, remark the pieces or add information
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

                if (!piece.CanMoveMoreTiles())
                {
                    Debug.LogWarning($"{piece.Name} has no moves left this turn.");
                    return false;
                }

                if (currentPlayer.MovePiece(piece, targetX, targetY, board))
                {
                    if (AllPiecesMoved(currentPlayer))
                    {
                        NextTurn();
                    }
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

     private bool AllPiecesMoved(Player player)
    {
        foreach (var piece in player.Pieces)
        {
            if (piece.CanMoveMoreTiles())
            {
                return false;
            }
        }
        return true;
    }
}
