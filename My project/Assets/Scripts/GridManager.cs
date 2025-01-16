using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10;   // グリッドの幅
    public int gridHeight = 10;  // グリッドの高さ
    public float cellSize = 1f;  // 各マスのサイズ

    // ワールド座標をグリッド座標に変換
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        int xCount = Mathf.RoundToInt(position.x / cellSize);
        int zCount = Mathf.RoundToInt(position.z / cellSize);

        float xPos = xCount * cellSize;
        float zPos = zCount * cellSize;

        return new Vector3(xPos, 0, zPos);
    }
}