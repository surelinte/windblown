#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class PlayFromBootstrap
{
    static PlayFromBootstrap()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                var currentScene = EditorSceneManager.GetActiveScene();
                if (currentScene.name != SceneRegistry.BootstrapScene)
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(
                        $"Assets/Scenes/Core/{SceneRegistry.BootstrapScene}.unity"
                    );
                }
            }
        };
    }
}
#endif
