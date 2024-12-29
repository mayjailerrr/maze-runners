using UnityEngine;

public class PieceView : MonoBehaviour
{
    private Animator animator;
    private Vector2 lastDirection = Vector2.right; 
    private Vector3 lastPosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    public void UpdateAnimation(Vector2 direction, bool isMoving)
    {
        if (isMoving)
        {
            lastDirection = direction; 
        }

        float speed = isMoving ? 1f : 0f;

        animator.SetFloat("Direction", direction.x); 
        animator.SetFloat("Speed", speed);        
    }

    public void SyncPosition(Vector3 targetPosition)
    {
        lastPosition = targetPosition;
        transform.position = targetPosition;
    }

    public Vector3 GetLastPosition()
    {
        return lastPosition;
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }
}
