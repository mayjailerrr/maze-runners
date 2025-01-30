using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PieceView : MonoBehaviour
{
    private Animator animator;
    private Vector2 lastDirection = Vector2.right;
    private RectTransform rectTransform;

    public GameObject FreezeIndicator { get; set; }
    public GameObject ShieldIndicator { get; set; }
    public BoardView BoardView;
    public Piece Piece { get; set; } 

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

    public void SetVisibility(bool isVisible)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(isVisible);
        }
    }
}
