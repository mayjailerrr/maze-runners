using System.Collections;
using UnityEngine;
using MazeRunners;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] music;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(PlayAudioClips());
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

    public void ResumeMusic()
    {
        if (!source.isPlaying)
        {
            source.UnPause();
        }
    }
}
