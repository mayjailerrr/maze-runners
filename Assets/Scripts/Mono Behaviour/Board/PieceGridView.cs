using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PieceGridView : MonoBehaviour
{
    [Header("Prefabs & Configuration")]
    public GameObject tilePrefab; 
    public GameObject piecePrefab; 
    public Transform boardParent; 

    [Header("Board Params")]
    private Board board;
    private int boardSize;
    public float tileSize = 1.0f;
    private GameObject[,] tiles; 
    private GameObject[,] pieces; 

    public void InitializeGrid(Board board)
    {
        this.board = board;
        boardSize = board.Size;
        tiles = new GameObject[boardSize, boardSize];
        pieces = new GameObject[boardSize, boardSize];
        GenerateVisualBoard(board);
    }

    private void GenerateVisualBoard(Board board)
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                CreateTile(x, y);

                Piece piece = board.GetPieceAtPosition(x, y);
                if (piece != null)
                {
                    PlacePiece(piece, x, y);
                }
            }
        }

    }

    private void CreateTile(int x, int y)
    {
        Vector3 position = GetTilePosition(x, y);
        GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, boardParent);
        tile.name = $"Tile ({x}, {y})";
        tiles[x, y] = tile;
    }

    public void PlacePiece(Piece piece, int x, int y)
    {
        GameObject tile = tiles[x, y];
        GameObject pieceObject = Instantiate(piecePrefab, tile.transform.position, Quaternion.identity, tile.transform);
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
        boardParent.GetComponent<GridLayoutGroup>().enabled = false;

        GameObject pieceObject = pieces[piece.Position.x, piece.Position.y];
        if (pieceObject != null)
        {
            pieces[piece.Position.x, piece.Position.y] = null;
            GameObject newTile = tiles[newX, newY];

            PieceView pieceView = pieceObject.GetComponent<PieceView>();
            if (pieceView != null)
            {
                pieceView.moveDuration = 2f; 

                StartCoroutine(MovePieceWithAnimation(pieceView, newTile.transform, newX, newY));
            }
        }
    }

    private IEnumerator MovePieceWithAnimation(PieceView pieceView, Transform newParent, int newX, int newY)
    {
        Vector3 targetPosition = newParent.position;

        yield return pieceView.AnimateMovement(targetPosition, () =>
        {
            pieceView.transform.SetParent(newParent, true);
            pieceView.transform.localPosition = Vector3.zero;

            pieces[newX, newY] = pieceView.gameObject;
        });
    }

    private Vector3 GetTilePosition(int x, int y)
    {
        float offsetX = -boardSize / 2.0f * tileSize; 
        float offsetY = -boardSize / 2.0f * tileSize; 
        float worldX = x * tileSize + offsetX + tileSize / 2;
        float worldY = y * tileSize + offsetY + tileSize / 2;
        return new Vector3(worldX, worldY, 0f);
    }
}
