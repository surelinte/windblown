using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TextButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private TMP_Text textToEdit;
    private Color32 normalColor;
    private Button button;

    private void Awake()
    {
        textToEdit = GetComponentInChildren<TMP_Text>(true);
        button = GetComponentInParent<Button>();
        if (textToEdit != null) normalColor = textToEdit.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button != null && !button.interactable) return;

        if (textToEdit != null && button != null)
            textToEdit.color = button.colors.pressedColor;

        transform.localScale = Vector3.one * 0.97f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (textToEdit != null) textToEdit.color = normalColor;
        transform.localScale = Vector3.one;
    }
}