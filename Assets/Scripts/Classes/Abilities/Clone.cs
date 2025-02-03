
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        Piece currentPiece = context.CurrentPiece;

        if (clonedPiece == null)
        {
            Debug.LogError("Failed to clone the piece.");
            return false;
        }

        context.CurrentPlayer.AddPiece(clonedPiece);
        
        CreateVisualClone(clonedPiece, context);

        context.CurrentPlayer.RecordAbilityUse();
        currentPiece.View.PlayAbilityEffect(new Color(0f, 0f, 0f, 0.8f));
        currentPiece.View.PlayAbilitySound();

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
        var clonePosition = FindNearestFreeTile(originalPosition, context);

        if (clonePosition == null)
        {
            Debug.LogWarning("No valid position available for visual clone.");
            return;
        }

        var tile = context.Board.GetTileAtPosition(clonePosition.Value.x, clonePosition.Value.y);
        tile.OccupyingPiece = clonedPiece; 

        var tileGO = context.BoardView.GetTileObject(clonePosition.Value.x, clonePosition.Value.y);
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
        clonedPiece.UpdatePosition(clonePosition.Value);
    }

    private (int x, int y)? FindNearestFreeTile((int x, int y) start, Context context)
    {
        var board = context.Board;
        var directions = new (int x, int y)[]
        {
            (0, 1), (1, 0), (0, -1), (-1, 0), 
            (1, 1), (1, -1), (-1, 1), (-1, -1)
        };

        var visited = new HashSet<(int, int)>();
        var queue = new Queue<((int x, int y) position, int distance)>();
        queue.Enqueue((start, 0));

        while (queue.Count > 0)
        {
            var (current, _) = queue.Dequeue();

            if (visited.Contains(current)) continue;
            visited.Add(current);

            if (board.IsWithinBounds(current.x, current.y))
            {
                var tile = board.GetTileAtPosition(current.x, current.y);

                bool isTileFree = tile != null && !(tile is TrapTile) && !(tile is ObstacleTile) &&
                                !tile.IsOccupied &&
                                (!(tile is CollectibleTile collectibleTile) || collectibleTile.Collectible == null);

                if (isTileFree)
                {
                    return current;
                }
            }

            foreach (var direction in directions)
            {
                var neighbor = (current.x + direction.x, current.y + direction.y);
                if (!visited.Contains(neighbor) && board.IsWithinBounds(neighbor.Item1, neighbor.Item2))
                {
                    queue.Enqueue((neighbor, 0));
                }
            }
        }

        return null;
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
