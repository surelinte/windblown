using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }

    private LoadingVisual loadingVisual; // from UI_Global
    private string _currentModeScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _currentModeScene = SceneManager.GetActiveScene().name; //define the loaded scene whatever it is

        if (!loadingVisual)
         loadingVisual = FindFirstObjectByType<LoadingVisual>(FindObjectsInactive.Include);
    }

    public void GoToMap()
    {
        StartCoroutine(SwitchTo(SceneRegistry.MapScene));
    }

    public void GoToBattle()
    {
        StartCoroutine(SwitchTo(SceneRegistry.BattleScene));
    }

    public void GoToStory()
    {
        StartCoroutine(SwitchTo(SceneRegistry.StoryScene));
    }
    
    public void GoToMainMenu()
    {
        StartCoroutine(SwitchTo(SceneRegistry.MainMenuScene));
    }

    private IEnumerator SwitchTo(string nextScene)
    {
        if (string.IsNullOrWhiteSpace(nextScene))
            yield break;

        if (_currentModeScene == nextScene)
            yield break;

        // 1) Fade to black
        if (loadingVisual != null)
            yield return loadingVisual.FadeIn();

        // 2) Load next mode scene
        yield return LoadAdditiveIfNeeded(nextScene);

        // 3) Set active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));

        // 4) Unload previous mode scene (never unload Persistent/UI_Global)
        if (!string.IsNullOrEmpty(_currentModeScene))
            yield return UnloadIfLoaded(_currentModeScene);

        _currentModeScene = nextScene;

        // 5) Reveal
        if (loadingVisual != null)
            yield return loadingVisual.FadeOut();
    }

    private static IEnumerator LoadAdditiveIfNeeded(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
            yield break;

        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (op != null && !op.isDone)
            yield return null;
    }

    private static IEnumerator UnloadIfLoaded(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.isLoaded)
            yield break;

        var op = SceneManager.UnloadSceneAsync(sceneName);
        while (op != null && !op.isDone)
            yield return null;
    }
}
