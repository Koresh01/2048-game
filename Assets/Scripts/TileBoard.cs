using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public GameManager gameManager;

    /// <summary>
    /// Префаб квадратика с цифрой.
    /// </summary>
    public Tile tilePrefab;

    /// <summary>
    /// Всевозможные состояния префабов цифр.
    /// </summary>
    public TileState[] tileStates;
    
    private TileGrid grid;

    /// <summary>
    /// Список всех плиток, которые сейчас в игре.
    /// </summary>
    private List<Tile> tiles;

    /// <summary>
    /// Параметр true - пока запущена анимация перемещения плиток.
    /// </summary>
    private bool waiting;


    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>();
    }


    /// <summary>
    /// Удаляет все плитки с игрового поля.
    /// </summary>
    public void ClearBoard()
    {
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }

        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
    }
    
    /// <summary>
    /// Создаёт плитку в рандомном свободном слоту.
    /// </summary>
    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0], 2);
        tile.Spawn(grid.GetRandomEmptyCell());

        tiles.Add(tile);
    }

    private void Update()
    {
        // Если не ждём анимацию перемещения плиток:
        if (!waiting)
        {
            // Чтобы определить направления осей X и Y зайди в TileGrid и в Start() ты задаёшь координаты.
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);   // Хотим сместить плитки вверх, значит должны проитерироваться по всем плиткам, кроме самого верхней row.
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
        }
    }

    /// <summary>
    /// Перемещение плитки в заданном направлении.
    /// </summary>
    /// <param name="direction">Направление перемещения плитки.</param>
    /// <param name="startX">Стартовая позиция по X (необходима для определения направления смещения)</param>
    /// <param name="incX">Шаг смещения по X (необходима для определения направления смещения)</param>
    /// <param name="startY">Стартовая позиция по Y (необходима для определения направления смещения)</param>
    /// <param name="incY">Шаг смещения по Y (необходима для определения направления смещения)</param>
    private void MoveTiles(Vector2Int direction, int startX, int incX, int startY, int incY)
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < grid.width; x += incX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        // Если какие то плитки сейчас сдивгаются в корутине, то блокируем ввод от пользователя на 0.1 секунды:
        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    /// <summary>
    /// Перемещает плитку в заданном направлении.
    /// </summary>
    /// <returns>
    /// True - если была запущена корутина по перемещению плитки.
    /// </returns>
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        // Пока найдётся слот(посадочная площадка для плитки) в направлении direction:
        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                // Слияние плиток:
                if (CanMerge(tile, adjacent.tile))
                {
                    Merge(tile, adjacent.tile);
                    return true;
                }
                break;
            }
            
            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Проверяет можно ли соединить 2 плитки.
    /// </summary>
    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.locked;
    }

    /// <summary>
    /// Слияние плиток.
    /// </summary>
    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);    // по сути это анимация где плитка "a" едет в слот "b", после чего "a" - самоуничтожается.

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length-1);
        int number = b.number * 2;

        b.SetState(tileStates[index], number);
    }

    /// <summary>
    /// Метод для получения индекса состояния плитки.
    /// </summary>
    /// <param name="state">Текущее состояние плитки.</param>
    /// <returns>Индекс этого состояния.</returns>
    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i]) {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Блокирует ввод от пользователя на 0.1 секунды.
    /// </summary>
    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;


        // Снова разрешаем одноразовое слияние каждой плитке.
        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        // Каждое перемещение вызывает создание новых плиток в игре 2048:
        if (tiles.Count != grid.size)
        {
            CreateTile();
        }
        

        // Проверка на GameOver:
        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }
    }

    private bool CheckForGameOver()
    {
        if (tiles.Count != grid.size)
        {
            return false;
        }

        // Есть ли у игрока возможность выполнить слияние хотя бы в одном направлении:
        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile))
            {
                return false;
            }

            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }

            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }

            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }

        return true;
    }

}
