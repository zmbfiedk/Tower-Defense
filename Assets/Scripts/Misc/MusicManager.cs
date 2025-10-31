using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip bossMusic;

    [Header("Playback Settings")]
    [SerializeField] private float initialVolume = 1f;
    [SerializeField, Tooltip("Seconds to crossfade between normal and boss music")]
    private float crossfadeDuration = 1f;

    [Header("Options")]
    [SerializeField, Tooltip("If true, this object won't be destroyed on scene load")]
    private bool persistAcrossScenes = true;

    private AudioSource normalSource;
    private AudioSource bossSource;
    private Coroutine fadeCoroutine;
    private bool isBossMusicActive = false;

    private void Awake()
    {
        if (persistAcrossScenes)
            DontDestroyOnLoad(gameObject);

        normalSource = gameObject.AddComponent<AudioSource>();
        bossSource = gameObject.AddComponent<AudioSource>();

        ConfigureSource(normalSource, normalMusic);
        ConfigureSource(bossSource, bossMusic);

        normalSource.volume = initialVolume;
        bossSource.volume = 0f;
    }

    private void OnEnable()
    {
        WaveChecker.OnBossWave += HandleBossWave;
        EnemyEvents.OnBossDefeated += HandleBossDefeated;
    }

    private void OnDisable()
    {
        // Unsubscribe
        try { WaveChecker.OnBossWave -= HandleBossWave; } catch { }
        try { EnemyEvents.OnBossDefeated -= HandleBossDefeated; } catch { }
    }

    private void Start()
    {
        if (normalSource.clip != null)
        {
            normalSource.Play();
            normalSource.volume = initialVolume;
            isBossMusicActive = false;
        }
        else if (bossSource.clip != null)
        {
            bossSource.Play();
            bossSource.volume = 0f;
            isBossMusicActive = false;
        }
    }

    private void ConfigureSource(AudioSource src, AudioClip clip)
    {
        src.clip = clip;
        src.loop = true;
        src.playOnAwake = false;
        src.spatialBlend = 0f; 
    }

    private void HandleBossWave()
    {
        PlayBossMusic();
    }

    private void HandleBossDefeated()
    {
        PlayNormalMusic();
    }

    public void PlayNormalMusic()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Crossfade(normalSource, bossSource, crossfadeDuration));
        isBossMusicActive = false;
    }

    public void PlayBossMusic()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Crossfade(bossSource, normalSource, crossfadeDuration));
        isBossMusicActive = true;
    }

    private IEnumerator Crossfade(AudioSource toSource, AudioSource fromSource, float duration)
    {
        if (toSource.clip != null && !toSource.isPlaying) toSource.Play();
        if (fromSource.clip != null && !fromSource.isPlaying) fromSource.Play();

        float startTime = Time.time;
        float toStartVol = toSource.volume;
        float fromStartVol = fromSource.volume;

        float targetTo = Mathf.Clamp01(initialVolume);
        float targetFrom = 0f;

        float effectiveDuration = Mathf.Max(0.001f, duration);

        while (Time.time < startTime + effectiveDuration)
        {
            float t = (Time.time - startTime) / effectiveDuration;
            toSource.volume = Mathf.Lerp(toStartVol, targetTo, t);
            fromSource.volume = Mathf.Lerp(fromStartVol, targetFrom, t);
            yield return null;
        }

        toSource.volume = targetTo;
        fromSource.volume = targetFrom;
        if (fromSource.volume <= 0.0001f && fromSource.isPlaying)
            fromSource.Pause();

        fadeCoroutine = null;
    }
}
