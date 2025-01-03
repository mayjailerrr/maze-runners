using MazeRunners;
using UnityEngine;

public class CloneAbility : IAbility
{
    public string Description => "Clones the piece and creates a new piece with the same abilities.";

    public bool Execute(Context context)
    {
        if (context.CurrentPiece == null)
        {
            Debug.LogError("No piece selected to clone.");
            return false;
        }

        Piece clonedPiece = context.CurrentPiece.Clone();

        if (clonedPiece == null)
        {
            Debug.LogError("Failed to clone the piece.");
            return false;
        }

        context.CurrentPlayer.AddPiece(clonedPiece);

        Debug.Log($"Piece {context.CurrentPiece.Name} cloned successfully. New piece: {clonedPiece.Name}");

        return true;
    }
}
