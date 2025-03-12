using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollectibleViewManager : MonoBehaviour
{
    [SerializeField] private GameObject collectiblePrefab; 
    public Transform hudContainer;
    private Dictionary<CollectibleName, GameObject> collectibleObjects = new Dictionary<CollectibleName, GameObject>();
    public Dictionary<CollectibleName, Sprite> collectibleSprites;

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
        collectibleSprites = new Dictionary<CollectibleName, Sprite>
        {
            { CollectibleName.Ham, Resources.Load<Sprite>("Ham") },
            { CollectibleName.Potion, Resources.Load<Sprite>("Potion") },
            { CollectibleName.Ship, Resources.Load<Sprite>("Ship") },

            { CollectibleName.Calcifer, Resources.Load<Sprite>("Calcifer") },
            { CollectibleName.Feather, Resources.Load<Sprite>("Feather") },
            { CollectibleName.MagicDoor, Resources.Load<Sprite>("MagicDoor") },

            { CollectibleName.CrystalDagger, Resources.Load<Sprite>("CrystalDagger") },
            { CollectibleName.Kodama, Resources.Load<Sprite>("Kodama") },
            { CollectibleName.MononokeMask, Resources.Load<Sprite>("MononokeMask") },

            { CollectibleName.Cage, Resources.Load<Sprite>("Cage") },
            { CollectibleName.Radio, Resources.Load<Sprite>("Radio") },
            { CollectibleName.RedShoes, Resources.Load<Sprite>("RedShoes") },

            { CollectibleName.BathTokens, Resources.Load<Sprite>("BathTokens") },
            { CollectibleName.Hairband, Resources.Load<Sprite>("Hairband") },
            { CollectibleName.Susuwatari, Resources.Load<Sprite>("Susuwatari") },

            { CollectibleName.Acorn, Resources.Load<Sprite>("Acorn") },
            { CollectibleName.ChibiTotoro, Resources.Load<Sprite>("Chibi-Totoro") },
            { CollectibleName.Corn, Resources.Load<Sprite>("Corn") },

            { CollectibleName.Hydroplane, Resources.Load<Sprite>("Hydroplane") },
            { CollectibleName.SmallPlane, Resources.Load<Sprite>("SmallPlane") },
            { CollectibleName.BrownRadio, Resources.Load<Sprite>("BrownRadio") },

            { CollectibleName.Teacup, Resources.Load<Sprite>("Teacup") },
            { CollectibleName.NeedleSword, Resources.Load<Sprite>("NeedleSword") },
            { CollectibleName.SugarCube, Resources.Load<Sprite>("SugarCube") }

        };
    }

    public GameObject CreateCollectibleVisual(Collectible collectible)
    {
        if (collectible == null || collectibleObjects.ContainsKey(collectible.Name))
        {
            return collectibleObjects.ContainsKey(collectible.Name) ? collectibleObjects[collectible.Name] : null;
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

        collectibleObject.AddComponent<CollectibleFloatingEffect>();    
        
        return collectibleObject;
    }

    private Sprite GetDefaultSprite()
    {
        return Resources.Load<Sprite>("Sprites/DefaultCollectible");
    }

    public void MoveToHUD(Collectible collectible)
    {
        GameObject collectibleGO = GetCollectibleObject(collectible.Name);

        collectibleGO.transform.SetParent(hudContainer, true);
        collectibleGO.transform.DOScale(Vector3.one * 1.2f, 0.3f).SetLoops(2, LoopType.Yoyo);
        collectibleGO.transform.DOMove(hudContainer.position, 1f).SetEase(Ease.InOutQuad)
            .OnComplete(() => ShowCollectibleEffect(collectibleGO));

        collectibleGO.transform.SetAsLastSibling();
    }

    private void ShowCollectibleEffect(GameObject collectibleGO)
    {
        GameObject effect = new GameObject("Effect");
        effect.transform.SetParent(collectibleGO.transform, false);
        Image effectImage = effect.AddComponent<Image>();
        effectImage.color = new Color(1, 1, 1, 0.8f);
        effectImage.rectTransform.sizeDelta = new Vector2(100, 100);

        effect.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase(Ease.OutQuad);
        effectImage.DOFade(0, 0.5f).OnComplete(() => Destroy(effect));
    }

    public GameObject GetCollectibleObject(CollectibleName collectibleName)
    {
        if (collectibleObjects.TryGetValue(collectibleName, out GameObject collectibleGO))
        {
            return collectibleGO;
        }

        Debug.LogError($"Collectible object with name '{collectibleName}' not found in the dictionary.");
        return null;
    }
}
