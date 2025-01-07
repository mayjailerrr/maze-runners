using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class TilemapCameraController : MonoBehaviour
{
    public Camera tilemapCamera; // Asigna tu nueva cámara
    public Grid grid; // Asigna tu Grid principal

    void Start()
    {
        if (tilemapCamera == null || grid == null)
        {
            Debug.LogError("Falta asignar la cámara o el Grid al script.");
            return;
        }

        StartCoroutine(WaitForGridToActivate());
    }

    private IEnumerator WaitForGridToActivate()
    {
        // Esperar hasta que el Grid esté activo
        while (!grid.gameObject.activeInHierarchy)
        {
            yield return null; // Espera al siguiente frame
        }

        // Ajustar la cámara después de que el Grid esté activo
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        // Encuentra el TilemapRenderer en los hijos del Grid
        TilemapRenderer tilemapRenderer = grid.GetComponentInChildren<TilemapRenderer>();

        if (tilemapRenderer == null)
        {
            Debug.LogError("No se encontró un TilemapRenderer en el Grid.");
            return;
        }

        // Obtener los bounds del TilemapRenderer
        Bounds tilemapBounds = tilemapRenderer.bounds;

        // Ajustar la posición de la cámara
        tilemapCamera.transform.position = new Vector3(
            tilemapBounds.center.x,
            tilemapBounds.center.y,
            tilemapCamera.transform.position.z
        );

        // Ajustar el tamaño de la cámara (si es ortográfica)
        if (tilemapCamera.orthographic)
        {
            tilemapCamera.orthographicSize = Mathf.Max(tilemapBounds.size.x, tilemapBounds.size.y) / 2f;
        }
    }
}
