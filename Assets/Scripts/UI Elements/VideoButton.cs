using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class RenderTextureButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RawImage rawImage;
    public RenderTexture normalTexture;
    public RenderTexture hoverTexture;
    public VideoPlayer videoPlayer; 
    public MusicPlayer musicPlayer; 

    private static RenderTextureButton currentHoveredButton;

    void Start()
    {
        if (rawImage == null)
            rawImage = GetComponent<RawImage>();

        if (musicPlayer == null)
            musicPlayer = FindObjectOfType<MusicPlayer>();

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false; 
            videoPlayer.SetDirectAudioMute(0, true);
        }

        rawImage.texture = normalTexture;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentHoveredButton != null && currentHoveredButton != this)
        {
            currentHoveredButton.PauseVideo();
            currentHoveredButton.MuteVideo();
        }

        currentHoveredButton = this;

        rawImage.texture = hoverTexture;
        musicPlayer.PauseMusic();
        UnmuteVideo();
        PlayVideo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentHoveredButton == this)
        {
            currentHoveredButton = null;
        }

        PauseVideo();
        MuteVideo();
        musicPlayer.ResumeMusic();
        rawImage.texture = normalTexture;
    }

    private void PlayVideo()
    {
        if (videoPlayer != null && !videoPlayer.isPlaying)
        {
            videoPlayer.Play(); 
        }
    }

    private void PauseVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    private void UnmuteVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.SetDirectAudioMute(0, false);
        }
    }

    private void MuteVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.SetDirectAudioMute(0, true);
        }
    }
}
