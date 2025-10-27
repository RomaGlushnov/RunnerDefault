using UnityEngine;

// Этот скрипт определяет свайпы и делает их доступными для PlayerController
public class SwipeController : MonoBehaviour {
    // Эти переменные будет "видеть" PlayerController
    public static bool swipeRight;
    public static bool swipeLeft;
    public static bool swipeUp;
    public static bool swipeDown;

    private bool isSwiping = false;
    private Vector2 startTouch;

    // Минимальная дистанция для свайпа
    [SerializeField] private float minSwipeDistance = 50f;

    void Update() {
        // Сбрасываем все свайпы в начале каждого кадра
        swipeRight = false;
        swipeLeft = false;
        swipeUp = false;
        swipeDown = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        // --- Логика для Мыши (для тестов в редакторе) ---
        if (Input.GetMouseButtonDown(0)) {
            isSwiping = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) {
            if (isSwiping) {
                isSwiping = false;
                CheckSwipe();
            }
        }
#endif

#if UNITY_IOS || UNITY_ANDROID
        // --- Логика для Тачскрина ---
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                isSwiping = true;
                startTouch = t.position;
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                if (isSwiping)
                {
                    isSwiping = false;
                    CheckSwipe();
                }
            }
        }
#endif
    }

    private void CheckSwipe() {
        Vector2 swipeDelta;

#if UNITY_EDITOR || UNITY_STANDALONE
        swipeDelta = (Vector2)Input.mousePosition - startTouch;
#else
            swipeDelta = Input.GetTouch(0).position - startTouch;
#endif


        if (swipeDelta.magnitude < minSwipeDistance) {
            // Это был просто тап, а не свайп
            return;
        }

        // Свайп был достаточно длинным, определяем направление
        float x = swipeDelta.x;
        float y = swipeDelta.y;

        if (Mathf.Abs(x) > Mathf.Abs(y)) {
            // Свайп по горизонтали
            if (x < 0)
                swipeLeft = true;
            else
                swipeRight = true;
        }
        else {
            // Свайп по вертикали
            if (y < 0)
                swipeDown = true;
            else
                swipeUp = true;
        }
    }
}