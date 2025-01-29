using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PieceView : MonoBehaviour
{
    private Animator animator;
    private Vector2 lastDirection = Vector2.right;
    private RectTransform rectTransform;

    public GameObject FreezeIndicator { get; set; }
    private GameObject shieldIndicator;

    public float moveDuration = 0.4f; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in PieceView.");
        }
        
    }

    public void UpdateAnimation(Vector2 direction, bool isMoving)
    {
        if (animator == null) return;

      
        if (isMoving && direction != Vector2.zero)
        {
            lastDirection = direction.normalized; 
        }
        
        
        animator.SetFloat("Horizontal", lastDirection.x);
        animator.SetFloat("Vertical", lastDirection.y);
        animator.SetBool("IsMoving", isMoving);

    }

    public IEnumerator AnimateMovement(Vector3 targetPosition, System.Action onMovementComplete)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        UpdateAnimation(direction, true);

        yield return MoveSmoothly(targetPosition);

        UpdateAnimation(Vector2.zero, false);

        onMovementComplete?.Invoke();
    }
    
    private IEnumerator MoveSmoothly(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(rectTransform.position, targetPosition);
        float duration = moveDuration; 
        float timeElapsed = 0f;

        rectTransform.position = targetPosition;

        while (timeElapsed < duration)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void SetVisibility(bool isVisible)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(isVisible);
        }
    }

    public void ShowShield()
    {
        if (shieldIndicator == null)
        {
            shieldIndicator = new GameObject("ShieldIndicator");
            var image = shieldIndicator.AddComponent<Image>();

            image.sprite = CreateCircleSprite(64);
            image.color = new Color(1f, 0.84f, 0f, 0.4f);

            shieldIndicator.transform.SetParent(this.transform.parent, false);
            shieldIndicator.transform.localPosition = Vector3.zero;
            shieldIndicator.transform.localScale = Vector3.one * 1.5f;

            shieldIndicator.transform.SetAsLastSibling();
        }

        shieldIndicator.SetActive(true);
        Debug.Log("Shield effect activated!");
    }

    public void HideShield()
    {
        if (shieldIndicator != null)
        {
            shieldIndicator.SetActive(false);
        }
        Debug.Log("Shield effect deactivated!");
    }

    private Sprite CreateCircleSprite(int diameter)
    {
        Texture2D texture = new Texture2D(diameter, diameter, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[diameter * diameter];
        int radius = diameter / 2;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                Vector2 pixelPos = new Vector2(x, y);
                float distance = Vector2.Distance(pixelPos, center);
                pixels[y * diameter + x] = distance <= radius ? Color.white : Color.clear;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        Rect rect = new Rect(0, 0, diameter, diameter);
        return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }

}
