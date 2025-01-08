using UnityEngine;
using System.Collections;

public class PieceGridView : MonoBehaviour
{
    [Header("Prefabs & Configuration")]
    public GameObject piecePrefab;
    public Transform boardParent;

    [Header("Board Params")]
    private Board board;
    private int boardSize;
    public float tileSize = 1.0f;

    private GameObject[,] pieces;

    public void InitializeGrid(Board board)
    {
        this.board = board;
        boardSize = board.Size;
        pieces = new GameObject[boardSize, boardSize];
        GeneratePieces();
    }

    private void GeneratePieces()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                Piece piece = board.GetPieceAtPosition(x, y);
                if (piece != null)
                {
                    PlacePiece(piece, x, y);
                }
            }
        }
    }

    public void PlacePiece(Piece piece, int x, int y)
    {
        Vector3 position = GetTilePosition(x, y);

        GameObject pieceObject = Instantiate(piecePrefab, position, Quaternion.identity, boardParent);
        pieceObject.name = $"Piece {piece.Name} ({x}, {y})";

        pieces[x, y] = pieceObject;

        PieceView pieceView = pieceObject.GetComponent<PieceView>();
        if (pieceView != null)
        {
            piece.View = pieceView;
        }
    }

    public void MovePiece(Piece piece, int newX, int newY)
    {
        Vector3 targetPosition = GetTilePosition(newX, newY);

        pieces[piece.Position.Item1, piece.Position.Item2] = null;
        pieces[newX, newY] = piece.View.gameObject;

        StartCoroutine(AnimatePieceMovement(piece.View.gameObject, targetPosition, 0.5f));
    }

    private Vector3 GetTilePosition(int x, int y)
    {
        float offsetX = -boardSize / 2.0f * tileSize;
        float offsetY = -boardSize / 2.0f * tileSize;

        float worldX = x * tileSize + offsetX + tileSize / 2;
        float worldY = y * tileSize + offsetY + tileSize / 2;

        return new Vector3(worldX, worldY, 0f);
    }

    private IEnumerator AnimatePieceMovement(GameObject pieceObject, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = pieceObject.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            pieceObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pieceObject.transform.position = targetPosition;
    }
}
