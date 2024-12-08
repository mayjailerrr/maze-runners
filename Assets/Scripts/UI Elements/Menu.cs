using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static bool AISelected = true;
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Exit()
    {
        Debug.Log("Saliendo...");
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
