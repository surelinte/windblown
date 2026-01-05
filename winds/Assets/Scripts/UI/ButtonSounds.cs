using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour, IPointerClickHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null && !button.interactable)
            return;

        UISoundPlayer.Instance?.PlayClick();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && !button.interactable)
            return;
        
        UISoundPlayer.Instance?.PlayHover();
    }

}