using UnityEngine;

/// <summary>
/// Контроллер "относительных" координат СЛОТОВ ячеек.
/// </summary>
public class TileGrid : MonoBehaviour
{
    public TileRow[] rows {  get; private set; }
    public TileCell[] cells { get; private set; }

    /// <summary>
    /// Общее число слотов.
    /// </summary>
    public int size => cells.Length;

    /// <summary>
    /// Высота поля.
    /// </summary>
    public int height => rows.Length;

    /// <summary>
    /// Ширина поля.
    /// </summary>
    public int width => size / height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        // Ассоциируем слоты с их "относительными" координатами:
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }

    /// <summary>
    /// Возвращает рандомный не занятый слот.
    /// </summary>
    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

        // Перебор занятых клеток, пока не встретим свободную.
        while (cells[index].occupied)
        {
            index++;

            if (index >= cells.Length)
            {
                index = 0;
            }

            // Если проверили все клетки и они оказались заняты.
            if (index == startingIndex)
            {
                return null;
            }
        }

        return cells[index];
    }
}
