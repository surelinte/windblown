using UnityEngine;
using System;

public class UISoundPlayer : MonoBehaviour
{
    public static UISoundPlayer Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private bool defaultSfxOn = true;
    [SerializeField] private string prefsKey = "UiSfxOn";

    public bool SfxEnabled { get; private set; }
    public event Action<bool> OnSfxEnabledChanged;

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
        SfxEnabled = PlayerPrefs.GetInt(prefsKey, defaultSfxOn ? 1 : 0) == 1;
        ApplySfxState(save: false, notify: true);
    }
    private void ApplySfxState(bool save, bool notify)
    {
        if (audioSource)
            audioSource.mute = !SfxEnabled;

        if (save)
        {
            PlayerPrefs.SetInt(prefsKey, SfxEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        if (notify)
            OnSfxEnabledChanged?.Invoke(SfxEnabled);
    }

    public void ToggleSfx() => SetSfxEnabled(!SfxEnabled);

    public void SetSfxEnabled(bool enabled)
    {
        if (SfxEnabled == enabled)
            return;

        SfxEnabled = enabled;
        ApplySfxState(save: true, notify: true);
    }

    public void SetVolume(float volume)
    {
        if (audioSource)
            audioSource.volume = Mathf.Clamp01(volume);
    }
    private void PlayOneShot(AudioClip clip)
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
    }

}
