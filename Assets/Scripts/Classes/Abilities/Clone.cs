
using UnityEngine;
using System.Collections;

public class CloneAbility : IAbility
{
    public string Description => "Clones the piece and creates a new piece with the same abilities.";

    public bool Execute(Context context)
    {
        Piece clonedPiece = context.CurrentPiece.Clone();
        Piece currentPiece = context.CurrentPiece;

        context.CurrentPlayer.AddPiece(clonedPiece);
        
        CreateVisualClone(clonedPiece, context);

        context.CurrentPlayer.RecordAbilityUse();
        currentPiece.View.PlayAbilityEffect(new Color(0f, 0f, 0f, 0.8f));
        GameEvents.TriggerCloneUsed();

        Debug.Log($"Piece {context.CurrentPiece.Name} cloned successfully. New piece: {clonedPiece.Name}");

        return true;
    }

    private void CreateVisualClone(Piece clonedPiece, Context context)
    {    
        var originalPosition = context.CurrentPiece.Position;
        var clonePosition = context.Board.FindNearestFreeTile(originalPosition);

        if (clonePosition == null)
        {
            Debug.LogWarning("No valid position available for visual clone.");
            return;
        }

        var tile = context.Board.GetTileAtPosition(clonePosition.Value.x, clonePosition.Value.y);
        tile.OccupyingPiece = clonedPiece; 

        var tileGO = context.BoardView.GetTileObject(clonePosition.Value.x, clonePosition.Value.y);

        var cloneObject = Object.Instantiate(context.CurrentPiece.View.gameObject, tileGO.transform);
        cloneObject.transform.localPosition = Vector3.zero;
        cloneObject.transform.localScale = Vector3.zero;

        context.BoardView.StartCoroutine(ScaleOverTime(cloneObject, Vector3.one, 0.5f));

        clonedPiece.View = cloneObject.GetComponent<PieceView>();
        clonedPiece.UpdatePosition(clonePosition.Value);

        if (!context.BoardView.activeMovements.ContainsKey(clonedPiece))
        {
            context.BoardView.activeMovements[clonedPiece] = null;
        }

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
