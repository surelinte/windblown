using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ButtonMusicToggle : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite MusicOnSprite;
    [SerializeField] private Sprite MusicOffSprite;

    private void Awake()
    {
        if (!button) button = GetComponent<Button>();
        if (!icon && button) icon = button.GetComponent<Image>();

        if (button)
            button.onClick.AddListener(OnClick);
    }

    private void OnEnable()
    {
        // Subscribe + refresh immediately (important when scene loads)
        if (MusicPlayer.Instance != null)
        {
            MusicPlayer.Instance.OnMusicEnabledChanged += UpdateIcon;
            UpdateIcon(MusicPlayer.Instance.MusicEnabled);
        }
        else
        {
            // If Persistent isn't loaded (e.g., you pressed Play from this scene),
            // at least show a default state.
            UpdateIcon(true);
        }
    }

    private void OnDisable()
    {
        if (MusicPlayer.Instance != null)
            MusicPlayer.Instance.OnMusicEnabledChanged -= UpdateIcon;
    }

    private void OnDestroy()
    {
        if (button)
            button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        MusicPlayer.Instance?.ToggleMusic();
        // Icon updates via event
    }

    private void UpdateIcon(bool MusicEnabled)
    {
        if (!icon) return;
        icon.sprite = MusicEnabled ? MusicOnSprite : MusicOffSprite;
    }
}
