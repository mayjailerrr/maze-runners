using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TurnManager
{
    private int currentPlayerIndex = 0;
    private List<Player> players;
    private Context gameContext;
    private bool isPaused = false;

    private readonly Dictionary<object, List<ITemporaryEffect>> activeEffects = new();
   
    public TurnManager(List<Player> players, Context context)
    {
        this.players = players;
        gameContext = context;
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
        if (isPaused) return;

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
        if (isPaused) return;

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        gameContext.CurrentPlayer = players[currentPlayerIndex];

        Debug.Log($"Turn changed to player {currentPlayerIndex + 1}.");

        StartTurn();
    }


    public void ApplyTemporaryEffect(ITemporaryEffect effect)
    {
        if (!activeEffects.ContainsKey(effect.Target))
        {
            activeEffects[effect.Target] = new List<ITemporaryEffect>();
        }

        activeEffects[effect.Target].Add(effect);
        effect.Apply();
    }

    public void UpdateTemporaryEffects()
    {
        foreach (var (obj, effects) in activeEffects)
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
        if (isPaused)
        {
            Debug.LogWarning("Cannot perform actions while the game is paused.");
            return false;
        }

        return true;
    }

    public void PauseTurns(bool pause) => isPaused = pause;
    
    public bool PerformAction(ActionType actionType, Piece piece, Board board, int targetX = 0, int targetY = 0)
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
