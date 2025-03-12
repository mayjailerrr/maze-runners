using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PieceView : MonoBehaviour
{
    private Animator animator;
    private Vector2 lastDirection = Vector2.right;

    public GameObject FreezeIndicator { get; set; }
    public BoardView BoardView;
    public Piece Piece { get; set; } 

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

        if (!isMoving)
        {
            if (lastDirection.x > 0)
                animator.Play("IdleRight");
            else if (lastDirection.x < 0)
                animator.Play("IdleLeft");
        }
    }

    public IEnumerator AnimateMovement(Vector3 targetPosition, System.Action onMovementComplete)
    {
        UpdateAnimation(Vector2.zero, false);
        
        Transform originalParent = transform.parent;
        Vector3 originalScale = transform.localScale;
        Vector2 direction = (targetPosition - transform.position).normalized;
        UpdateAnimation(direction, true);

        float duration = 0.5f;
        Vector3 startPos = transform.position;
        float elapsed = 0;

        while (elapsed < duration)
        {
            if (transform.parent != originalParent || transform.localScale != originalScale)
            {
                UpdateAnimation(Vector2.zero, false);
                yield break; 
            }

            transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (transform.parent == originalParent)
        {
            transform.position = targetPosition;
            UpdateAnimation(Vector2.zero, false);
            onMovementComplete?.Invoke();
        }
    }

    public void SetVisibility(bool isVisible, Piece piece)
    {
         Image imageComponent = gameObject.GetComponent<Image>();
    
        if (imageComponent != null)
        {
            imageComponent.enabled = isVisible;
            piece.IsInvisible = !isVisible;
            piece.IsShielded = !isVisible;
        }
    }

    public void PlayAbilityEffect(Color effectColor)
    {
        GameObject halo = new GameObject("AbilityHalo");
        halo.transform.SetParent(transform);
        halo.transform.localPosition = Vector3.zero;

        var image = halo.AddComponent<Image>();
        image.sprite = CreateSimpleCircleSprite();
        image.color = effectColor;

        RectTransform rectTransform = halo.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(150, 150);
        rectTransform.localScale = Vector3.zero;

        LeanTween.scale(halo, Vector3.one, 0.5f).setEaseOutBack();
        LeanTween.alpha(rectTransform, 0f, 1.5f).setOnComplete(() =>
        {
            Destroy(halo);
        });
    }

    public Sprite CreateSimpleCircleSprite()
    {
        int resolution = 128;
        Texture2D texture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Bilinear;

        Color transparent = new Color(0, 0, 0, 0);
        Color solid = Color.white;

        float centerX = resolution / 2;
        float centerY = resolution / 2;
        float radius = resolution / 2 * 0.9f;

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                texture.SetPixel(x, y, distance > radius ? transparent : solid);
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f));
    }

    public void ShowFreezeIndicator(Piece targetPiece)
    {
        var freezeIndicator = new GameObject("FreezeIndicator");
        var image = freezeIndicator.AddComponent<Image>();

        image.color = new Color(0f, 0.5f, 1f, 0.5f);
        image.sprite = CreateBlurryCircleSprite(8);

        freezeIndicator.transform.SetParent(targetPiece.View.transform, false);
        freezeIndicator.transform.localPosition = Vector3.zero;
        freezeIndicator.transform.localScale = Vector3.one * 1.5f;
        freezeIndicator.transform.SetAsLastSibling();

        targetPiece.View.FreezeIndicator = freezeIndicator;

        LeanTween.alpha(freezeIndicator.GetComponent<RectTransform>(), 0.2f, 0.5f)
            .setLoopPingPong();
    }

    public void HideFreezeIndicator(Piece targetPiece)
    {
        if (targetPiece.View.FreezeIndicator != null)
        {
            LeanTween.cancel(targetPiece.View.FreezeIndicator);
            GameObject.Destroy(targetPiece.View.FreezeIndicator);
            targetPiece.View.FreezeIndicator = null;
        }
    }

    private Sprite CreateBlurryCircleSprite(int diameter)
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
