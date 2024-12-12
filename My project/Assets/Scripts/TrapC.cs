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
        if (Input.GetMouseButtonDown(0)) // ���N���b�N�Őݒu
        {
            Placewall();
        }

        for (int i = 0; i < wallprefab.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // �����L�[�Ńu���b�N��؂�ւ�
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
            // �O���b�h���W���擾
            Vector2Int gridPosition = gridManager.GetGridPosition(hit.point);

            // �O���b�h�O�̍��W�Ȃ�ݒu���Ȃ�
            if (gridPosition.x < 0 || gridPosition.x >= gridManager.width ||
                gridPosition.y < 0 || gridPosition.y >= gridManager.height)
            {
                Debug.Log("Cannot place block outside the grid.");
                return;
            }

            // ���Ƀu���b�N������ꍇ�͐ݒu���Ȃ�
            if (gridManager.grid[gridPosition.x, gridPosition.y] != 0)
            {
                Debug.Log("A block already exists here.");
                return;
            }

            // �u���b�N��z�u
            Vector3 placePosition = new Vector3(gridPosition.x * gridManager.cellSize, 0, gridPosition.y * gridManager.cellSize);
            GameObject newBlock = Instantiate(wallprefab[selectwall], placePosition, Quaternion.identity);

            // �O���b�h�ɓo�^
            gridManager.grid[gridPosition.x, gridPosition.y] = 1; // �u���b�N��z�u�ς�
        }

    }
}
