using UnityEngine;

public class TouchesController : MonoBehaviour
{
    public TileBoard tileBoard;
    public TileGrid grid;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool touchStarted = false;

    // Минимальная дистанция для распознавания свайпа
    private const float MIN_SWIPE_DISTANCE = 50f;

    private void Update()
    {
        // Если не ждём анимацию перемещения плиток:
        if (!tileBoard.waiting)
        {
            // Обработка касаний на мобильных устройствах
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStartPos = touch.position;
                        touchStarted = true;
                        break;

                    case TouchPhase.Ended:
                        if (touchStarted)
                        {
                            touchEndPos = touch.position;
                            ProcessSwipe();
                            touchStarted = false;
                        }
                        break;

                    case TouchPhase.Canceled:
                        touchStarted = false;
                        break;
                }
            }
        }
    }

    private void ProcessSwipe()
    {
        Vector2 swipeVector = touchEndPos - touchStartPos;

        // Проверяем, достаточно ли длинный свайп
        if (swipeVector.magnitude < MIN_SWIPE_DISTANCE)
            return;

        // Определяем направление свайпа
        Vector2 swipeDirection = swipeVector.normalized;

        // Определяем основное направление (горизонтальное или вертикальное)
        bool isHorizontalSwipe = Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y);

        if (isHorizontalSwipe)
        {
            // Горизонтальный свайп
            if (swipeDirection.x > 0)
            {
                // Свайп вправо
                Debug.Log("Swipe Right");
                tileBoard?.MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
            else
            {
                // Свайп влево
                Debug.Log("Swipe Left");
                tileBoard?.MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
        }
        else
        {
            // Вертикальный свайп
            if (swipeDirection.y > 0)
            {
                // Свайп вверх
                Debug.Log("Swipe Up");
                tileBoard?.MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else
            {
                // Свайп вниз
                Debug.Log("Swipe Down");
                tileBoard?.MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
        }
    }
}