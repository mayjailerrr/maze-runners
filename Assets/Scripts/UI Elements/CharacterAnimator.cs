using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        PlayAnimation();
    }

    void PlayAnimation()
    {
        animator.Play("CharacterAnimation");
    }
}
