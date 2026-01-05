using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Bootstrapper : MonoBehaviour
{
    private static bool s_Booted;

    [Header("App")]
    [SerializeField] private int targetFps = 60;

    private IEnumerator Start()
    {
        // Prevent double boot (useful with Domain Reload disabled)
        if (s_Booted)
        {
            Destroy(gameObject);
            yield break;
        }
        s_Booted = true;

        DontDestroyOnLoad(gameObject);

        ApplyAppConfig();

        // Load persistent layers
        yield return LoadAdditiveIfNeeded(SceneRegistry.PersistentScene);
        yield return LoadAdditiveIfNeeded(SceneRegistry.UI_GlobalScene);

        // Decide which scene to start with (for now: MainMenu)
        string firstScene = DetermineFirstScene();

        // Load + activate
        yield return LoadAdditiveIfNeeded(firstScene);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(firstScene));

        // Unload Bootstrap scene
        Scene bootstrapScene = gameObject.scene;
        Destroy(gameObject);
        SceneManager.UnloadSceneAsync(bootstrapScene);
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