using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Префаб квадратика с цифрой. "Плитка"
/// </summary>
public class Tile : MonoBehaviour
{
    /// <summary>
    /// Текущее (актуальное) состояние префаба.
    /// </summary>
    public TileState state { get; private set; }
    
    /// <summary>
    /// Актуальный слот к которому привязан этот префаб плитки.
    /// </summary>
    public TileCell cell { get; private set; }
    
    /// <summary>
    /// Число в квадратике.
    /// </summary>
    public int number { get; private set; }

    private Image background;
    private TextMeshProUGUI text;

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }


    /// <summary>
    /// Устанавливает состояние плитки.
    /// </summary>
    /// <param name="state">Состояние (цвет фона и цвет текста).</param>
    /// <param name="number">Число на плитке.</param>
    public void SetState(TileState state, int number)
    {
        this.state = state;
        this.number = number;

        background.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = number.ToString();
    }

    /// <summary>
    /// Спавнит плитку в позиции слота.
    /// </summary>
    /// <param name="cell">Слот, в позиции которого, создаём плитку.</param>
    public void Spawn(TileCell cell)
    {
        // Если этот слот - существует.
        if (this.cell != null)
        {
            this.cell.tile = null;  // Отвязываем старую плитку, потому что сейчас тут будет новая.
        }

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    /// <summary>
    /// Перемещает плитку в слот.
    /// </summary>
    /// <param name="cell">Слот в который хотим переместиться.</param>
    public void MoveTo(TileCell cell)
    {
        // Если слот в который хотим переместиться - существует.
        if (this.cell != null)
        {
            this.cell.tile = null;  // Отвязываем старую плитку, потому что сейчас тут будет новая.
        }

        this.cell = cell;
        this.cell.tile = this;

        StartCoroutine(Animate(cell.transform.position));
    }

    /// <summary>
    /// Перемещает плитку в указанную позицию.
    /// </summary>
    private IEnumerator Animate(Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed/duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;
    }
}
