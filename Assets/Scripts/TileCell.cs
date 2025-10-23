using UnityEngine;

/// <summary>
/// Слот для плитки.
/// </summary>
public class TileCell : MonoBehaviour
{
    /// <summary>
    /// Координаты слота для ячейки (не мировые, а относительно Grid).
    /// </summary>
    public Vector2Int coordinates { get; set; }

    /// <summary>
    /// Префаб плитки, который на данный момент располагается в этом слоту.
    /// </summary>
    public Tile tile { get; set; }

    public bool empty => tile == null;
    public bool occupied => tile != null;
}
