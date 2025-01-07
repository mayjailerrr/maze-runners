// using UnityEngine;
// using UnityEngine.Tilemaps;
// using MazeRunners;
// using System.Collections.Generic;
// using System.Linq;

// public class BoardView : MonoBehaviour
// {
//     [SerializeField] private Tilemap tilemap; 
//     [SerializeField] private TileBase tileCorridorHorizontalTile;
//     [SerializeField] private TileBase tileCorridorVerticalTile;
//     [SerializeField] private TileBase tileCornerTopRightTile;
//     [SerializeField] private TileBase tileCornerTopLeftTile;
//     [SerializeField] private TileBase tileCornerBottomRightTile;
//     [SerializeField] private TileBase tileCornerBottomLeftTile;
//     [SerializeField] private TileBase tileEmptyTile;

//     [SerializeField] private TileBase obstacleCorridorHorizontalTile;
//     [SerializeField] private TileBase obstacleCorridorVerticalTile;
//     [SerializeField] private TileBase obstacleCornerTopRightTile;
//     [SerializeField] private TileBase obstacleCornerTopLeftTile;
//     [SerializeField] private TileBase obstacleCornerBottomRightTile;
//     [SerializeField] private TileBase obstacleCornerBottomLeftTile;
//     [SerializeField] private TileBase obstacleEmptyTile;

//     private Dictionary<string, TileBase> tileDictionary;

//     private void Awake()
//     {
//         tileDictionary = new Dictionary<string, TileBase>()
//         {
//             { "tile_corridor_horizontal", tileCorridorHorizontalTile },
//             { "tile_corridor_vertical", tileCorridorVerticalTile },
//             { "tile_corner_top_right", tileCornerTopRightTile },
//             { "tile_corner_top_left", tileCornerTopLeftTile },
//             { "tile_corner_bottom_right", tileCornerBottomRightTile },
//             { "tile_corner_bottom_left", tileCornerBottomLeftTile },
//             { "tile_empty", tileEmptyTile },

//             { "obstacle_corridor_horizontal", obstacleCorridorHorizontalTile },
//             { "obstacle_corridor_vertical", obstacleCorridorVerticalTile },
//             { "obstacle_corner_top_right", obstacleCornerTopRightTile },
//             { "obstacle_corner_top_left", obstacleCornerTopLeftTile },
//             { "obstacle_corner_bottom_right", obstacleCornerBottomRightTile },
//             { "obstacle_corner_bottom_left", obstacleCornerBottomLeftTile },
//             { "obstacle_empty", obstacleEmptyTile }
//         };
//     }

//     public void InitializeTileBoardView(Board board)
//     {
//         tilemap.ClearAllTiles();
//         GenerateVisualBoard(board);
//     }

//     private void GenerateVisualBoard(Board board)
//     {
//         for (int x = 0; x < board.Size; x++)
//         {
//             for (int y = 0; y < board.Size; y++)
//             {
//                 MazeRunners.Tile tile = board.GetTileAtPosition(x, y);
//                 string tileType = GetTileType(tile, board);

//                 if (tileDictionary.ContainsKey(tileType))
//                 {
//                     var tileBase = tileDictionary[tileType];
//                     tilemap.SetTile(new Vector3Int(x, -y, 0), tileBase);
//                 }
//                 else
//                 {
//                     Debug.LogWarning($"No tile found for type: {tileType}");
//                 }
//             }
//         }
//     }

//     private string GetTileType(MazeRunners.Tile tile, Board board)
//     {
//         var neighbours = board.GetNeighbours(tile).ToList();

//         bool hasTop = neighbours.Any(n => n.Position.y == tile.Position.y + 1 && (n is ObstacleTile || n is MazeRunners.Tile));
//         bool hasBottom = neighbours.Any(n => n.Position.y == tile.Position.y - 1 && (n is ObstacleTile || n is MazeRunners.Tile));
//         bool hasLeft = neighbours.Any(n => n.Position.x == tile.Position.x - 1 && (n is ObstacleTile || n is MazeRunners.Tile));
//         bool hasRight = neighbours.Any(n => n.Position.x == tile.Position.x + 1 && (n is ObstacleTile || n is MazeRunners.Tile));

//         if (tile is MazeRunners.Tile)
//         {
//             if (hasLeft && hasRight && !hasTop && !hasBottom) return "tile_corridor_horizontal";
//             if (hasTop && hasBottom && !hasLeft && !hasRight) return "tile_corridor_vertical";
//             if (hasTop && hasRight && !hasBottom && !hasLeft) return "tile_corner_top_right";
//             if (hasTop && hasLeft && !hasBottom && !hasRight) return "tile_corner_top_left";
//             if (hasBottom && hasRight && !hasTop && !hasLeft) return "tile_corner_bottom_right";
//             if (hasBottom && hasLeft && !hasTop && !hasRight) return "tile_corner_bottom_left";

//             return "tile_empty";
//         }

//         if (tile is ObstacleTile)
//         {
//             if (hasLeft && hasRight && !hasTop && !hasBottom) return "obstacle_corridor_horizontal";
//             if (hasTop && hasBottom && !hasLeft && !hasRight) return "obstacle_corridor_vertical";
//             if (hasTop && hasRight && !hasBottom && !hasLeft) return "obstacle_corner_top_right";
//             if (hasTop && hasLeft && !hasBottom && !hasRight) return "obstacle_corner_top_left";
//             if (hasBottom && hasRight && !hasTop && !hasLeft) return "obstacle_corner_bottom_right";
//             if (hasBottom && hasLeft && !hasTop && !hasRight) return "obstacle_corner_bottom_left";

//             return "obstacle_empty";
//         }

//         return "tile_empty";
//     }
// }

using UnityEngine;
using UnityEngine.Tilemaps;


public class BoardView : MonoBehaviour
{
    public Tilemap tilemap; // Referencia al Tilemap
    public TileBase obstacleTile; // Tile para los obstáculos
    public TileBase regularTile; // Tile para casillas normales

    public void InitializeTileBoardView(Board board)
    {
        Debug.Log("Inicializando tablero...");
        tilemap.ClearAllTiles(); // Limpiar el Tilemap previo

        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                Vector3Int position = new Vector3Int(x * 100, y * 100, 0);

                if (board.TileGrid[x, y] is ObstacleTile)
                {
                     Debug.Log($"Setting tile at {position} to {(board.TileGrid[x, y] is ObstacleTile ? "Obstacle" : "Regular")}");
                    tilemap.SetTile(position, obstacleTile);
                }
                else
                {
                    tilemap.SetTile(position, regularTile);
                }
            }
        }

        Debug.Log("Tablero inicializado.");
    }
}
