using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieSelectionView : MonoBehaviour
{   
    public Button[] movieButtons;

    private void Update()
    {
        // Deshabilita los botones si se alcanzó el límite de jugadores
        bool shouldDisable = GameManager.Instance.PlayerCount >= GameManager.MaxPlayers;
        foreach (var button in movieButtons)
        {
            button.interactable = !shouldDisable;
        }
    }

}
