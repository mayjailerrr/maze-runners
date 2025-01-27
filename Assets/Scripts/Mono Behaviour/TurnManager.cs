using UnityEngine;
using MazeRunners;
using System.Collections.Generic;
using System.Linq;

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

    private readonly Dictionary<Piece, List<ITemporaryEffect>> activeEffects = new();
   
    private bool hasMovedPiece = false;
    private bool hasUsedAbility = false;
   
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

    public Player GetNextPlayer(Player currentPlayer)
    {
        int index = players.IndexOf(currentPlayer);
        return players[(index + 1) % players.Count];
    }

    public void StartTurn()
    {
        UpdateTemporaryEffects();

        Player currentPlayer = GetCurrentPlayer();

        currentPlayer.ResetTurn();
        gameContext.ResetContextForNewTurn(currentPlayer);

        foreach (var piece in currentPlayer.Pieces)
        {
            piece.ResetTurn();
            piece.ReduceCooldown();
        }

        Debug.Log($"Player {currentPlayer.ID + 1}, it's your turn!"); 
    }

    public void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        gameContext.CurrentPlayer = players[currentPlayerIndex];

        Debug.Log($"Turn changed to player {currentPlayerIndex + 1}.");

        StartTurn();
    }


    public void ApplyTemporaryEffect(ITemporaryEffect effect)
    {
        if (!activeEffects.ContainsKey(effect.TargetPiece))
        {
            activeEffects[effect.TargetPiece] = new List<ITemporaryEffect>();
        }

        activeEffects[effect.TargetPiece].Add(effect);
        effect.Apply();
    }

    public void UpdateTemporaryEffects()
    {
        foreach (var (piece, effects) in activeEffects)
        {
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                var effect = effects[i];
                effect.DecrementDuration();

                if (effect.HasExpired)
                {
                    effect.Revert();
                    effects.RemoveAt(i);
                }
            }
        }
    }

    public bool CanPerformAction(ActionType actionType)
    {
        if (actionType == ActionType.Move && hasMovedPiece)
        {
            Debug.LogWarning("Cannot move more than once per turn.");
            return false;
        }

        if (actionType == ActionType.UseAbility && hasUsedAbility)
        {
            Debug.LogWarning("Cannot use an ability more than once per turn.");
            return false;
        }

        return true;
    }
    public bool PerformAction(ActionType actionType, Piece piece, Board board, int targetX = 0, int targetY = 0, Context context = null)
    {
        if (!CanPerformAction(actionType))  return false;
       
        var currentPlayer = GetCurrentPlayer();

        switch (actionType)
        {
            case ActionType.Move:
                if (!piece.CanMoveMoreTiles())
                {
                    Debug.LogWarning($"{piece.Name} has no moves left this turn.");
                    return false;
                }

                if (currentPlayer.MovePiece(piece, targetX, targetY, board))
                {
                    CheckPieceExhausted();
                    return true;
                }
                return false;

            case ActionType.UseAbility:
                if (currentPlayer.HasUsedAbility())
                {
                    Debug.LogWarning("Cannot use more than one ability per turn.");
                    return false;
                }

                if (piece.HasUsedAbility)
                {
                    CheckPieceExhausted();
                    return true;
                }
                return false;

            default:
                Debug.LogWarning($"Invalid action type: {actionType}");
                return false;
        }
    }

    public void CheckPieceExhausted()
    {
        Player currentPlayer = GetCurrentPlayer();

        if (currentPlayer.Pieces.Any(p => !p.CanMoveMoreTiles()) && currentPlayer.HasUsedAbility())
        {
            Debug.Log($"Player {currentPlayer.ID + 1}: At least one piece is exhausted and ability used. Ending turn.");
            NextTurn();
        }
    }
}
