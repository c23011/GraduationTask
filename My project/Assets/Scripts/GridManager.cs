using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public List<GameObject> mapPrefabs;
    public int mapCounter;
    public GameObject startPrefab;  // �X�^�[�g�n�_
    public GameObject goalPrefab;   // �S�[���n�_
    public GameObject player; // �v���C���[
    public List<GameObject> randomMapPrefabs;
    public List<GameObject> manualMapPrefabs;
    public Camera mainCamera;
    public float cameraHeight = 10f;
    public float cameraMoveDuration = 1f;
    public int gridSizeX = 5;
    public int gridSizeY = 5;
    public float spacing = 1.1f;
    public float mapHeight = 0.5f;

    private List<GameObject> cells = new List<GameObject>();
    private HashSet<int> usedCells = new HashSet<int>();
    private List<int> emptyCells = new List<int>();

    private int selectedMapIndex = -1;
    private int startCellIndex = -1;
    private int goalCellIndex = -1;

    private int playerCellIndex; // ���݂̃v���C

    private int currentX = 0; // ���݂̑I�𒆂̃}�X��X���W
    private int currentY = 0; // ���݂̑I�𒆂̃}�X��Y���W

    public GameObject[] InstMaps;
    public int InstMapNum;
    public Vector3 InstMapRot;
    Vector3 StartPlayerPos;

    void Start()
    {
        GenerateGrid();
        SetStartAndGoal();
        PlaceRandomMaps(20); // �����_����3�z�u
        FocusOnCell(currentX, currentY);
    }   

    void Update()
    {
        // �����L�[�Ń}�b�v�I��
        for (int i = 0; i < mapPrefabs.Count; i++)
        {
            if (Input.GetKeyDown((KeyCode)(KeyCode.Alpha1 + i)))
            {
                selectedMapIndex = i;
                Debug.Log($"Selected Map: {mapPrefabs[selectedMapIndex].name}");
            }
        }

        // �\���L�[�Ń}�X�ړ�
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveSelection(0, -1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) MoveSelection(0, 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveSelection(-1, 0);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveSelection(1, 0);

        //���N���b�N�Ŏ��v���ɉ�]
        if (Input.GetMouseButtonDown(0))
        {
            InstMapRot = new Vector3(0, InstMapRot.y + 90.0f, 0);
        }

        if (Input.GetMouseButtonDown(1))
        {
            InstMapRot = new Vector3(0, InstMapRot.y - 90.0f, 0);
        }

        // Space�L�[�Ń}�b�v��z�u
        if (Input.GetKeyDown(KeyCode.Space) && selectedMapIndex != -1)
        {
            PlaceMapAtCurrentCell();
        }
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // ���W���v�Z
                Vector3 position = new Vector3(x * spacing, 0, y * spacing);

                // Prefab�𐶐����A���X�g�ɒǉ�
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cells.Add(cell); // ���������}�X�����X�g�ɒǉ�
                int a = 0;
                GameObject cellObj;
                cellObj = cells[a];
                a += 1;
                Debug.Log(cellObj);
            }
        }
    }

    void PlaceRandomMaps(int mapCount)
    {
        // �����_���Ń}�X��I��
        List<int> selectedIndices = new List<int>();
        while (selectedIndices.Count < mapCount)
        {
            int randomIndex = Random.Range(0, cells.Count);
            if (!usedCells.Contains(randomIndex))
            {
                if (!selectedIndices.Contains(randomIndex))
                {
                    selectedIndices.Add(randomIndex);
                    usedCells.Add(randomIndex); // �g�p�ς݂ɒǉ�
                }
            }
        }

        // �I�΂ꂽ�}�X�̏�Ƀ}�b�v��z�u
        foreach (int index in selectedIndices)
        {
            //List���̎w����W��cell��I��
            GameObject selectedCell = cells[index];

            // �}�X�̐^��Ƀ����_���ȃ}�b�vPrefab��z�u
            int randomMaps = Random.Range(0, mapCounter);
            int randomRot = Random.Range(-1, 3);
            Vector3 MapRot; InstMapNum = index;

            if (mapPrefabs[randomMaps] != null)
            {
                Vector3 mapPosition = selectedCell.transform.position + Vector3.up * mapHeight;
                InstMaps[InstMapNum] = Instantiate(mapPrefabs[randomMaps], mapPosition, Quaternion.identity, transform);

                //���������}�b�v�������_����90�����݂ŉ�]
                MapRot = new Vector3(0, randomRot * 90.0f, 0);

                InstMaps[InstMapNum].transform.rotation = Quaternion.Euler(MapRot);
            }
        }
    }

    // �\���L�[�őI�𒆂̃}�X���ړ�
    void MoveSelection(int deltaX, int deltaY)
    {
        currentX = Mathf.Clamp(currentX + deltaX, 0, gridSizeX - 1);
        currentY = Mathf.Clamp(currentY + deltaY, 0, gridSizeY - 1);
        FocusOnCell(currentX, currentY);
    }

    // ���ݑI�𒆂̃}�X�Ƀt�H�[�J�X
    void FocusOnCell(int x, int y)
    {
        int cellIndex = y * gridSizeX + x; // 1�����z��̃C���f�b�N�X�v�Z
        GameObject cell = cells[cellIndex];

        MoveCameraToCell(cell);

        // �n�C���C�g�\���i��F�F��ύX�j
        //foreach (var c in cells) c.GetComponent<Renderer>().material.color = Color.white; // �f�t�H���g�F
        //cell.GetComponent<Renderer>().material.color = Color.yellow; // �I�𒆂̃}�X���n�C���C�g
    }

    // ���݂̑I�𒆂̃}�X�Ƀ}�b�v��z�u
    void PlaceMapAtCurrentCell()
    {
        int cellIndex = currentY * gridSizeX + currentX; // ���݂̃C���f�b�N�X���v�Z

        GameObject cell = cells[cellIndex];
        GameObject mapPrefab = mapPrefabs[selectedMapIndex];
        InstMapNum = cellIndex;

        if (InstMaps[InstMapNum] != null)
        {
            Destroy(InstMaps[InstMapNum]);
            Debug.Log(InstMapNum);
        }

        if (usedCells.Contains(cellIndex))
        {
            usedCells.Remove(cellIndex);
        }

        PlaceMapOnCell(cell, mapPrefab);

        usedCells.Add(cellIndex); // �g�p�ς݂ɒǉ�
    }

    // �J�����ړ�
    void MoveCameraToCell(GameObject cell)
    {
        Vector3 targetPosition = cell.transform.position + Vector3.up * cameraHeight;
        StartCoroutine(SmoothCameraMove(targetPosition));
    }

    // �J�����ړ��A�j���[�V����
    System.Collections.IEnumerator SmoothCameraMove(Vector3 targetPosition)
    {
        Vector3 startPosition = mainCamera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < cameraMoveDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / cameraMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
    }

    // �Z���̏�Ƀ}�b�v��z�u
    void PlaceMapOnCell(GameObject cell, GameObject mapPrefab)
    {
        Vector3 mapPosition = cell.transform.position + Vector3.up * mapHeight;
        InstMaps[InstMapNum] = Instantiate(mapPrefab, mapPosition, Quaternion.Euler(InstMapRot), cell.transform);
    }

    void SetStartAndGoal()
    {
        List<int> availableCells = new List<int>();
        for (int i = 0; i < cells.Count; i++)
        {
            if (!usedCells.Contains(i))
            {
                availableCells.Add(i);
            }
        }

        if (availableCells.Count < 2)
        {
            Debug.LogError("�󂫃}�X������܂���I");
            return;
        }

        // �����_���ɃX�^�[�g�ƃS�[��������
        startCellIndex = availableCells[Random.Range(0, availableCells.Count)];
        usedCells.Remove(startCellIndex);
        goalCellIndex = availableCells[Random.Range(0, availableCells.Count)];
        usedCells.Remove(goalCellIndex);

        // �X�^�[�g�n�_��z�u
        Instantiate(startPrefab, cells[startCellIndex].transform.position + Vector3.up * mapHeight, Quaternion.identity);
        usedCells.Add(startCellIndex);

        // �S�[���n�_��z�u
        Instantiate(goalPrefab, cells[goalCellIndex].transform.position + Vector3.up * mapHeight, Quaternion.identity);
        usedCells.Add(goalCellIndex);

        SpawnPlayer();
    }

    // �v���C���[���X�|�[��
    void SpawnPlayer()
    {
        if (startCellIndex == -1) return;
        StartPlayerPos = cells[startCellIndex].transform.position + Vector3.up * mapHeight;
        StartPlayerPos.y = 0.2f;
        player.transform.position = StartPlayerPos;
        playerCellIndex = startCellIndex;
    }
}


