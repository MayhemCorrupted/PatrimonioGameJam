using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip baseTrack;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        PlayBaseMusic();
    }

    public void PlayBaseMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeTrack(baseTrack));
    }

    public void PlayObjectMusic(AudioClip objectTrack)
    {
        if (objectTrack == null) return;
        StopAllCoroutines();
        StartCoroutine(FadeTrack(objectTrack));
    }

    public void PlayPingSound(AudioClip pingClip, int damageLevel)
    {
        if (pingClip == null) return;
        sfxSource.clip = pingClip;

        sfxSource.pitch = 1f - (damageLevel * 0.15f);
        sfxSource.volume = 1f + (damageLevel * 0.2f);

        sfxSource.Play();
    }

    public void StopMusic()
    {
        StopAllCoroutines(); 
        bgmSource.Stop();
    }

    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float fadeTime = 1.5f;

        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }

        bgmSource.clip = newClip;
        bgmSource.Play();

        while (bgmSource.volume < 1f)
        {
            bgmSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}