using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MazeRunners;

public class PieceGridView : MonoBehaviour
{
    [Header("Prefabs & Configuration")]
    public PiecePrefabRegistry piecePrefabRegistry; 
    private BoardView boardView;
    private Board board;

    [Header("Parent Objects")]
    public Transform boardParent;

    public void InitializeGrid(Board board, BoardView boardView)
    {
        this.board = board;
        this.boardView = boardView;
        PlaceAllPieces();
    }

    private void PlaceAllPieces()
    {
        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                Tile tile = board.TileGrid[x, y];
                if (tile.OccupyingPiece != null)
                {
                    PlacePiece(tile.OccupyingPiece, x, y);
                }
            }
        }
    }

    public void PlacePiece(Piece piece, int x, int y)
    {
        GameObject tileObject = boardView.GetTileObject(x, y); // Usa el tile de BoardView
        if (tileObject == null)
        {
            Debug.LogError($"No tile found at ({x}, {y}) to place piece {piece.Name}.");
            return;
        }

        GameObject prefab = piecePrefabRegistry.GetPrefab(piece.Name);
        if (prefab == null)
        {
            Debug.LogError($"No prefab found for piece {piece.Name}. Skipping instantiation.");
            return;
        }

        GameObject pieceObject = Instantiate(prefab, tileObject.transform.position, Quaternion.identity, tileObject.transform);
        pieceObject.name = $"Piece {piece.Name} ({x}, {y})";

        PieceView pieceView = pieceObject.GetComponent<PieceView>();
        if (pieceView != null)
        {
            piece.View = pieceView;
        }
    }

    public void MovePiece(Piece piece, int newX, int newY)
    {   
        // Obtén el objeto del tile actual y el nuevo tile
        GameObject currentTileObject = boardView.GetTileObject(piece.Position.x, piece.Position.y);
        GameObject newTileObject = boardView.GetTileObject(newX, newY);

        if (currentTileObject == null || newTileObject == null)
        {
            Debug.LogError($"Tile object not found for position ({piece.Position.x}, {piece.Position.y}) or ({newX}, {newY}).");
            return;
        }

        // Verificar si el tile actual tiene un hijo
        if (currentTileObject.transform.childCount == 0)
        {
            Debug.LogError($"No piece found on tile ({piece.Position.x}, {piece.Position.y}).");
            return;
        }

        GameObject pieceObject = currentTileObject.transform.GetChild(0).gameObject;

        PieceView pieceView = pieceObject.GetComponent<PieceView>();
        if (pieceView != null)
        {
            StartCoroutine(MovePieceWithAnimation(pieceView, newTileObject.transform));
        }
    }

    private IEnumerator MovePieceWithAnimation(PieceView pieceView, Transform newParent)
    {
        Vector3 targetPosition = newParent.position;

        yield return pieceView.AnimateMovement(targetPosition, () =>
        {
            pieceView.transform.SetParent(newParent, true);
            pieceView.transform.localPosition = Vector3.zero;
        });
    }
}
