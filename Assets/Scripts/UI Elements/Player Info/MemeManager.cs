using UnityEngine;
using System.Collections.Generic;

public class MemeManager
{
    private static MemeManager instance;
    public static MemeManager Instance => instance ??= new MemeManager();

    private Dictionary<Movies, List<Sprite>> memeDictionary;

    private MemeManager()
    {
        LoadMemes();
    }

    private void LoadMemes()
    {
        memeDictionary = new Dictionary<Movies, List<Sprite>>();
       
        foreach (Movies movie in System.Enum.GetValues(typeof(Movies)))
        {
            string path = $"Memes/{movie}";
            Sprite[] memes = Resources.LoadAll<Sprite>(path);
            
            if (memes.Length > 0)
            {
                memeDictionary[movie] = new List<Sprite>(memes);
            }
            else
            {
                Debug.LogWarning($"Memes for {movie} not found at {path}");
            }
        }
    }

    public Sprite GetMemeForMovie(Movies movie)
    {
        if (memeDictionary.TryGetValue(movie, out List<Sprite> memes) && memes.Count > 0)
        {
            int randomIndex = Random.Range(0, memes.Count);
            return memes[randomIndex];
            
        }
        
        Debug.LogWarning($"No meme found for movie: {movie}");
        return null;
    }
}
