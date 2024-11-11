using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovieSelectionMenu : MonoBehaviour
{
    public string movie = "";
    [SerializeField] TMP_InputField input;
    [SerializeField] string faction;

    private void Update()
    {
        input.gameObject.SetActive(Player.movies.Count > 4);
    }

    public void SaveInfo()
    {
        PlayerPrefs.SetString(faction  + " movie", movie);
    }

    public void SelectChihiro()
    {
        movie = "Spirited Away";
    }
    public void SelectPonyo()
    {
        movie = "Ponyo";
    }
    public void SelectMovingCastle()
    {
        movie = "Howl's Moving Castle";
    }
    public void SelectMononoke()
    {
        movie = "Mononoke Princess";
    }

    public void SelectCreated()
    {
        if (input.text.Length == 0) return;

        if (Player.movies.ContainsKey(input.text.Trim()))
        {
            if (Player.movies[input.text].Faction == faction)
            {
                movie = input.text;
                input.text = "";
                input.placeholder.GetComponent<TMP_Text>().text = "Wanted movie:";
            }
            else
            {
                string name = input.text;
                input.text = "";
                input.placeholder.GetComponent<TMP_Text>().text = "These theme '" + name + "' belongs to other player.";
            }
        }
        else
        {
            string name = input.text;
            input.text = "";
            input.placeholder.GetComponent<TMP_Text>().text = "The theme '" + name + "' doesn't exist.";
        }
    }

    public bool CheckStartGame()
    {
        if (movie != "")
        {
            movie = "";
            return true;
        }
        else return false;
    }

    public void SelectAsEnemy()
    {
        PlayerPrefs.SetString("AI", faction);
        Menu.AISelected = true;
    }
}
