
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


public class BoardView : MonoBehaviour
{
    public Tilemap tilemap; 
    public TileBase horizontalTile;
    public TileBase verticalTile;
    public TileBase obstacleTileBottom;
    public TileBase obstacleTileInterior;

    public void InitializeTileBoardView(Board board)
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                Vector3Int position = new Vector3Int(x * 100, y * 100, 0);
                MazeRunners.Tile currentTile = board.TileGrid[x, y];

                if (currentTile is ObstacleTile)
                {
                    TileBase tileToSet = DetermineObstacleTile(board, currentTile);
                    tilemap.SetTile(position, tileToSet);
                }
                else
                {
                    TileBase tileToSet = DetermineWalkableTile(board, currentTile);
                    tilemap.SetTile(position, tileToSet);
                }

                Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180));
                tilemap.SetTransformMatrix(position, rotationMatrix);

            }
        }

    }

    private TileBase DetermineObstacleTile(Board board, MazeRunners.Tile tile)
    {
        var neighbors = board.GetNeighbours(tile);

        bool hasTopObstacle = neighbors.Any(n => n.Position.y > tile.Position.y && n is ObstacleTile);

        return hasTopObstacle ? obstacleTileInterior : obstacleTileBottom;
    }

    private TileBase DetermineWalkableTile(Board board, MazeRunners.Tile tile)
    {
        var neighbors = board.GetNeighbours(tile);

        bool hasLeft = neighbors.Any(n => n.Position.x < tile.Position.x && !(n is ObstacleTile));
        bool hasRight = neighbors.Any(n => n.Position.x > tile.Position.x && !(n is ObstacleTile));
        bool hasAbove = neighbors.Any(n => n.Position.y < tile.Position.y && !(n is ObstacleTile)); 
        bool hasBelow = neighbors.Any(n => n.Position.y > tile.Position.y && !(n is ObstacleTile)); 

        if ((hasLeft && hasRight) && !(hasAbove || hasBelow))
            return horizontalTile;
        if ((hasAbove && hasBelow) && !(hasLeft || hasRight))
            return verticalTile;

        return horizontalTile; 
    }

}
