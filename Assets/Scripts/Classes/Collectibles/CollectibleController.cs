using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public GameObject collectiblePrefab;
    private List<Collectible> collectibles;
    private Transform collectiblesParent;
    public CollectibleViewManager collectibleViewManager;

    public void Initialize(List<Collectible> collectibles, Transform parent)
    {
        this.collectibles = collectibles;
        this.collectiblesParent = parent;

        CreateCollectibleObjects();
    }

    private void CreateCollectibleObjects()
    {
        foreach (var collectible in collectibles)
        {
            GameObject collectibleObject = Instantiate(collectiblePrefab, collectiblesParent);

            // Verificar si se encuentra el sprite en el CollectibleViewManager
            if (collectibleViewManager.collectibleSprites.TryGetValue(collectible.Name, out Sprite sprite))
            {
                SpriteRenderer spriteRenderer = collectibleObject.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;

                // Crear y agregar sombra
                CreateShadow(collectibleObject);
            }
            else
            {
                Debug.LogError($"No sprite found for collectible: {collectible.Name}");
            }
        }
    }


    private void CreateShadow(GameObject collectibleObject)
    {
        // Crear un objeto sombra debajo de la ficha
        GameObject shadow = new GameObject("Shadow");
        shadow.transform.SetParent(collectibleObject.transform);
        shadow.transform.localPosition = new Vector3(0, -0.2f, 0); // Ajusta según sea necesario

        SpriteRenderer shadowRenderer = shadow.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = GetShadowSprite(); // Aquí debes tener un sprite para la sombra
        shadowRenderer.sortingOrder = -1; // Para que la sombra quede detrás
    }

    private Sprite GetShadowSprite()
    {
        // Aquí puedes cargar un sprite específico para la sombra
        // Si no tienes uno, puedes crear una textura simple o un color gris
        return null; // Ejemplo, reemplázalo con el sprite de sombra
    }
}
