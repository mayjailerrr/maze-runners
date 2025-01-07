using UnityEngine;
using System.Collections;

public class PieceView : MonoBehaviour
{
    private Animator animator;
    private Vector2 lastDirection = Vector2.right;
    private RectTransform rectTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in PieceView.");
        }
        Debug.Log($"PieceView is child of {transform.parent.name}");

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

    public void MoveTo(Vector3 targetPosition)
    {
        StartCoroutine(MoveSmoothly(targetPosition));
    }

    private IEnumerator MoveSmoothly(Vector3 targetPosition)
    {
        float speed = 5f; 
        while (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) > 0.01f)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition; 
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
