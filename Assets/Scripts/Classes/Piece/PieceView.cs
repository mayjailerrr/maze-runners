using UnityEngine;
using System.Collections;

public class PieceView : MonoBehaviour
{
    private Animator animator;
    private Vector2 lastDirection = Vector2.right;
    private RectTransform rectTransform;
    private float moveSpeed = 2f;
    public float moveDuration = 1.0f; 

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

        while (timeElapsed < duration)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.position = targetPosition;
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
        //to-do: activate animation or visual effect
        Debug.Log("Shield effect activated!");
    }

    public void HideShield()
    {
        //to-do: deactivate animation or visual effect
        Debug.Log("Shield effect deactivated!");
    }

}
