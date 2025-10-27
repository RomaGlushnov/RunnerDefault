using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class PlayFromMenu {
    // Путь к твоему главному меню. Убедись, что он правильный!
    private const string FirstScenePath = "Assets/Scenes/Menu.unity";

    // Название кнопки в меню
    private const string MenuPath = "Tools/ЗАПУСК С МЕНЮ (Ctrl+P)";

    static PlayFromMenu() {
        EditorApplication.delayCall += () => {
            Menu.SetChecked(MenuPath, EditorApplication.isPlayingOrWillChangePlaymode);
        };

        EditorApplication.playModeStateChanged += (state) => {
            Menu.SetChecked(MenuPath, state == PlayModeStateChange.ExitingEditMode);
        };
    }

    [MenuItem(MenuPath)]
    private static void Play() {
        if (EditorApplication.isPlaying) {
            EditorApplication.isPlaying = false;
            return;
        }

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(FirstScenePath);
        EditorApplication.isPlaying = true;
    }
}