using UnityEngine;
using System.Collections.Generic;

public class MemeManager
{
    private static MemeManager instance;
    public static MemeManager Instance => instance ??= new MemeManager();

    private Dictionary<Movies, Sprite> memeDictionary;

    private MemeManager()
    {
        LoadMemes();
    }

    private void LoadMemes()
    {
        memeDictionary = new Dictionary<Movies, Sprite>();
        foreach (Movies movie in System.Enum.GetValues(typeof(Movies)))
        {
            string path = $"Memes/{movie}";
            Sprite memeSprite = Resources.Load<Sprite>(path);
            if (memeSprite != null)
            {
                memeDictionary[movie] = memeSprite;
            }
            else
            {
                Debug.LogWarning($"Meme for {movie} not found at {path}");
            }
        }
    }

    public Sprite GetMemeForMovie(Movies movie)
    {
        if (memeDictionary.TryGetValue(movie, out Sprite meme))
        {
            return meme;
        }
        Debug.LogWarning($"No meme found for movie: {movie}");
        return null;
    }
}
