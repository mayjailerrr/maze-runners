using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] LeaderCardSelectionMenu fidel;
    [SerializeField] LeaderCardSelectionMenu batista;
    public static bool AISelected = true;

    public void Play()
    {
        Player.Reset();
        if (fidel.CheckStartGame() && batista.CheckStartGame() && AISelected) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Exit()
    {
        Debug.Log("Quiting...");
        Application.Quit();
    }
    public void SinglePlayer()
    {
        AISelected = false;
    }
    public void Multiplayer()
    {
        AISelected = true;
    }
}
