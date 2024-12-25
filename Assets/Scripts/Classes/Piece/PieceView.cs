using MazeRunners;
using UnityEngine;

public class PieceView : MonoBehaviour
{
    public void AnimateMove(Vector3 newPosition)
    {
        // to-do: here i will add the animations
        transform.position = newPosition;
    }
}