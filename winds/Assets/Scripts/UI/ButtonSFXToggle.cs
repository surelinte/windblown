using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ButtonSFXToggle : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite sfxOnSprite;
    [SerializeField] private Sprite sfxOffSprite;

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
        if (UISoundPlayer.Instance != null)
        {
            UISoundPlayer.Instance.OnSfxEnabledChanged += UpdateIcon;
            UpdateIcon(UISoundPlayer.Instance.SfxEnabled);
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
        if (UISoundPlayer.Instance != null)
            UISoundPlayer.Instance.OnSfxEnabledChanged -= UpdateIcon;
    }

    private void OnDestroy()
    {
        if (button)
            button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        UISoundPlayer.Instance?.ToggleSfx();
        // Icon updates via event
    }

    private void UpdateIcon(bool sfxEnabled)
    {
        if (!icon) return;
        icon.sprite = sfxEnabled ? sfxOnSprite : sfxOffSprite;
    }
}
