using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
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


    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>();
    }

    private void Start()
    {
        CreateTile();
        CreateTile();
    }

    private void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0], 2);
        tile.Spawn(grid.GetRandomEmptyCell());

        tiles.Add(tile);
    }
}
