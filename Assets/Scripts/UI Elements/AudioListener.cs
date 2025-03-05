using UnityEngine;

public class AudioListener : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource minigameAudioSource;
    public AudioClip minigameBackgroundMusic;

    public AudioClip trapSound;
    public AudioClip collectibleSound;

    public AudioClip patternIlluminateSound;
    public AudioClip patternButtonSound;
    public AudioClip memoryButtonSound;
    public AudioClip memoryPairFoundSound;
    public AudioClip minigameWinSound;
    public AudioClip minigameFailSound;

    private void OnEnable()
    {
        GameEvents.OnTrapTriggered += PlayTrapSound;
        GameEvents.OnCollectiblePicked += PlayCollectibleSound;

        GameEvents.OnPatternIlluminate += PlayPatternIlluminateSound;
        GameEvents.OnPatternButtonPressed += PlayPatternButtonSound;
        GameEvents.OnMemoryButtonPressed += PlayMemoryButtonSound;
        GameEvents.OnMemoryPairFound += PlayMemoryPairFoundSound;
        GameEvents.OnMinigameEnd += PlayMinigameEndSound;
    
        GameEvents.OnMinigameStarted += PauseBackgroundMusic;
        GameEvents.OnMinigameEnded += ResumeBackgroundMusic;
    }

    private void OnDisable()
    {
        GameEvents.OnTrapTriggered -= PlayTrapSound;
        GameEvents.OnCollectiblePicked -= PlayCollectibleSound;

        GameEvents.OnPatternIlluminate -= PlayPatternIlluminateSound;
        GameEvents.OnPatternButtonPressed -= PlayPatternButtonSound;
        GameEvents.OnMemoryButtonPressed -= PlayMemoryButtonSound;
        GameEvents.OnMemoryPairFound -= PlayMemoryPairFoundSound;
        GameEvents.OnMinigameEnd -= PlayMinigameEndSound;
    
        GameEvents.OnMinigameStarted -= PauseBackgroundMusic;
        GameEvents.OnMinigameEnded -= ResumeBackgroundMusic;
    }

    private void PlayTrapSound()
    {
        if (trapSound != null)
            minigameAudioSource.PlayOneShot(trapSound);
    }

    private void PlayCollectibleSound()
    {
        if (collectibleSound != null)
            minigameAudioSource.PlayOneShot(collectibleSound);
    }

    private void PlayPatternIlluminateSound()
    {
        if (patternIlluminateSound != null)
            minigameAudioSource.PlayOneShot(patternIlluminateSound);
    }

    private void PlayPatternButtonSound()
    {
        if (patternButtonSound != null)
            minigameAudioSource.PlayOneShot(patternButtonSound);
    }

    private void PlayMemoryButtonSound()
    {
        if (memoryButtonSound != null)
            minigameAudioSource.PlayOneShot(memoryButtonSound);
    }

    private void PlayMemoryPairFoundSound()
    {
        if (memoryPairFoundSound != null)
            minigameAudioSource.PlayOneShot(memoryPairFoundSound);
    }

    private void PlayMinigameEndSound(bool success)
    {
        if (success && minigameWinSound != null)
            minigameAudioSource.PlayOneShot(minigameWinSound);
        else if (!success && minigameFailSound != null)
            minigameAudioSource.PlayOneShot(minigameFailSound);
    }

    private void PauseBackgroundMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }

        if (minigameBackgroundMusic != null && minigameAudioSource != null)
        {
            minigameAudioSource.clip = minigameBackgroundMusic;
            minigameAudioSource.loop = true;
            minigameAudioSource.Play();
        }
    }

    private void ResumeBackgroundMusic()
    {
        if (minigameAudioSource.isPlaying)
        {
            minigameAudioSource.Stop();
        }

        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
}
