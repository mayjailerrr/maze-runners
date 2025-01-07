using UnityEngine;
using System.Collections.Generic;
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
                Piece piece = board.GetPieceAtPosition(x, y);

                if (piece != null)
                {
                    PlacePiece(piece, x, y);
                }
                else
                {
                    PlaceTransparentTile(x, y);
                }
            }
        }
    }

    private void PlaceTransparentTile(int x, int y)
    {
        Vector3 position = GetTilePosition(x, y);
        GameObject transparentTile = Instantiate(tilePrefab, position, Quaternion.identity, boardParent);
        transparentTile.name = $"Transparent Tile ({x}, {y})";
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
       // Vector3 newPosition = GetTilePosition(newX, newY);

        // GameObject pieceObject = pieces[piece.Position.x, piece.Position.y];
        // if (pieceObject != null)
        // {
        //     // piece.View.MoveTo(newPosition);
        //     pieceObject.transform.position = newPosition;
        //     pieces[piece.Position.x, piece.Position.y] = null; 
        //     pieces[newX, newY] = pieceObject; 
        // }

         Vector3 targetPosition = GetTilePosition(newX, newY);
    piece.View.MoveTo(targetPosition);
    piece.View.transform.position = targetPosition; 
    }

    // private Vector3 GetTilePosition(int x, int y)
    // {
    //     float offsetX = -boardSize / 2.0f * tileSize; 
    //     float offsetY = -boardSize / 2.0f * tileSize; 

    //     float worldX = x * tileSize + offsetX + tileSize / 2;
    //     float worldY = y * tileSize + offsetY + tileSize / 2;

    //     return new Vector3(worldX, worldY, 0f);
    // }

    private Vector3 GetTilePosition(int x, int y)
{
    float cellWidth = 100f; // Tamaño de las celdas en unidades
    float cellHeight = 100f;
    
    // Calcula la posición local basada en el tablero
    Vector3 localPosition = new Vector3(x * cellWidth, y * cellHeight, 0);

    // Devuelve la posición local
    return localPosition;
}

    public IEnumerator AnimatePieceMovement(GameObject pieceObject, Vector3 targetPosition, float duration)
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
