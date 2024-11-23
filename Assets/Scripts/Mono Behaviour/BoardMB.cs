using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeRunners;

public class BoardMB : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject obstaclePrefab;
    public GameObject trapPrefab;
    public GameObject exitPrefab;

    private Board board;
    private GameObject[,] tileObjects;

    public int boardSize = 10;

    private void Start()
    {
        board = new Board(boardSize);
        tileObjects = new GameObject[boardSize, boardSize];
        GenerateVisualBoard();
    }

    private void GenerateVisualBoard()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                Tile tile = board.GetTileAtPosition(x, y);
                GameObject prefab = GetPrefabForTile(tile);

                if (prefab != null)
                {
                    GameObject tileGO = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                    tileObjects[x, y] = tileGO;
                }
            }
        }
    }

    private GameObject GetPrefabForTile(Tile tile)
    {
        if (tile is ObstacleTile) return obstaclePrefab;
        if (tile is TrapTile) return trapPrefab;
        if (tile is ExitTile) return exitPrefab;

        return tilePrefab;
    }
}
