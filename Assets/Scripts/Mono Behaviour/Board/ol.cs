using UnityEngine;

public class Grid3D : MonoBehaviour
{
    public GameObject cellPrefab; // Asigna aquí un prefab de cubo o celda
    public int rows = 10;
    public int columns = 10;
    public float cellSize = 1.0f;

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, z * cellSize);
                Instantiate(cellPrefab, position, Quaternion.identity, transform);
            }
        }
    }

    private Vector3 GridToWorldPosition(Vector2Int gridPosition)
{
    float x = gridPosition.x * cellSize;
    float z = gridPosition.y * cellSize;
    float y = 0.5f; // Eleva la ficha ligeramente sobre la casilla
    return new Vector3(x, y, z);
}

}
