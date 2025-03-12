
using MazeRunners;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WallBombAbility : IAbility
{
    public string Description => "Explodes, destroying all walls in a 3x3 area around the piece.";

     public bool Execute(Context context)
    {
        var targetPiece = context.CurrentPiece;

        var board = context.Board;
        var boardView = context.BoardView;
        var position = targetPiece.Position;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int targetX = position.x + x;
                int targetY = position.y + y;

                if (board.IsWithinBounds(targetX, targetY))
                {
                    var tile = board.GetTileAtPosition(targetX, targetY);

                    if (tile != null)
                    {
                        if (tile is ObstacleTile || tile is TrapTile)
                        {
                            board.ReplaceTile(targetX, targetY, new Tile(targetX, targetY)); 
                            ReplaceTileVisual(boardView, targetX, targetY, board);
                        }
                    }
                }

                Debug.Log($"WallBomb destroyed wall at {targetX}, {targetY}");
            }
        }

        context.CurrentPlayer.RecordAbilityUse();
        targetPiece.View.PlayAbilityEffect(new Color(0.5f, 0f, 0f, 0.8f));
        GameEvents.TriggerWallBombUsed();

        return true;
    }

    private void ReplaceTileVisual(BoardView boardView, int x, int y, Board board)
    {
        var tileGO = boardView.GetTileObject(x, y);

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

}