using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public Image transitionMask; 
    public float transitionDuration = 3f;

    private void Start()
    {
        if (transitionMask != null)
        {
            transitionMask.gameObject.SetActive(false); 
        }
    }

    public void StartTransition(System.Action onTransitionComplete)
    {
        if (transitionMask != null)
        {
            transitionMask.gameObject.SetActive(true);
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
        float initialSize = 0f;
        float targetSize = Mathf.Max(Screen.width, Screen.height) * 2f;

        transitionMask.rectTransform.sizeDelta = Vector2.zero;
        transitionMask.rectTransform.anchoredPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float size = Mathf.Lerp(initialSize, targetSize, elapsedTime / transitionDuration);
            transitionMask.rectTransform.sizeDelta = new Vector2(size, size);

            yield return null;
        }

        onTransitionComplete?.Invoke();

        yield return new WaitForSeconds(0.5f);

        transitionMask.gameObject.SetActive(false);
    }
}
