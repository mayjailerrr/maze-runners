using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndGameManager : MonoBehaviour
{
    public Image blackoutImage;
    public RectTransform memeImage;
    public TextMeshProUGUI statsText;
    public Button restartButton;

    private void Start()
    {
        memeImage.localScale = Vector3.zero;
        statsText.text = "";
        blackoutImage.color = new Color(blackoutImage.color.r, blackoutImage.color.g, blackoutImage.color.b, 0);
        restartButton.gameObject.SetActive(false); // Asegúrate de que el botón esté desactivado al inicio
        restartButton.onClick.AddListener(RestartGame);
    }

    public void EndGame(Player winner)
    {
        Debug.Log($"Player {winner.ID + 1} wins the game!");

        float gameEndTime = Time.time;
        float totalPlayTime = gameEndTime - GameManager.Instance.GameStartTime;

        List<Player> allPlayers = GameManager.Instance.Players.Values.ToList();

        var winnerMovie = GameManager.Instance.GetSelectedMovieForPlayer(winner.ID);
        Sprite memeSprite = MemeManager.Instance.GetMemeForMovie(winnerMovie);
        memeImage.GetComponent<Image>().sprite = memeSprite;

        StartCoroutine(EndGameSequence(winner, allPlayers, totalPlayTime));
    }

    private IEnumerator EndGameSequence(Player winner, List<Player> allPlayers, float totalPlayTime)
    {
        yield return ShowEndGameSequence(winner);

        yield return DisplayStats(winner, allPlayers, totalPlayTime);

        restartButton.gameObject.SetActive(true);
    }

    private IEnumerator ShowEndGameSequence(Player winner)
    {
        yield return StartCoroutine(FadeToBlack());

        yield return StartCoroutine(ZoomInMeme());

        yield return StartCoroutine(MoveMemeToLeft());
    }

    private IEnumerator FadeToBlack()
    {
        Color color = blackoutImage.color;
        float duration = 2f;
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
        Vector3 targetPosition = new Vector3(-Screen.width * 0.2f, memeImage.anchoredPosition.y, 0);
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

    private IEnumerator DisplayStats(Player winner, List<Player> allPlayers, float totalPlayTime)
    {
        string fullStatsMessage = $"Player {winner.ID + 1} Wins!\n" +
                                $"Time Played: {totalPlayTime:F2} seconds\n" +
                                $"Moves: {winner.Moves}\n" +
                                $"Traps Triggered: {winner.TrapsTriggered}\n" +
                                $"Abilities Used: {winner.AbilitiesUsed}\n" +
                                $"Collectibles: {string.Join(", ", winner.CollectedObjects.Select(c => c.Name))}\n";

        foreach (var player in allPlayers.Where(p => p != winner))
        {
            string playerStats = $"\nPlayer {player.ID + 1} Stats:\n" +
                                $"Moves: {player.Moves}\n" +
                                $"Traps Triggered: {player.TrapsTriggered}\n" +
                                $"Abilities Used: {player.AbilitiesUsed}\n" +
                                $"Collectibles: {string.Join(", ", player.CollectedObjects.Select(c => c.Name))}\n";

            fullStatsMessage += playerStats;
        }

        statsText.text = "";

        foreach (char c in fullStatsMessage)
        {
            statsText.text += c;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
