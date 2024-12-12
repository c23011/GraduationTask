using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject blockPrefabs;
    private Dictionary<Vector2Int, GameObject> gridCells = new Dictionary<Vector2Int, GameObject>();
    private int selectedBlockIndex = 0;
    public int width = 10;  // マップの横幅
    public int height = 10; // マップの高さ
    public float cellSize = 1f; // 各セルのサイズ

    public int[,] grid; // マップデータ（0: 空き, 1: 壁）
    // ワールド座標をグリッド座標に変換
   
    


    public Vector2Int GetGridPosition(Vector3 position)
    {
        int xCount = Mathf.RoundToInt(position.x / cellSize);
        int zCount = Mathf.RoundToInt(position.z / cellSize);
        return new Vector2Int(xCount, zCount);
    }

    // ブロックを配置できるかを確認
    public bool CanPlaceBlock(Vector2Int gridPosition)
    {
        return !gridCells.ContainsKey(gridPosition); // 指定の位置にブロックがない場合のみ配置可能
    }

    public bool IsAdjacentToPath(Vector2Int position)
    {
        foreach (Vector2Int neighbor in GetNeighbors(position))
        {
            // 通路や接続可能なブロックが存在するかを確認
            if (grid[neighbor.x, neighbor.y] == 0) // 例: 0が通路を意味する
            {
                return true;
            }
        }
        return false;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        if (cell.x > 0) neighbors.Add(new Vector2Int(cell.x - 1, cell.y)); // 左
        if (cell.x < width - 1) neighbors.Add(new Vector2Int(cell.x + 1, cell.y)); // 右
        if (cell.y > 0) neighbors.Add(new Vector2Int(cell.x, cell.y - 1)); // 下
        if (cell.y < height - 1) neighbors.Add(new Vector2Int(cell.x, cell.y + 1)); // 上

        return neighbors;
    }

    private GameObject GetBlockPrefabs()
    {
        return blockPrefabs;
    }

    void PlaceBlock(GameObject[] blockPrefabs)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // グリッド座標を取得
            Vector2Int gridPosition = this.GetGridPosition(hit.point);

            // 既にブロックがある場合は設置しない
            if (!this.CanPlaceBlock(gridPosition))
            {
                Debug.Log("A block already exists here.");
                return;
            }

            // 通路に接続している場合のみ設置を許可
            if (!this.IsAdjacentToPath(gridPosition))
            {
                Debug.Log("Block must be adjacent to an existing path.");
                return;
            }

            // ブロックを配置
            Vector3 placePosition = new Vector3(gridPosition.x * this.cellSize, 0, gridPosition.y * this.cellSize);
            GameObject newBlock = Instantiate(blockPrefabs[selectedBlockIndex], placePosition, Quaternion.identity);
            this.RegisterBlock(gridPosition, newBlock);
        }
    }

    // ブロックを登録
    public void RegisterBlock(Vector2Int gridPosition, GameObject block)
    {
        if (!gridCells.ContainsKey(gridPosition))
        {
            gridCells[gridPosition] = block;
        }
    }

    // ブロックを削除
    public void RemoveBlock(Vector2Int gridPosition)
    {
        if (gridCells.ContainsKey(gridPosition))
        {
            Destroy(gridCells[gridPosition]);
            gridCells.Remove(gridPosition);
        }
    }
    
}