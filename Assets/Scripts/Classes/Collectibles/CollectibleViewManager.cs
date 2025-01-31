using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleViewManager : MonoBehaviour
{
    [SerializeField] private GameObject collectiblePrefab; 
    public Transform hudContainer;
    private Dictionary<string, GameObject> collectibleObjects = new Dictionary<string, GameObject>();
    public Dictionary<string, Sprite> collectibleSprites;

    public static CollectibleViewManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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

        if (collectibleObjects.ContainsKey(collectible.Name))
        {
            Debug.LogWarning($"Collectible {collectible.Name} already exists in the view. Reusing existing object.");
            return collectibleObjects[collectible.Name];
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
        collectibleObjects.Add(collectible.Name, collectibleObject);

        return collectibleObject;
    }

    private Sprite GetDefaultSprite()
    {
        return Resources.Load<Sprite>("Sprites/DefaultCollectible");
    }

    public void MoveToHUD(Collectible collectible)
    {
        GameObject collectibleGO = GetCollectibleObject(collectible.Name);
        if (collectibleGO == null)
        {
            Debug.LogError($"Collectible {collectible.Name} cannot be moved to HUD because it does not exist.");
            return;
        }

        collectibleGO.transform.SetParent(hudContainer, false);
        collectibleGO.transform.localScale = Vector3.one;

        collectibleGO.transform.SetAsLastSibling();

        Debug.Log($"Collectible {collectible.Name} moved to HUD.");
    }

    public void RemoveCollectible(string collectibleName)
    {
        if (!collectibleObjects.TryGetValue(collectibleName, out GameObject collectibleGO))
        {
            Debug.LogWarning($"Collectible {collectibleName} not found in view.");
            return;
        }

        Destroy(collectibleGO);
        collectibleObjects.Remove(collectibleName);
        Debug.Log($"Collectible {collectibleName} removed from view.");
    }

    public GameObject GetCollectibleObject(string collectibleName)
    {
        if (collectibleObjects.TryGetValue(collectibleName, out GameObject collectibleGO))
        {
            return collectibleGO;
        }

        Debug.LogError($"Collectible object with name '{collectibleName}' not found in the dictionary.");
        return null;
    }
}
