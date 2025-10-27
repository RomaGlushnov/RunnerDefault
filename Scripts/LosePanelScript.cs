using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanelScript : MonoBehaviour {
    [SerializeField] Text recordText;
    public void ShowPanel(int scoreFromThisRun, int coinsFromThisRun) {
        gameObject.SetActive(true);
        Time.timeScale = 0;

        int recordScore = PlayerPrefs.GetInt("recordScore",0);

        if (scoreFromThisRun > recordScore) {
            recordScore = scoreFromThisRun;
            PlayerPrefs.SetInt("recordScore", recordScore);
            recordText.text = recordScore.ToString();
        }
        else {
            recordText.text = recordScore.ToString();
        }
        //       Добавление общей суммы МОНЕТ --

        // 1. Получаем *текущую общую* сумму (или 0, если ее еще нет)
        int totalCoins = PlayerPrefs.GetInt("totalCoins", 0);
        // 2. Добавляем к ней монеты из этого забега
        totalCoins += coinsFromThisRun;
        // 3. Сохраняем новое *общее* значение
        PlayerPrefs.SetInt("totalCoins", totalCoins);
        // принудительно сохранение
        PlayerPrefs.Save();
    }

    public void RestartLevel() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ToMenu() {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}