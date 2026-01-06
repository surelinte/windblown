using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GlobalLogic : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup; //UI menu object
    private string hideOnScene = SceneRegistry.MainMenuScene;

    private void Awake()
    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        // Apply immediately for the current scene
        OnActiveSceneChanged(default, SceneManager.GetActiveScene());
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        bool shouldHide = newScene.name == hideOnScene;
        SetVisible(!shouldHide);
    }

    private void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}
