using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel; 
    public Image tutorialImage;
    public Sprite[] tutorialSprites;
    private int currentIndex = 0;

    void Start()
    {
        tutorialPanel.SetActive(false);
    }

    public void OpenTutorial()
    {
        currentIndex = 0;
        tutorialPanel.SetActive(true);
        ShowImage(currentIndex);
    }

    public void ShowImage(int index)
    {
        tutorialImage.sprite = tutorialSprites[index];
    }

    public void NextImage()
    {
        if (currentIndex < tutorialSprites.Length - 1)
        {
            currentIndex++;
            ShowImage(currentIndex);
        }
    }

    public void PreviousImage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowImage(currentIndex);
        }
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}
