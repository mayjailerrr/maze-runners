
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;
using System;
using System.Linq;


public class Board
{
    public int Size { get; private set; }
    public Tile[,] TileGrid;

    public BoardGenerator Generator;

    public Board(int size, List<Collectible> collectibles)
    {
        Size = size;
        TileGrid = new Tile[size, size];

        Generator = new BoardGenerator(this);

        Generator.GenerateBoard(collectibles);
        PrintGridsToConsole();
    }

    public void PrintGridsToConsole()
    {
        Debug.Log("TileGrid:");
        for (int y = 0; y < TileGrid.GetLength(1); y++)
        {
            string row = "";
            for (int x = 0; x < TileGrid.GetLength(0); x++)
            {
                row += TileGrid[x, y]?.ToString() ?? "null";
                row += "\t"; 
            }
            Debug.Log(row);
        }
    }

    public Tile GetTileAtPosition(int x, int y)
    {
        if (IsWithinBounds(x, y)) return TileGrid[x, y];

        return null;

    }

    public void PlacePiecesRandomly(IEnumerable<Piece> piecesToPlace)
    {
        System.Random random = new System.Random();
        List<Tile> neutralTiles = new List<Tile>();


        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                Tile tile = TileGrid[x, y];
                if (!(tile is ObstacleTile) && !(tile is TrapTile) && !(tile is CollectibleTile))
                {
                    neutralTiles.Add(tile);
                }
            }
        }

        foreach (Piece piece in piecesToPlace)
        {
            if (neutralTiles.Count == 0)
            {
                Debug.LogError("No more neutral tiles available to place pieces.");
                return;
            }

            int randomIndex = random.Next(neutralTiles.Count);
            Tile selectedTile = neutralTiles[randomIndex];
            neutralTiles.RemoveAt(randomIndex);

            selectedTile.OccupyingPiece = piece;
            piece.InitialPosition = (selectedTile.Position.x, selectedTile.Position.y);
            piece.UpdatePosition((selectedTile.Position.x, selectedTile.Position.y));
        }
    }

    public Tile GetRandomTile()
    {
        List<Tile> neutralTiles = new List<Tile>();

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                Tile tile = TileGrid[x, y];
                if (!(tile is ObstacleTile) && !(tile is TrapTile) && !(tile is CollectibleTile))
                {
                    neutralTiles.Add(tile);
                }
            }
        }

        if (neutralTiles.Count == 0)
        {
            Debug.LogError("No neutral tiles available.");
            return null;
        }

        System.Random random = new System.Random();
        int randomIndex = random.Next(neutralTiles.Count);
        return neutralTiles[randomIndex];
    }

    public IEnumerable<Tile> GetNeighbours(Tile tile)
    {
        int x = tile.Position.x;
        int y = tile.Position.y;

        int [,] directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int nx = x + directions[i, 0];
            int ny = y + directions[i, 1];

            if (IsWithinBounds(nx, ny))
            {
                yield return TileGrid[nx, ny];
            }
        }
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < Size && y >= 0 && y < Size;
    }


    public bool IsValidMove(Piece piece, int targetX, int targetY)
    {
        if (!IsWithinBounds(targetX, targetY)) return false;

        Tile targetTile = GetTileAtPosition(targetX, targetY);

        return !(targetTile is null) && !(targetTile is ObstacleTile);
    }

    public void ReplaceTile(int x, int y, Tile newTile)
    {
        if (!IsWithinBounds(x, y))
        {
            Debug.LogError($"Position ({x}, {y}) is out of bounds.");
            return;
        }

        TileGrid[x, y] = newTile;
    }
}