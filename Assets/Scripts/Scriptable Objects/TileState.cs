using UnityEngine;

/// <summary>
/// Генератор состояний плитки.
/// </summary>
[CreateAssetMenu(menuName = "Состояние Плитки")]
public class TileState : ScriptableObject
{
    [Tooltip("Цвет фона плитки с цифрой.")] public Color backgroundColor;
    [Tooltip("Цвет самой цифры на плитке.")] public Color textColor;
}
