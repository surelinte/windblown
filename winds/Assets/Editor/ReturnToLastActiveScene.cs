#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class ReturnToLastActiveScene
{
    private static string lastActiveScene;

    static ReturnToLastActiveScene()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private static void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        if (EditorApplication.isPlaying)
            lastActiveScene = newScene.path;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode &&
            !string.IsNullOrEmpty(lastActiveScene))
        {
            EditorSceneManager.OpenScene(lastActiveScene);
        }
    }
}
#endif
