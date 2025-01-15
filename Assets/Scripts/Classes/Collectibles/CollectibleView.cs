using UnityEngine;
using UnityEngine.UI;

public class CollectibleView : MonoBehaviour
{
    public SpriteRenderer collectibleRenderer; // Imagen principal del coleccionable.
    private GameObject shadowObject; // Objeto sombra.

    public Text collectibleDescription; 

    public void Initialize(Sprite collectibleSprite, Vector2 shadowOffset, float shadowOpacity)
    {
        collectibleRenderer.sprite = collectibleSprite;
        GenerateShadow(shadowOffset, shadowOpacity);
    }

    private void GenerateShadow(Vector2 offset, float opacity)
    {
        // Crear el GameObject de la sombra
        shadowObject = new GameObject("Shadow");
        shadowObject.transform.SetParent(transform);
        shadowObject.transform.localPosition = offset;

        // Añadir un SpriteRenderer
        SpriteRenderer shadowRenderer = shadowObject.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = GetShadowSprite();
        shadowRenderer.color = new Color(0, 0, 0, opacity);
        shadowRenderer.sortingOrder = collectibleRenderer.sortingOrder - 1;
    }

    private Sprite GetShadowSprite()
    {
        // Aquí puedes cargar un sprite o usar una textura simple como sombra
        return Resources.Load<Sprite>("Sprites/Shadow"); // Asegúrate de tener este sprite en Resources/Sprites
    }


    private void OnDestroy()
    {
        // Eliminar la sombra si el objeto es destruido.
        if (shadowObject != null)
        {
            Destroy(shadowObject);
        }
    }
}
