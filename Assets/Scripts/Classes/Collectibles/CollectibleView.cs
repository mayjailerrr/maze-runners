// using UnityEngine;
// using UnityEngine.UI;

// public class CollectibleView : MonoBehaviour
// {
//     public SpriteRenderer collectibleRenderer; 
//     private GameObject shadowObject;

//     public void Initialize(Sprite collectibleSprite, Vector2 shadowOffset, float shadowOpacity)
//     {
//         collectibleRenderer.sprite = collectibleSprite;
//         GenerateShadow(shadowOffset, shadowOpacity);
//     }

//     private void GenerateShadow(Vector2 offset, float opacity)
//     {
//         shadowObject = new GameObject("Shadow");
//         shadowObject.transform.SetParent(transform);
//         shadowObject.transform.localPosition = offset;

//         SpriteRenderer shadowRenderer = shadowObject.AddComponent<SpriteRenderer>();
//         shadowRenderer.sprite = GetShadowSprite();
//         shadowRenderer.color = new Color(0, 0, 0, opacity);
//         shadowRenderer.sortingOrder = collectibleRenderer.sortingOrder - 1;
//     }

//     private Sprite GetShadowSprite()
//     {
//         return Resources.Load<Sprite>("Sprites/Shadow");
//     }


//     private void OnDestroy()
//     {
//         if (shadowObject != null)
//         {
//             Destroy(shadowObject);
//         }
//     }
// }
