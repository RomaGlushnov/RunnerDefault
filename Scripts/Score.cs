using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] public TextMeshProUGUI scoreText;

    public int currentScore;
    private void Update() {
        // scoreText.text = ((int)(player.position.z / 2)).ToString();
        currentScore = (int)(player.position.z / 2); // Мы обновляем переменную
        scoreText.text = currentScore.ToString();
    }
}