using UnityEngine;
using UnityEngine.Video;

public class VideoTest : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("No VideoPlayer assigned!");
            return;
        }

        videoPlayer.errorReceived += OnVideoError;
        videoPlayer.prepareCompleted += OnVideoPrepared;

        videoPlayer.Prepare();
        Debug.Log("Preparing video...");
    }

    void OnVideoPrepared(VideoPlayer source)
    {
        Debug.Log("Video prepared. Playing...");
        videoPlayer.Play();
    }

    void OnVideoError(VideoPlayer source, string message)
    {
        Debug.LogError("Video Error: " + message);
    }
}
