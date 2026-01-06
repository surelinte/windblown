using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Bootstrapper : MonoBehaviour
{
    private static bool s_Booted;
    [SerializeField] private int targetFps = 60;
    [SerializeField] private LoadingVisual screenFader;
    [SerializeField] private float splashHoldTime = 2f;

    private IEnumerator Start()
    {
        // Prevent double boot (useful with Domain Reload disabled)
        if (s_Booted)
        {
            Destroy(gameObject);
            yield break;
        }
        s_Booted = true;

        if (targetFps > 0)
            Application.targetFrameRate = targetFps;

        //DontDestroyOnLoad(gameObject);

        ApplyAppConfig();

        if (screenFader != null)
        {
            screenFader.SetAlpha(0f);
            yield return screenFader.FadeIn();
            
            yield return new WaitForSecondsRealtime(splashHoldTime);
        }

        // Load persistent layers
        yield return LoadAdditiveIfNeeded(SceneRegistry.PersistentScene);
        yield return LoadAdditiveIfNeeded(SceneRegistry.UI_GlobalScene);

        // Decide which scene to start with (for now: MainMenu)
        string firstScene = DetermineFirstScene();

        // Load + activate
        yield return LoadAdditiveIfNeeded(firstScene);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(firstScene));

        /*// Unload Bootstrap scene
        Scene bootstrapScene = gameObject.scene;
        Destroy(gameObject);
        SceneManager.UnloadSceneAsync(bootstrapScene);*/

        // Fade out the bootstrap overlay to reveal the first scene
        if (screenFader != null)
            yield return screenFader.FadeOut();

        // Unload Bootstrap scene (no need to DontDestroyOnLoad this object)
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    private void ApplyAppConfig()
    {
        if (targetFps > 0)
            Application.targetFrameRate = targetFps;

        // Optional knobs:
        // Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // Input.multiTouchEnabled = false;
    }

    private static string DetermineFirstScene()
    {
        // Later you can add:
        // return SaveExists ? SceneRegistry.MapScene : SceneRegistry.MainMenuScene;
        return SceneRegistry.MainMenuScene;
    }

    private static IEnumerator LoadAdditiveIfNeeded(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
            yield break;

        if (SceneManager.GetSceneByName(sceneName).isLoaded)
            yield break;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (op != null && !op.isDone)
            yield return null;
    }
}