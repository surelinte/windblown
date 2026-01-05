using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIRaycastDebugger : MonoBehaviour
{
    private PointerEventData ped;
    private readonly List<RaycastResult> results = new();

    void Update()
    {
        if (EventSystem.current == null)
        {
            Debug.Log("UI Raycast: No EventSystem.current");
            return;
        }

        if (Mouse.current == null)
        {
            Debug.Log("UI Raycast: No Mouse detected (are you on mobile/controller?)");
            return;
        }

        ped ??= new PointerEventData(EventSystem.current);
        ped.position = Mouse.current.position.ReadValue();

        results.Clear();
        EventSystem.current.RaycastAll(ped, results);

        Debug.Log(results.Count == 0
            ? "UI Raycast: NOTHING"
            : $"UI Raycast TOP: {results[0].gameObject.name} (hits: {results.Count})");
    }
}
