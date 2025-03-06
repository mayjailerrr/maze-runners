using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MazeRunners;
using System.Linq;
using System.Collections;
using TMPro;
using System.Text.RegularExpressions;

public class MovieSelectionController : MonoBehaviour
{
    private MovieSelectionModel model;
    public List<Button> movieButtons;
    public TextMeshProUGUI info;

    private List<GameObject> frameImages;

    private void OnEnable()
    {
        if (model == null)
        {
            model = new MovieSelectionModel();
        }

        StartCoroutine(WaitForButtonsAndInitialize());
    }

    private IEnumerator WaitForButtonsAndInitialize()
    {
        while (!movieButtons.Any(button => button.gameObject.activeInHierarchy))
        {
            yield return null;
        }

        InitializeFrames();
    }

    private void InitializeFrames()
    {
        frameImages = new List<GameObject>();

        foreach (var button in movieButtons)
        {
            if (!button.gameObject.activeInHierarchy) continue;

            GameObject frame = button.transform.Find("Frame")?.gameObject;   
            frameImages.Add(frame);
            frame.SetActive(false);
        }
    }

    public void OnMovieButtonClicked(int movieIndex)
    {
        if (!model.CanSelectMovie())
        {
            DisplayInfoMessage("Maximum amount of players has been reached.", false);
            return;
        }

        GameEvents.TriggerPatternIlluminate();
        Movies selectedMovie = (Movies)movieIndex;
        int currentPlayerIndex = GameManager.Instance.GetCurrentPlayerIndex();

        if (!model.IsMovieAvailable(selectedMovie))
        {
            DisplayInfoMessage("This movie is not available now.", false);
            return;
        }

        model.AssignMovieToPlayer(currentPlayerIndex, selectedMovie);
        GameManager.Instance.AssignMovieToPlayer(selectedMovie);

        string movieName = FormatMovieName(selectedMovie.ToString());
        DisplayInfoMessage($"Player {currentPlayerIndex + 1} selected {movieName}.", true);
        HighlightButton(movieIndex);

        GameManager.Instance.NextPlayer();
    }

    private void DisplayInfoMessage(string message, bool isPositive)
    {
        info.text = message;
        info.color = isPositive ? Color.yellow : Color.red;
    }

    private void HighlightButton(int index)
    {
        if (frameImages.Count > index && frameImages[index] != null)
        {
            frameImages[index].SetActive(true); 
        }
    }

    private string FormatMovieName(string movieName)
    {
        return Regex.Replace(movieName, "(\\B[A-Z])", " $1");
    }
}

