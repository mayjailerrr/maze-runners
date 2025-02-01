
using UnityEngine;
using UnityEngine.UI;

public class MovieSelectionView : MonoBehaviour
{   
    public Button[] movieButtons;

    private void Update()
    {
        bool shouldDisable = GameManager.Instance.PlayerCount >= 4;
        foreach (var button in movieButtons)
        {
            button.interactable = !shouldDisable;
        }
    }
}
