using System.Collections.Generic;
using UnityEngine;

public class CollectibleViewManager : MonoBehaviour
{
    [SerializeField] private GameObject collectiblePrefab; 
    public PieceGridView pieceGridView;
    
    public Dictionary<string, Sprite> collectibleSprites;

    private void Awake()
    {
        InitializeCollectibleSprites();
    }

    private void InitializeCollectibleSprites()
    {
        collectibleSprites = new Dictionary<string, Sprite>
        {
            { "Ham", Resources.Load<Sprite>("Ham") },
            { "Potion", Resources.Load<Sprite>("Potion") },
            { "Ship", Resources.Load<Sprite>("Ship") },
            { "Calcifer", Resources.Load<Sprite>("Calcifer") },
            { "Feather", Resources.Load<Sprite>("Feather") },
            { "MagicDoor", Resources.Load<Sprite>("MagicDoor") }
            // Añade el resto de los mapeos...
        };
    }

   public void CreateCollectibleVisual(Collectible collectible)
    {
        if (collectibleSprites == null)
        {
            Debug.LogError("Collectible sprites dictionary is null!");
            return;
        }

        if (collectiblePrefab == null)
        {
            Debug.LogError("Collectible prefab is not assigned!");
            return;
        }

        if (collectibleSprites.TryGetValue(collectible.Name, out Sprite sprite))
        {
            if (sprite is null)
            {
                Debug.LogWarning($"No sprite found for collectible: {collectible.Name}. Using default sprite.");
            }
            GameObject collectibleObject = Instantiate(collectiblePrefab);  // Sin parámetros extras
            CollectibleView view = collectibleObject.GetComponent<CollectibleView>();

            if (view is null)
            {
                Debug.LogError("CollectibleView component not found in collectible prefab!");
                return;
            }
            view.Initialize(sprite, new Vector2(0, -0.2f), 0.5f); // Configuración de sombra
        }
        else
        {
            Debug.LogWarning($"No sprite found for collectible: {collectible.Name}. Using default sprite.");
            GameObject collectibleObject = Instantiate(collectiblePrefab);  // Sin parámetros extras
            CollectibleView view = collectibleObject.GetComponent<CollectibleView>();
            view.Initialize(GetDefaultSprite(), new Vector2(0, -0.2f), 0.5f);
        }
    }

    //  public static void CreateVisualCollectible(Collectible collectible, Transform parent)
    // {
    //     // Cargar el sprite correspondiente para el coleccionable
    //     Sprite collectibleSprite = Resources.Load<Sprite>("Sprites/" + collectible.Name);  

    //     // Si no se encuentra el sprite, usa uno predeterminado
    //     if (collectibleSprite == null)
    //     {
    //         collectibleSprite = Resources.Load<Sprite>("Sprites/DefaultCollectible");
    //         Debug.LogWarning($"No sprite found for {collectible.Name}, using default sprite.");
    //     }

    //     // Crear el GameObject visual del coleccionable
    //     GameObject collectibleObject = new GameObject(collectible.Name);
    //     collectibleObject.transform.SetParent(parent);

    //     // Añadir el SpriteRenderer al objeto
    //     SpriteRenderer spriteRenderer = collectibleObject.AddComponent<SpriteRenderer>();
    //     spriteRenderer.sprite = collectibleSprite;

    //     // Crear y asignar la sombra
    //     CollectibleView view = collectibleObject.AddComponent<CollectibleView>();
    //     view.Initialize(collectibleSprite, new Vector2(0, -0.2f), 0.5f);  // Desplazamiento y opacidad de la sombra
    // }


    private Sprite GetDefaultSprite()
    {
        // Aquí puedes retornar un sprite predeterminado en caso de que no haya un sprite específico
        return Resources.Load<Sprite>("Sprites/DefaultCollectible");
    }

}
