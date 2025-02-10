using UnityEngine;
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
        GameObject tileObject = boardView.GetTileObject(x, y);
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
        pieceObject.transform.localScale = Vector3.one * 0.85f;

        PieceView pieceView = pieceObject.GetComponent<PieceView>();
        if (pieceView != null)
        {
            piece.View = pieceView;
            pieceView.Piece = piece;
            pieceView.BoardView = boardView;
        }
    }

    public void MovePiece(Piece piece, int previousX, int previousY, int newX, int newY)
    {   
        GameObject currentTileObject = boardView.GetTileObject(previousX, previousY);
        GameObject newTileObject = boardView.GetTileObject(newX, newY);

        if (currentTileObject == null || newTileObject == null)
        {
            Debug.LogError($"Tile object not found for position ({piece.Position.x}, {piece.Position.y}) or ({newX}, {newY}).");
            return;
        }

        if (currentTileObject.transform.childCount == 0)
        {
            Debug.LogError($"No piece found on tile ({piece.Position.x}, {piece.Position.y}).");
            return;
        }

        GameObject pieceObject = currentTileObject.transform.GetChild(0).gameObject;

        PieceView pieceView = pieceObject.GetComponent<PieceView>();
        if (pieceView != null)
        {
            Coroutine movementCoroutine = StartCoroutine(MovePieceWithAnimation(pieceView, newTileObject.transform));

            if (!boardView.activeMovements.ContainsKey(piece))
            {
                boardView.activeMovements[piece] = movementCoroutine;
            }
            else
            {
                Debug.LogWarning($"La pieza {piece.Name} ya está en activeMovements.");
            }
        }
    }

    private IEnumerator MovePieceWithAnimation(PieceView pieceView, Transform newParent)
    {
         if (!pieceView.isActiveAndEnabled) yield break;

        Vector3 targetPosition = newParent.position;

        yield return pieceView.AnimateMovement(targetPosition, () =>
        {
            if (pieceView != null && newParent != null)
            {
                pieceView.transform.SetParent(newParent, true);
                pieceView.transform.localPosition = Vector3.zero;
            }

            if (pieceView != null && pieceView.Piece != null)
            {
                if (boardView.activeMovements.ContainsKey(pieceView.Piece))
                    boardView.activeMovements.Remove(pieceView.Piece);
            }
        });
    }
}
