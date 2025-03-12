
using MazeRunners;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WallDestroyerAbility : IAbility
{
    public string Description => "Destroys a wall that blocks the path of the actual player.";

    public bool Execute(Context context)
    {
        var currentPiece = context.CurrentPiece;

        var board = context.Board;
        var boardView = context.BoardView;
        var currentPosition = currentPiece.Position;

        (int dx, int dy) direction = GetFacingDirection(context);
        int targetX = currentPosition.x + direction.dx;
        int targetY = currentPosition.y + direction.dy;

        if (board.IsWithinBounds(targetX, targetY))
        {
            var targetTile = board.GetTileAtPosition(targetX, targetY);

            if (targetTile is ObstacleTile)
            {
                board.ReplaceTile(targetX, targetY, new Tile(targetX, targetY));
                ReplaceTileVisual(boardView, targetX, targetY, board);

                context.CurrentPlayer.RecordAbilityUse();
                currentPiece.View.PlayAbilityEffect(new Color(0f, 0.39f, 0f, 0.8f));
                GameEvents.TriggerWallDestroyerUsed();

                Debug.Log($"Wall destroyed at ({targetX}, {targetY}).");
                return true;
            }
            else
            {
                Debug.LogWarning($"No wall to destroy at ({targetX}, {targetY}).");
                return false;
            }
        }

        Debug.LogError($"Target position ({targetX}, {targetY}) is out of bounds.");
        return false;
    }

    private void ReplaceTileVisual(BoardView boardView, int x, int y, Board board)
    {
        var tileGO = boardView.GetTileObject(x, y);
        if (tileGO == null)
        {
            Debug.LogError($"No tile found at ({x}, {y}).");
            return;
        }

        var gridLayoutGroup = boardView.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.enabled = false; 

        LeanTween.scale(tileGO, Vector3.zero, 0.4f).setEaseInBack();
        LeanTween.color(tileGO, Color.red, 0.4f)
        .setOnComplete(() =>
        {
            GameObject.Destroy(tileGO);

            var newTileGO = GameObject.Instantiate(boardView.horizontalTilePrefab, boardView.transform);
            newTileGO.name = $"Tile ({x}, {y})";
            newTileGO.transform.localRotation = Quaternion.identity;

            boardView.UpdateTileReference(x, y, newTileGO);

            LeanTween.scale(newTileGO, Vector3.one, 0.5f).setEaseOutElastic()
            .setOnComplete(() =>
            {
                ReorderTilesByName(boardView);
                gridLayoutGroup.enabled = true;
            });
        });
    }

    private void ReorderTilesByName(BoardView boardView)
    {
        var tiles = new List<Transform>();

        foreach (Transform child in boardView.transform)
        {
            if (child.name.StartsWith("Tile ("))
            {
                tiles.Add(child);
            }
        }

        tiles.Sort((a, b) =>
        {
            Vector2Int posA = ExtractPosition(a.name);
            Vector2Int posB = ExtractPosition(b.name);

            if (posA.x != posB.x)
                return posA.x.CompareTo(posB.x); 
            return posA.y.CompareTo(posB.y);   
        });

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].SetSiblingIndex(i);
        }
    }

    private Vector2Int ExtractPosition(string name)
    {
        int startIndex = name.IndexOf('(') + 1;
        int endIndex = name.IndexOf(')');
        string[] coordinates = name.Substring(startIndex, endIndex - startIndex).Split(',');

        int x = int.Parse(coordinates[0].Trim());
        int y = int.Parse(coordinates[1].Trim());

        return new Vector2Int(x, y);
    }

    private (int dx, int dy) GetFacingDirection(Context context)
    {
        Directions direction = context.PlayerDirection;
        return direction switch
        {
            Directions.Up => (0, -1),
            Directions.Down => (0, 1),
            Directions.Left => (-1, 0),
            Directions.Right => (1, 0),
            _ => (0, 0),
        };
    }
}
