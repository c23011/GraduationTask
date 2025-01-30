using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject[] blockPrefabs;  // �z�u����u���b�N�̎�ނ����X�g�ŕێ�
    public GridManager gridManager;    // �O���b�h�Ǘ��X�N���v�g�̎Q��
    private int selectedBlockIndex = 0; // ���ݑI�𒆂̃u���b�N�̃C���f�b�N�X

    void Update()
    {
        // �u���b�N�ݒu
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBlock();
        }

        // �����L�[�Ńu���b�N�I��
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
        // �}�E�X�̈ʒu���擾���A���C�L���X�g���g�p���ă��[���h���W���擾
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // �q�b�g�����ʒu�̃O���b�h��̍��W���擾
            //Vector3 gridPosition = gridManager.GetNearestPointOnGrid(hit.point);

            // �I�𒆂̃u���b�N��z�u
            //Instantiate(blockPrefabs[selectedBlockIndex], gridPosition, Quaternion.identity);
        }
    }
}