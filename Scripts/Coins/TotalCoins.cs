using UnityEngine;
using TMPro;

public class DisplayTotalCoins : MonoBehaviour {
    [SerializeField] private TMP_Text coinsText;

    void Start() {
        // 1. «агружаем общее количество монет (или 0, если еще не сохранено)
        int totalCoins = PlayerPrefs.GetInt("totalCoins", 0);

        // 2. ќтображаем это значение в текстовом поле
        coinsText.text = totalCoins.ToString();
    }
}