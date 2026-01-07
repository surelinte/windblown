using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GlobalLogic : MonoBehaviour
{
    [SerializeField] public CanvasGroup canvasGroup; //UI menu object
    private string hideOnScene = SceneRegistry.MainMenuScene;
    public static UI_GlobalLogic Instance { get; private set; }
    private bool _sceneAllowsUi = true;
    private bool _isLocked = false;
    private void Awake()

    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
        EvaluateScene(newScene);
    }

    private void EvaluateScene(Scene scene)
    {
        _sceneAllowsUi = scene.name != hideOnScene;
        Apply();
    }

    /*private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        bool shouldHide = newScene.name == hideOnScene;
        SetVisible(!shouldHide);
    }*/

    public void LockUi()
    {
        _isLocked = true;
        Apply();
    }

    public void UnlockUi()
    {
        _isLocked = false;
        Apply();
    }

    private void Apply()
    {
        if (!canvasGroup) return;

        bool visible = _sceneAllowsUi && !_isLocked;

        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    /*private void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

     public void SetUiEnabled(bool enabled)
    {
        if (!canvasGroup) return;

        canvasGroup.interactable = enabled;
        canvasGroup.blocksRaycasts = enabled;
        canvasGroup.alpha = enabled ? 1f : 0.9f;
    }

    public void LockUi()   => SetUiEnabled(false);
    public void UnlockUi() => SetUiEnabled(true);*/
}
