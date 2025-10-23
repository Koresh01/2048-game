using Unity.VisualScripting;
using UnityEngine;

public class ComputerInputController : MonoBehaviour
{
    public TileBoard tileBoard;
    public TileGrid grid;

    private void Update()
    {
        // ���� �� ��� �������� ����������� ������:
        if (!tileBoard.waiting)
        {
            // ����� ���������� ����������� ���� X � Y ����� � TileGrid � � Start() �� ������ ����������.
            if (Input.GetKeyDown(KeyCode.W))
            {
                tileBoard.MoveTiles(Vector2Int.up, 0, 1, 1, 1);   // ����� �������� ������ �����, ������ ������ ���������������� �� ���� �������, ����� ������ ������� row.
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                tileBoard.MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                tileBoard.MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                tileBoard.MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
        }
    }


}
