using UnityEngine;
using UnityEngine.EventSystems;

public class UIRaycasterProbe : MonoBehaviour
{
    void Start()
    {
        var raycasters = FindObjectsOfType<BaseRaycaster>(true);
        Debug.Log($"[UIRaycasterProbe] BaseRaycasters found: {raycasters.Length}");

        foreach (var r in raycasters)
            Debug.Log($"[UIRaycasterProbe] Raycaster: {r.GetType().Name} on '{r.gameObject.name}', enabled={r.enabled}, activeInHierarchy={r.gameObject.activeInHierarchy}");
    }
}
