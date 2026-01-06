using UnityEngine;
using System;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip MainTheme;
    [SerializeField] private AudioClip GameTheme;
    [SerializeField] private bool defaultMusicOn = true;
    [SerializeField] private string prefsKey = "GlobalMusicOn";

    public bool MusicEnabled { get; private set; }
    public event Action<bool> OnMusicEnabledChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Enforce singleton
            return;
        }

        Instance = this;
        
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        MusicEnabled = PlayerPrefs.GetInt(prefsKey, defaultMusicOn ? 1 : 0) == 1;
        ApplyMusicState(save: false, notify: true);
        // Optional: start menu music immediately
        PlayMainTheme();
    }
    
    public void PlayMainTheme() => PlayMusic(MainTheme);
    public void PlayGameTheme() => PlayMusic(GameTheme);
    
    private void ApplyMusicState(bool save, bool notify)
    {
        if (audioSource)
            audioSource.mute = !MusicEnabled;

        if (save)
        {
            PlayerPrefs.SetInt(prefsKey, MusicEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        if (notify)
            OnMusicEnabledChanged?.Invoke(MusicEnabled);
    }

    public void ToggleMusic() => SetMusicEnabled(!MusicEnabled);

    public void SetMusicEnabled(bool enabled)
    {
        if (MusicEnabled == enabled)
            return;

        MusicEnabled = enabled;
        ApplyMusicState(save: true, notify: true);
    }

    public void SetVolume(float volume)
    {
        if (audioSource)
            audioSource.volume = Mathf.Clamp01(volume);
    }
    /*private void PlayOneShot(AudioClip clip)
    {
        if (!audioSource || clip == null) return;
        audioSource.PlayOneShot(clip);
    }
    public void PlayHover()
    {
        if (hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
    }

    public void PlayClick()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }*/

    public void PlayMusic(AudioClip clip, bool restartIfSame = false)
    {
        if (!audioSource || clip == null) return;

        if (!restartIfSame && audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.mute = !MusicEnabled;

        audioSource.Play();
    }

    public void StopMusic()
    {
        if (!audioSource) return;
        audioSource.Stop();
        audioSource.clip = null;
    }

}
