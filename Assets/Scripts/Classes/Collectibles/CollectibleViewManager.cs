using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleViewManager : MonoBehaviour
{
    [SerializeField] private GameObject collectiblePrefab; 
    public PieceGridView pieceGridView;
    public List<GameObject> collectibleObjects = new List<GameObject>();
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
            { "MagicDoor", Resources.Load<Sprite>("MagicDoor") },

            { "CrystalDagger", Resources.Load<Sprite>("CrystalDagger") },
            { "Kodama", Resources.Load<Sprite>("Kodama") },
            { "MononokeMask", Resources.Load<Sprite>("MononokeMask") },

            { "Cage", Resources.Load<Sprite>("Cage") },
            { "Radio", Resources.Load<Sprite>("Radio") },
            { "RedShoes", Resources.Load<Sprite>("RedShoes") },

            { "BathTokens", Resources.Load<Sprite>("BathTokens") },
            { "Hairband", Resources.Load<Sprite>("Hairband") },
            { "Susuwatari", Resources.Load<Sprite>("Susuwatari") },

            { "Acorn", Resources.Load<Sprite>("Acorn") },
            { "Chibi-Totoro", Resources.Load<Sprite>("Chibi-Totoro") },
            { "Corn", Resources.Load<Sprite>("Corn") },

            { "Pilot Goggles", Resources.Load<Sprite>("Pilot Goggles") },
            { "Propeller", Resources.Load<Sprite>("Propeller") },
            { "AmeliaScarf", Resources.Load<Sprite>("AmeliaScarf") },

            { "Teacup", Resources.Load<Sprite>("Teacup") },
            { "NeedleSword", Resources.Load<Sprite>("NeedleSword") },
            { "SugarCube", Resources.Load<Sprite>("SugarCube") }
        };
    }

    public GameObject CreateCollectibleVisual(Collectible collectible)
    {
        if (collectible == null || collectibleSprites == null || collectiblePrefab == null)
        {
            Debug.LogError("Error: Missing collectible, sprite dictionary, or prefab.");
            return null;
        }

        if (!collectibleSprites.TryGetValue(collectible.Name, out Sprite sprite))
        {
            Debug.LogWarning($"No sprite found for collectible: {collectible.Name}. Using default sprite.");
            sprite = GetDefaultSprite();
        }

        GameObject collectibleObject = Instantiate(collectiblePrefab);

        Image image = collectibleObject.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Prefab missing Image component.");
            Destroy(collectibleObject);
            return null;
        }

        image.sprite = sprite;

        collectibleObject.transform.localScale = Vector3.one;
        collectibleObject.name = $"Collectible_{collectible.Name}";
        collectibleObjects.Add(collectibleObject);

        return collectibleObject;
    }

    private Sprite GetDefaultSprite()
    {
        return Resources.Load<Sprite>("Sprites/DefaultCollectible");
    }

}
