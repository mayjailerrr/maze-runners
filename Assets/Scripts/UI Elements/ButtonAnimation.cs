using UnityEngine;
using UnityEngine.EventSystems;
using MazeRunners;

public class ButtonAnimatorController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.Play("PlayVideo"); // Switch to the video animation state
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.Play("IdleGIF"); // Switch back to the GIF animation state
    }
}
