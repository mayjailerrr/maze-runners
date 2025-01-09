
using UnityEngine;
using UnityEngine.Tilemaps;


public class BoardView : MonoBehaviour
{
    public Tilemap tilemap; 
    public TileBase obstacleTile; 
    public TileBase regularTile; 

    public void InitializeTileBoardView(Board board)
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                Vector3Int position = new Vector3Int(x * 100, y * 100, 0);

                if (board.TileGrid[x, y] is ObstacleTile)
                {
                    tilemap.SetTile(position, obstacleTile);
                }
                else
                {
                    tilemap.SetTile(position, regularTile);
                }
            }
        }
    }
}
