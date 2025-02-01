
using UnityEngine;
using System.Collections;

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
        
        CreateVisualClone(clonedPiece, context);
        context.CurrentPlayer.RecordAbilityUse();
        context.CurrentPiece.View.PlayAbilitySound();

        Debug.Log($"Piece {context.CurrentPiece.Name} cloned successfully. New piece: {clonedPiece.Name}");

        return true;
    }

    private void CreateVisualClone(Piece clonedPiece, Context context)
    {
        if (context.BoardView == null)
        {
            Debug.LogError("BoardView not assigned. Cannot create visual clone.");
            return;
        }
        
        var originalPosition = context.CurrentPiece.Position;
        var clonePosition = (originalPosition.x + 1, originalPosition.y);

        if (!context.Board.IsWithinBounds(clonePosition.Item1, clonePosition.Item2) ||
            context.Board.GetTileAtPosition(clonePosition.Item1, clonePosition.Item2)?.IsOccupied == true)
        {
            Debug.LogWarning("Cannot place visual clone: no valid position available.");
            return;
        }

        var tileGO = context.BoardView.GetTileObject(clonePosition.Item1, clonePosition.Item2);
        if (tileGO == null)
        {
            Debug.LogError("Tile GameObject not found.");
            return;
        }

        var cloneObject = Object.Instantiate(context.CurrentPiece.View.gameObject, tileGO.transform);

        cloneObject.transform.localPosition = Vector3.zero;
        cloneObject.transform.localScale = Vector3.zero;

        context.BoardView.StartCoroutine(ScaleOverTime(cloneObject, Vector3.one, 0.5f));

        clonedPiece.View = cloneObject.GetComponent<PieceView>();
        clonedPiece.UpdatePosition(clonePosition);
    }

    private IEnumerator ScaleOverTime(GameObject target, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = target.transform.localScale;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            target.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        target.transform.localScale = targetScale;
    }
}
