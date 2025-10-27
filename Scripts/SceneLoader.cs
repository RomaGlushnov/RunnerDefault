using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    private float delay = 0.2f;
    // нопка "PLAY" в меню
    public void StartGame() {
    Time.timeScale = 1f;

        SceneManager.LoadScene(1);

    }
   // Restart button
    public void RestartGame() {
        Time.timeScale = 1f;
        // ѕерезагружаем текущую активную сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //Main menu
    public void MainMenu() {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);

    }
}