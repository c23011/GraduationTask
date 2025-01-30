using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject[] blockPrefabs;  // 配置するブロックの種類をリストで保持
    public GridManager gridManager;    // グリッド管理スクリプトの参照
    private int selectedBlockIndex = 0; // 現在選択中のブロックのインデックス

    void Update()
    {
        // ブロック設置
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBlock();
        }

        // 数字キーでブロック選択
        for (int i = 0; i < blockPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedBlockIndex = i;
                Debug.Log("Selected Block: " + blockPrefabs[selectedBlockIndex].name);
            }
        }
    }

    void PlaceBlock()
    {
        // マウスの位置を取得し、レイキャストを使用してワールド座標を取得
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // ヒットした位置のグリッド上の座標を取得
            //Vector3 gridPosition = gridManager.GetNearestPointOnGrid(hit.point);

            // 選択中のブロックを配置
            //Instantiate(blockPrefabs[selectedBlockIndex], gridPosition, Quaternion.identity);
        }
    }
}