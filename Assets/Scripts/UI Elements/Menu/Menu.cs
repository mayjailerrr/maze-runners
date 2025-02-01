
using UnityEngine;


public class Menu : MonoBehaviour
{
    public static bool AISelected = true;

    public void Exit()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }
}
