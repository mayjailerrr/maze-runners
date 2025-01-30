using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndGameManager : MonoBehaviour
{
    public Image blackoutImage;
    public RectTransform memeImage;
    public TextMeshProUGUI statsText;

    private void Start()
    {
        // Ensure initial states are set
        memeImage.localScale = Vector3.zero; // Hide meme
        statsText.text = ""; // Clear text
        blackoutImage.color = new Color(blackoutImage.color.r, blackoutImage.color.g, blackoutImage.color.b, 0); // Fully transparent
    }

    public void EndGame(Player winner)
    {
        Debug.Log($"Player {winner.ID + 1} wins the game!");
        StartCoroutine(ShowEndGameSequence(winner));
    }

    private IEnumerator ShowEndGameSequence(Player winner)
    {
        // Step 1: Darken the screen
        yield return StartCoroutine(FadeToBlack());

        // Step 2: Animate the meme zooming in
        yield return StartCoroutine(ZoomInMeme());

        // Step 3: Move meme to the left
        yield return StartCoroutine(MoveMemeToLeft());

        // Step 4: Type out the stats
        yield return StartCoroutine(DisplayStats(winner));
    }

    private IEnumerator FadeToBlack()
    {
        Color color = blackoutImage.color;
        float duration = 2f; // 2 seconds to fade
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / duration);
            blackoutImage.color = color;
            yield return null;
        }
    }

    private IEnumerator ZoomInMeme()
    {
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * 1.5f;
        float duration = 1.5f;
        float elapsed = 0f;
        memeImage.localScale = initialScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            memeImage.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            yield return null;
        }
    }

    private IEnumerator MoveMemeToLeft()
    {
        Vector3 targetPosition = new Vector3(-Screen.width * 0.4f, memeImage.anchoredPosition.y, 0);
        Vector3 startPosition = memeImage.anchoredPosition;
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            memeImage.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            yield return null;
        }
    }

    private IEnumerator DisplayStats(Player winner)
    {
        //string statsMessage = $"Player {winner.ID + 1} Wins!\nScore: {winner.Score}\nMoves: {winner.Moves}\nCollectibles: {winner.Collectibles}";
        string statsMessage = $"Player {winner.ID + 1} Wins!\nScore:";
        statsText.text = "";

        foreach (char c in statsMessage)
        {
            statsText.text += c;
            yield return new WaitForSeconds(0.05f); // Typing speed
        }
    }
}
