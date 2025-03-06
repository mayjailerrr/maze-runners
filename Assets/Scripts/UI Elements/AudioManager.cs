using UnityEngine;

public class AudioManager : MonoBehaviour
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

    public AudioClip absorbSound;
    public AudioClip absorbDamageSound;
    public AudioClip cloneSound;
    public AudioClip freezeSound;
    public AudioClip healthDamageSound;
    public AudioClip invisibilitySound;
    public AudioClip rampartBuilderSound;
    public AudioClip shieldSound;
    public AudioClip speedBoostSound;
    public AudioClip teleportSound;
    public AudioClip WallBombSound;
    public AudioClip WallBuilderSound;
    public AudioClip WallDestroyerSound;


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

        GameEvents.OnAbsorbUsed += PlayAbsorbSound;
        GameEvents.OnAbsorbDamageUsed += PlayAbsorbDamageSound;
        GameEvents.OnCloneUsed += PlayCloneSound;
        GameEvents.OnFreezeUsed += PlayFreezeSound;
        GameEvents.OnHealthDamageUsed += PlayHealthDamageSound;
        GameEvents.OnInvisibilityUsed += PlayInvisibilitySound;
        GameEvents.OnRampartBuilderUsed += PlayRampartBuilderSound;
        GameEvents.OnShieldUsed += PlayShieldSound;
        GameEvents.OnSpeedBoostUsed += PlaySpeedBoostSound;
        GameEvents.OnTeleportUsed += PlayTeleportSound;
        GameEvents.OnWallBombUsed += PlayWallBombSound;
        GameEvents.OnWallBuilderUsed += PlayWallBuilderSound;
        GameEvents.OnWallDestroyerUsed += PlayWallDestroyerSound;

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

        GameEvents.OnAbsorbUsed -= PlayAbsorbSound;
        GameEvents.OnAbsorbDamageUsed -= PlayAbsorbDamageSound;
        GameEvents.OnCloneUsed -= PlayCloneSound;
        GameEvents.OnFreezeUsed -= PlayFreezeSound;
        GameEvents.OnHealthDamageUsed -= PlayHealthDamageSound;
        GameEvents.OnInvisibilityUsed -= PlayInvisibilitySound;
        GameEvents.OnRampartBuilderUsed -= PlayRampartBuilderSound;
        GameEvents.OnShieldUsed -= PlayShieldSound;
        GameEvents.OnSpeedBoostUsed -= PlaySpeedBoostSound;
        GameEvents.OnTeleportUsed -= PlayTeleportSound;
        GameEvents.OnWallBombUsed -= PlayWallBombSound;
        GameEvents.OnWallBuilderUsed -= PlayWallBuilderSound;
        GameEvents.OnWallDestroyerUsed -= PlayWallDestroyerSound;
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

    private void PlayAbsorbSound()
    {
        if (absorbSound != null)
            audioSource.PlayOneShot(absorbSound);
    }

    private void PlayAbsorbDamageSound()
    {
        if (absorbDamageSound != null)
            audioSource.PlayOneShot(absorbDamageSound);
    }

    private void PlayCloneSound()
    {
        if (cloneSound != null)
            audioSource.PlayOneShot(cloneSound);
    }

    private void PlayFreezeSound()
    {
        if (freezeSound != null)
            audioSource.PlayOneShot(freezeSound);
    }

    private void PlayHealthDamageSound()
    {
        if (healthDamageSound != null)
            audioSource.PlayOneShot(healthDamageSound);
    }

    private void PlayInvisibilitySound()
    {
        if (invisibilitySound != null)
            audioSource.PlayOneShot(invisibilitySound);
    }

    private void PlayRampartBuilderSound()
    {
        if (rampartBuilderSound != null)
            audioSource.PlayOneShot(rampartBuilderSound);
    }

    private void PlayShieldSound()
    {
        if (shieldSound != null)
            audioSource.PlayOneShot(shieldSound);
    }

    private void PlaySpeedBoostSound()
    {
        if (speedBoostSound != null)
            audioSource.PlayOneShot(speedBoostSound);
    }

    private void PlayTeleportSound()
    {
        if (teleportSound != null)
            audioSource.PlayOneShot(teleportSound);
    }

    private void PlayWallBombSound()
    {
        if (WallBombSound != null)
            audioSource.PlayOneShot(WallBombSound);
    }

    private void PlayWallBuilderSound()
    {
        if (WallBuilderSound != null)
            audioSource.PlayOneShot(WallBuilderSound);
    }

    private void PlayWallDestroyerSound()
    {
        if (WallDestroyerSound != null)
            audioSource.PlayOneShot(WallDestroyerSound);
    }

}
