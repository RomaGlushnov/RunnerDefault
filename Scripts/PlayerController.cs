using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// Этот скрипт "слушает" SwipeController и двигает игрока
public class PlayerController : MonoBehaviour {

    private CharacterController controller;
    private Vector3 dir; // Вектор для расчета всего движения

    [Header("Настройки движения")]
    [SerializeField] public float speed = 10f;
    [SerializeField] public float laneChangeSpeed = 10f;
    [SerializeField] public float jumpForce = 8f;
    [SerializeField] public float gravity = -20f;
    [SerializeField] private int coins;
    [SerializeField] private Text coinsText;
    [SerializeField] private LosePanelScript losePanelScript;
    [SerializeField] private Score scoreScript;

    [Header("Настройки полос")]
    [SerializeField] private float lineDistance = 4; // Дистанция между полосами
    private int lineToMove = 1; // 0 = лево, 1 = центр, 2 = право

    // --- Новые переменные для скольжения и удара ---
    [Header("Действия (Свайп Вниз)")]
    [SerializeField] public float slamForce = -30f; // Сила удара (должна быть отрицательной)
    [SerializeField] public float slideDuration = 1.0f;

    private float originalControllerHeight;
    private Vector3 originalControllerCenter;
    private bool isSliding = false;
    // ---

    private float maxSpeed = 110;

    void Start() {
        controller = GetComponent<CharacterController>();
        // Запоминаем исходные размеры коллайдера
        originalControllerHeight = controller.height;
        originalControllerCenter = controller.center;
        // Сбрасываем счетчик монет в начале каждого забега
        coins = 0;
        coinsText.text = coins.ToString();

        StartCoroutine(SpeedIncrease());
    }

    // Update() ТОЛЬКО считывает ввод и меняет переменные
    private void Update() {
        // "Спрашиваем" у SwipeController, был ли свайп
        if (SwipeController.swipeRight) {
            if (lineToMove < 2)
                lineToMove++; // Реагируем на свайп
        }
        if (SwipeController.swipeLeft) {
            if (lineToMove > 0)
                lineToMove--; // Реагируем на свайп
        }
        if (SwipeController.swipeUp) {
            // controller.isGrounded теперь работает корректно
            if (controller.isGrounded)
                Jump();
        }

        // --- НОВАЯ ЛОГИКА СВАЙПА ВНИЗ ---
        if (SwipeController.swipeDown) {
            if (controller.isGrounded) {
                // Если на земле - СКОЛЬЗИТЬ
                if (!isSliding) {
                    StartCoroutine(Slide());
                }
            }
            else {
                // Если в воздухе - УДАР О ЗЕМЛЮ
                dir.y = slamForce; // Просто задаем сильную скорость вниз
            }
        }
        // ---
    }

    private void Jump() {
        // Задаем вертикальный импульс
        dir.y = jumpForce;
    }

    // --- НОВАЯ ФУНКЦИЯ СКОЛЬЖЕНИЯ (Корутина) ---
    private IEnumerator Slide() {
        isSliding = true;

        // 1. Уменьшаем коллайдер
        float newHeight = originalControllerHeight / 2f;
        float newCenterY = originalControllerCenter.y - (newHeight / 2f);

        controller.height = newHeight;
        controller.center = new Vector3(originalControllerCenter.x, newCenterY, originalControllerCenter.z);

        // 2. Ждем N секунд
        yield return new WaitForSeconds(slideDuration);

        // 3. Возвращаем коллайдер в норму
        controller.height = originalControllerHeight;
        controller.center = originalControllerCenter;

        isSliding = false;
    }
    // ---

    // FixedUpdate() ДВИГАЕТ персонажа
    private void FixedUpdate() {
        // 1. Движение ВПЕРЕД (ось Z)
        dir.z = speed;

        // 2. ГРАВИТАЦИЯ и ПРЫЖОК (ось Y)
        if (controller.isGrounded) {
            // Если на земле и не прыгаем, сбрасываем верт. скорость
            if (dir.y < 0)
                dir.y = -2f;
        }
        else {
            // Если в воздухе - применяем гравитацию
            dir.y += gravity * Time.fixedDeltaTime;
        }

        // 3. СМЕНА ПОЛОС (ось X)
        // Рассчитываем целевую позицию по X
        float targetX = (lineToMove - 1) * lineDistance;
        // Плавно двигаем по X к цели, задавая СКОРОСТЬ
        dir.x = (targetX - transform.position.x) * laneChangeSpeed;

        // 4. ПРИМЕНЯЕМ ВСЕ ДВИЖЕНИЕ (X, Y, Z) ОДНИМ ВЫЗОВОМ
        controller.Move(dir * Time.fixedDeltaTime);
    }

    private IEnumerator SpeedIncrease() {
        while (speed < maxSpeed) {
            yield return new WaitForSeconds(4);
            if (speed < maxSpeed) {
                speed += 1;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.tag == "obstacle") {
            //Сохраняем ОЧКИ для рекорда
            int lastRunScore = scoreScript.currentScore;
            // 2. НАПРЯМУЮ вызываем панель и передаем ей данные
            losePanelScript.ShowPanel(lastRunScore, coins);
            //открываем панель проиграша
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "coin") {
            coins++;
            coinsText.text = coins.ToString();
            Destroy(other.gameObject);
        }
    }
}
