using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenManager : MonoBehaviour
{
    public Image splashImage;
    public float fadeInDuration = 2f;
    public float displayDuration = 1.5f;
    public float fadeOutDuration = 2f;
    public string nextSceneName = "MainMenu";

    private void Start()
    {
        if (splashImage == null)
        {
            Debug.LogError("Splash Image not assigned!");
            return;
        }

        StartCoroutine(PlaySplashScreenSequence());
    }

    private IEnumerator PlaySplashScreenSequence()
    {
        yield return StartCoroutine(FadeImage(0f, 1f, fadeInDuration));

        yield return new WaitForSeconds(displayDuration);

        yield return StartCoroutine(FadeImage(1f, 0f, fadeOutDuration));

        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        Color color = splashImage.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            splashImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        splashImage.color = color;
    }
}
