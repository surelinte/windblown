using UnityEngine;
using UnityEngine.UI;

public class UIGraphicProbe : MonoBehaviour
{
    void Start()
    {
        var graphics = FindObjectsOfType<Graphic>(true);
        int raycastOn = 0;

        Debug.Log($"[UIGraphicProbe] Graphics found: {graphics.Length}");

        foreach (var g in graphics)
        {
            if (!g.gameObject.activeInHierarchy) continue;

            bool target = g.raycastTarget;
            var cg = g.GetComponentInParent<CanvasGroup>(true);

            bool blockedByCanvasGroup = cg != null && (!cg.interactable || !cg.blocksRaycasts || cg.alpha == 0f && cg.blocksRaycasts == false);

            if (target) raycastOn++;

            // Log only the important “why not raycastable” cases
            if (!target || (cg != null && !cg.blocksRaycasts))
            {
                Debug.Log($"[UIGraphicProbe] '{g.gameObject.name}' type={g.GetType().Name} raycastTarget={target} " +
                          $"CanvasGroupBlocks={(cg != null ? cg.blocksRaycasts.ToString() : "n/a")} " +
                          $"active={g.gameObject.activeInHierarchy}");
            }
        }

        Debug.Log($"[UIGraphicProbe] raycastTarget ON count: {raycastOn}");
    }
}
