using UnityEngine;

public class CoinRotate : MonoBehaviour {
    [Header("Настройки Вращения")]
    public float rotationSpeed = 100f; // Скорость в градусах в секунду

    [Header("Настройки Приседания")]
    public float bobSpeed = 3f;        // Скорость
    public float bobHeight = 0.1f;   // Дистанция движения (в юнитах)

    private Vector3 startLocalPosition;

    void Start() {
        // Запоминаем стартовую позицию относительно родителя,
        // чтобы "приседания" работали для каждой монеты в стопке.
        startLocalPosition = transform.localPosition;
    }

    void Update() {
        // 1. Вращение
        // Time.deltaTime делает вращение плавным и независимым от ФПС
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // 2. Приседание (Bobbing)
        // Mathf.Sin() генерирует плавную волну от -1 до +1
        float sinWave = Mathf.Sin(Time.time * bobSpeed);
        float newY = startLocalPosition.y + (sinWave * bobHeight);

        // Используем localPosition, чтобы монета двигалась
        // относительно своей позиции в стопке, а не в мире.
        transform.localPosition = new Vector3(startLocalPosition.x, newY, startLocalPosition.z);
    }
}