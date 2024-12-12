using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject[] blockPrefabs;  // �z�u�\�ȃu���b�N�i���E�ǂȂǁj
    public GridManager gridManager;    // �O���b�h�Ǘ��X�N���v�g�ւ̎Q��
    private int selectedBlockIndex = 0; // �I�𒆂̃u���b�N�̃C���f�b�N�X

    
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���N���b�N�Őݒu
        {
            PlaceBlock();
        }

        for (int i = 0; i < blockPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // �����L�[�Ńu���b�N��؂�ւ�
            {
                selectedBlockIndex = i;
                Debug.Log("Selected Block: " + blockPrefabs[selectedBlockIndex].name);
            }
        }
    }

    void PlaceBlock()
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
            GameObject newBlock = Instantiate(blockPrefabs[selectedBlockIndex], placePosition, Quaternion.identity);

            // �O���b�h�ɓo�^
            gridManager.grid[gridPosition.x, gridPosition.y] = 1; // �u���b�N��z�u�ς݂ɐݒ�
        }
    }
}