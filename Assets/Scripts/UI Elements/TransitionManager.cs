using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public Image transitionMask;
    private float transitionDuration = 4f;

    private void Start()
    {
        if (transitionMask != null)
        {
            transitionMask.gameObject.SetActive(false); 
        }
    }

    public void CheckAndStartTransition(System.Action onTransitionComplete)
    {
        StartCoroutine(WaitForCanvasAndStartTransition(onTransitionComplete));
    }

    private IEnumerator WaitForCanvasAndStartTransition(System.Action onTransitionComplete)
    {
        while (transitionMask == null || transitionMask.canvas == null || !transitionMask.canvas.gameObject.activeSelf)
        {
            yield return null;
        }

        transitionMask.gameObject.SetActive(true);

        if (transitionMask.GetComponent<Mask>() == null)
        {
            transitionMask.gameObject.AddComponent<Mask>();
        }

        StartTransition(onTransitionComplete);
    }

    private void StartTransition(System.Action onTransitionComplete)
    {
        if (transitionMask != null)
        {
            StartCoroutine(Transition(onTransitionComplete));
        }
        else
        {
            Debug.LogWarning("No transition mask assigned!");
        }
    }

    private IEnumerator Transition(System.Action onTransitionComplete)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float fillValue = Mathf.SmoothStep(1f, 0f, elapsedTime / transitionDuration); 
            transitionMask.fillAmount = fillValue;
            yield return null;
        }

        transitionMask.gameObject.SetActive(false);

        onTransitionComplete?.Invoke();
    }
}
