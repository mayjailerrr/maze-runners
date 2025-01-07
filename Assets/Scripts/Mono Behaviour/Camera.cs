using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public Camera mainCamera;

    void Start()
    {
        // Si no se asigna una cámara, toma la cámara principal
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Asegúrate de que hay una cámara asignada
        if (mainCamera != null)
        {
            // Modifica el valor de Near Clipping Plane
            mainCamera.nearClipPlane = 0.1f;

            // Puedes ajustar otros parámetros si es necesario
            mainCamera.farClipPlane = 1000f;
            mainCamera.orthographicSize = 540f; // Si es una cámara ortográfica
        }
        else
        {
            Debug.LogError("No se encontró ninguna cámara principal en la escena.");
        }
    }
}
