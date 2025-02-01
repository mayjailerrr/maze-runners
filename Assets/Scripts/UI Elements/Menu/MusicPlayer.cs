using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] music;
    public AudioSource source; 

    private Coroutine musicCoroutine;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        musicCoroutine = StartCoroutine(PlayAudioClips());
    }

    IEnumerator PlayAudioClips()
    {
        while (true)
        {
            source.clip = music[Random.Range(0, music.Length)];
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
        }
    }

    public void PauseMusic()
    {
        if (source.isPlaying)
        {
            source.Pause();
        }
    }

    public void StopMusic()
    {
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
            musicCoroutine = null;
        }
        source.Stop();
    }

    public void PlayClip(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void ResumeMusic()
    {
        if (!source.isPlaying)
        {
            source.UnPause();
        }
    }
}
