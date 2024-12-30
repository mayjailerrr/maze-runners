using UnityEngine;
using MazeRunners;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    [SerializeField] private Sprite tileCorridorHorizontalSprite;
    [SerializeField] private Sprite tileCorridorVerticalSprite;
    [SerializeField] private Sprite tileCornerTopRightSprite;
    [SerializeField] private Sprite tileCornerTopLeftSprite;
    [SerializeField] private Sprite tileCornerBottomRightSprite;
    [SerializeField] private Sprite tileCornerBottomLeftSprite;
    [SerializeField] private Sprite tileEmptySprite;

    [SerializeField] private Sprite obstacleCorridorHorizontalSprite;
    [SerializeField] private Sprite obstacleCorridorVerticalSprite;
    [SerializeField] private Sprite obstacleCornerTopRightSprite;
    [SerializeField] private Sprite obstacleCornerTopLeftSprite;
    [SerializeField] private Sprite obstacleCornerBottomRightSprite;
    [SerializeField] private Sprite obstacleCornerBottomLeftSprite;
    [SerializeField] private Sprite obstacleEmptySprite;

    public GameObject tilePrefab;
    public GameObject obstaclePrefab;
    public GameObject trapPrefab;
    public GameObject collectiblePrefab;

    private GameObject[,] tileObjects;

    public GameObject piecePrefab;
    private Dictionary<Piece, GameObject> pieceGameObjects = new Dictionary<Piece, GameObject>();

    private Dictionary<string, Sprite> tileSprites;

    private void Awake()
    {
        tileSprites = new Dictionary<string, Sprite>()
        {
            // Sprites para Tile
            { "tile_corridor_horizontal", tileCorridorHorizontalSprite },
            { "tile_corridor_vertical", tileCorridorVerticalSprite },
            { "tile_corner_top_right", tileCornerTopRightSprite },
            { "tile_corner_top_left", tileCornerTopLeftSprite },
            { "tile_corner_bottom_right", tileCornerBottomRightSprite },
            { "tile_corner_bottom_left", tileCornerBottomLeftSprite },
            { "tile_empty", tileEmptySprite },

            // Sprites para ObstacleTile
            { "obstacle_corridor_horizontal", obstacleCorridorHorizontalSprite },
            { "obstacle_corridor_vertical", obstacleCorridorVerticalSprite },
            { "obstacle_corner_top_right", obstacleCornerTopRightSprite },
            { "obstacle_corner_top_left", obstacleCornerTopLeftSprite },
            { "obstacle_corner_bottom_right", obstacleCornerBottomRightSprite },
            { "obstacle_corner_bottom_left", obstacleCornerBottomLeftSprite },
            { "obstacle_empty", obstacleEmptySprite }
        };
    }


    public void InitializeView(Board board)
    {
        int boardSize = board.Size;
        tileObjects = new GameObject[boardSize, boardSize];
        GenerateVisualBoard(board);
    }

    private void GenerateVisualBoard(Board board)
    {
        for (int x = 0; x < board.Size; x++)
        {
            for (int y = 0; y < board.Size; y++)
            {
                Tile tile = board.GetTileAtPosition(x, y);

                string tileType = GetTileType(tile, board);

                GameObject prefab = GetPrefabForTile(tile);

                if (prefab != null)
                {
                    GameObject tileGO = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, transform);

                    Image renderer = tileGO.GetComponent<Image>();
                    if (tileSprites.ContainsKey(tileType))
                    {
                        renderer.sprite = tileSprites[tileType];
                    }
                    else
                    {
                        Debug.LogWarning($"No sprite found for tile type {tileType}");
                    }

                    tileObjects[x, y] = tileGO;
                }
            }
        }
    }

    private GameObject GetPrefabForTile(Tile tile)
    {
        if (tile is ObstacleTile) return obstaclePrefab;
        if (tile is TrapTile) return trapPrefab;
        if (tile is CollectibleTile) return collectiblePrefab;

        return tilePrefab;
    }

    private string GetTileType(Tile tile, Board board)
    {
        var neighbours = board.GetNeighbours(tile).ToList();
       
        bool hasTop = neighbours.Any(n => n.Position.y == tile.Position.y + 1 && (n is ObstacleTile || n is Tile));
        bool hasBottom = neighbours.Any(n => n.Position.y == tile.Position.y - 1 && (n is ObstacleTile || n is Tile));
        bool hasLeft = neighbours.Any(n => n.Position.x == tile.Position.x - 1 && (n is ObstacleTile || n is Tile));
        bool hasRight = neighbours.Any(n => n.Position.x == tile.Position.x + 1 && (n is ObstacleTile || n is Tile));

        bool topIsObstacle = neighbours.Any(n => n.Position.y == tile.Position.y + 1 && n is ObstacleTile);
        bool bottomIsObstacle = neighbours.Any(n => n.Position.y == tile.Position.y - 1 && n is ObstacleTile);
        bool leftIsObstacle = neighbours.Any(n => n.Position.x == tile.Position.x - 1 && n is ObstacleTile);
        bool rightIsObstacle = neighbours.Any(n => n.Position.x == tile.Position.x + 1 && n is ObstacleTile);

        if (tile is Tile)
        {
            if (leftIsObstacle && rightIsObstacle && !topIsObstacle && !bottomIsObstacle) return "tile_corridor_horizontal";
            if (topIsObstacle && bottomIsObstacle && !leftIsObstacle && !rightIsObstacle) return "tile_corridor_vertical";
            if (topIsObstacle && rightIsObstacle && !bottomIsObstacle && !leftIsObstacle) return "tile_corner_top_right";
            if (topIsObstacle && leftIsObstacle && !bottomIsObstacle && !rightIsObstacle) return "tile_corner_top_left";
            if (bottomIsObstacle && rightIsObstacle && !topIsObstacle && !leftIsObstacle) return "tile_corner_bottom_right";
            if (bottomIsObstacle && leftIsObstacle && !topIsObstacle && !rightIsObstacle) return "tile_corner_bottom_left";

            return "tile_empty";
        }

         if (tile is ObstacleTile)
        {
            if (hasLeft && hasRight && !hasTop && !hasBottom) return "obstacle_corridor_horizontal";
            if (hasTop && hasBottom && !hasLeft && !hasRight) return "obstacle_corridor_vertical";
            if (hasTop && hasRight && !hasBottom && !hasLeft) return "obstacle_corner_top_right";
            if (hasTop && hasLeft && !hasBottom && !hasRight) return "obstacle_corner_top_left";
            if (hasBottom && hasRight && !hasTop && !hasLeft) return "obstacle_corner_bottom_right";
            if (hasBottom && hasLeft && !hasTop && !hasRight) return "obstacle_corner_bottom_left";

            return "obstacle_empty"; 
        }

        return "empty";
    }
    
    public void UpdatePiecePosition(Piece piece)
    {
        PieceView pieceView = piece.View;
        if (pieceView == null)
        {
            Debug.LogWarning($"PieceView not found for piece: {piece.Name}");
            return;
        }

        Vector2 targetPosition = GetTilePosition(piece.Position.Item1, piece.Position.Item2);
        pieceView.MoveTo(targetPosition);

        Vector2 direction = CalculateDirection(piece);
        bool isMoving = direction != Vector2.zero;

        pieceView.UpdateAnimation(direction, isMoving);
    }

    private Vector2 CalculateDirection(Piece piece)
    {
        (int oldX, int oldY) = piece.PreviousPosition.Value; 
        (int newX, int newY) = piece.Position;

        return new Vector2(newX - oldX, newY - oldY);
    }


    public void InitializePieces(IEnumerable<Piece> pieces)
    {
         foreach (var piece in pieces)
        {
            GameObject pieceObject = Instantiate(piecePrefab, transform);
            PieceView pieceView = pieceObject.GetComponent<PieceView>();
            piece.View = pieceView; 
            pieceObject.transform.position = new Vector2(piece.Position.Item1, piece.Position.Item2);
        
        }
    }

    public PieceView GetPieceView(Piece piece)
    {
        return FindObjectOfType<PieceView>();
    }

    public Vector3 GetTilePosition(int x, int y)
    {
        float tileSize = 1f;

        return new Vector3(x * tileSize, y * tileSize, 0f); 
    }

}
