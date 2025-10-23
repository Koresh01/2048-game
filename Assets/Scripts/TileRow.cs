using UnityEngine;

/// <summary>
/// Строка слотов для плитки.
/// </summary>
public class TileRow : MonoBehaviour
{
    /// <summary>
    /// Слоты для плитки.
    /// </summary>
    public TileCell[] cells { get; private set; }

    private void Awake()
    {
        cells = GetComponentsInChildren<TileCell>();
    }
}
