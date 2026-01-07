using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//describes how scenes are switching

public sealed class GameFlow : MonoBehaviour 
{
    public static GameFlow Instance { get; private set; }
    public static bool IsTransitioning => Instance != null && Instance._isTransitioning;
    public event Action<bool> OnTransitioningChanged;
    private bool _isTransitioning; //“The game is in a transition state”
    private LoadingVisual loadingVisual; // from UI_Global
    private string _currentModeScene;
    private bool _isSwitching; // “The coroutine is running”
    [SerializeField] private float LoadingTime = 0.5f;

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

      /*  if (!loadingVisual)
         loadingVisual = FindFirstObjectByType<LoadingVisual>(FindObjectsInactive.Include);*/
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
        //double check
        if (_isSwitching) yield break;
        if (string.IsNullOrWhiteSpace(nextScene)) yield break;
        if (_currentModeScene == nextScene) yield break;
        
        SetTransitioning(true);
        
        if (_isSwitching) yield break;
        _isSwitching = true;
        
        /* if (string.IsNullOrWhiteSpace(nextScene) || _currentModeScene == nextScene)
        {
            _isSwitching = false;
            yield break;
        }*/
        
        UI_GlobalLogic.Instance?.LockUi();

        loadingVisual = FindFirstObjectByType<LoadingVisual>(FindObjectsInactive.Include);

        if (loadingVisual != null)
        {
            yield return loadingVisual.FadeIn();
        }

        // Load next mode scene additively
        yield return LoadAdditiveIfNeeded(nextScene);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene)); //active scene

        // Unload previous mode scene (only if it's loaded; never unload Persistent/UI_Global)
        if (!string.IsNullOrEmpty(_currentModeScene))
            yield return UnloadIfLoaded(_currentModeScene);

        _currentModeScene = nextScene;

        loadingVisual = FindFirstObjectByType<LoadingVisual>(FindObjectsInactive.Include); //double-check

        // Pause
        if (LoadingTime > 0f)
            yield return new WaitForSecondsRealtime(LoadingTime);

        // Reveal
        if (loadingVisual != null)
            yield return loadingVisual.FadeOut();

        UI_GlobalLogic.Instance?.UnlockUi();

        SetTransitioning(false);
        _isSwitching = false;
    }

    private void SetTransitioning(bool value)
    {
        if (_isTransitioning == value) return;

        _isTransitioning = value;
        OnTransitioningChanged?.Invoke(_isTransitioning);
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
