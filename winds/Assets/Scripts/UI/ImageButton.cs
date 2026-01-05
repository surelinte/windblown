using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ImageButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Color32 normalColor;
    private Button button;

   /* private void Awake()
    {
        textToEdit = GetComponentInChildren<TMP_Text>(true);
        button = GetComponentInParent<Button>();
        if (textToEdit != null) normalColor = textToEdit.color;
    }*/

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button != null && !button.interactable) return;

        transform.localScale = Vector3.one * 0.97f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}