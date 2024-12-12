using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapC : MonoBehaviour
{
    public GameObject wall;
    public GameObject[] wallprefab;
    private int selectwall;
    public GridManager gridManager;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリックで設置
        {
            Placewall();
        }

        for (int i = 0; i < wallprefab.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // 数字キーでブロックを切り替え
            {
                selectwall = i;
                Debug.Log("Selected Block: " + wallprefab[selectwall].name);
            }
        }
    }

    void Placewall()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // グリッド座標を取得
            Vector2Int gridPosition = gridManager.GetGridPosition(hit.point);

            // グリッド外の座標なら設置しない
            if (gridPosition.x < 0 || gridPosition.x >= gridManager.width ||
                gridPosition.y < 0 || gridPosition.y >= gridManager.height)
            {
                Debug.Log("Cannot place block outside the grid.");
                return;
            }

            // 既にブロックがある場合は設置しない
            if (gridManager.grid[gridPosition.x, gridPosition.y] != 0)
            {
                Debug.Log("A block already exists here.");
                return;
            }

            // ブロックを配置
            Vector3 placePosition = new Vector3(gridPosition.x * gridManager.cellSize, 0, gridPosition.y * gridManager.cellSize);
            GameObject newBlock = Instantiate(wallprefab[selectwall], placePosition, Quaternion.identity);

            // グリッドに登録
            gridManager.grid[gridPosition.x, gridPosition.y] = 1; // ブロックを配置済み
        }

    }
}
