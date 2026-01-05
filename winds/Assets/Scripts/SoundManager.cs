using UnityEngine;
using UnityEngine.UI;

public sealed class SoundManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button soundButton;
    [SerializeField] private Image soundButtonImage;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    [Header("State")]
    [SerializeField] private bool defaultSoundOn = true;

    private const string PrefKey = "SoundOn";
    private bool isOn;

    private void Awake()
    {
        if (!soundButton) soundButton = GetComponent<Button>();
        if (!soundButtonImage && soundButton) soundButtonImage = soundButton.GetComponent<Image>();

        soundButton.onClick.AddListener(Toggle);
    }

    private void Start()
    {
        // Load saved setting (fallback to default)
        isOn = PlayerPrefs.GetInt(PrefKey, defaultSoundOn ? 1 : 0) == 1;
        Apply(isOn, save: false);
    }

    private void Toggle()
    {
        Apply(!isOn, save: true);
    }

    private void Apply(bool enabled, bool save)
    {
        isOn = enabled;

        if (soundButtonImage)
            soundButtonImage.sprite = isOn ? soundOnSprite : soundOffSprite;

        // If you only have UISoundPlayer for now:
        // Decide what "sound off" means:
        // - set volume to 0 (keeps clips playable)
        // - or set AudioListener.pause / AudioListener.volume
        UISoundPlayer.Instance?.SetVolume(isOn ? 1f : 0f);

        // If you later add a full AudioManager (music + sfx), call that instead:
        // AudioManager.Instance?.SetMasterVolume(isOn ? 1f : 0f);

        if (save)
        {
            PlayerPrefs.SetInt(PrefKey, isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    private void OnDestroy()
    {
        if (soundButton)
            soundButton.onClick.RemoveListener(Toggle);
    }
}
