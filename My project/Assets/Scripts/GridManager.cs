using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public List<GameObject> mapPrefabs;
    public int mapCounter;
    public GameObject startPrefab;  // スタート地点
    public GameObject goalPrefab;   // ゴール地点
    public GameObject player; // プレイヤー
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

    private int playerCellIndex; // 現在のプレイ

    private int currentX = 0; // 現在の選択中のマスのX座標
    private int currentY = 0; // 現在の選択中のマスのY座標

    public GameObject[] InstMaps;
    public int InstMapNum;
    public Vector3 InstMapRot;
    Vector3 StartPlayerPos;

    void Start()
    {
        GenerateGrid();
        SetStartAndGoal();
        PlaceRandomMaps(20); // ランダムに3つ配置
        FocusOnCell(currentX, currentY);
    }   

    void Update()
    {
        // 数字キーでマップ選択
        for (int i = 0; i < mapPrefabs.Count; i++)
        {
            if (Input.GetKeyDown((KeyCode)(KeyCode.Alpha1 + i)))
            {
                selectedMapIndex = i;
                Debug.Log($"Selected Map: {mapPrefabs[selectedMapIndex].name}");
            }
        }

        // 十字キーでマス移動
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveSelection(0, -1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) MoveSelection(0, 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveSelection(-1, 0);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveSelection(1, 0);

        //左クリックで時計回りに回転
        if (Input.GetMouseButtonDown(0))
        {
            InstMapRot = new Vector3(0, InstMapRot.y + 90.0f, 0);
        }

        if (Input.GetMouseButtonDown(1))
        {
            InstMapRot = new Vector3(0, InstMapRot.y - 90.0f, 0);
        }

        // Spaceキーでマップを配置
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
                // 座標を計算
                Vector3 position = new Vector3(x * spacing, 0, y * spacing);

                // Prefabを生成し、リストに追加
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cells.Add(cell); // 生成したマスをリストに追加
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
        // ランダムでマスを選ぶ
        List<int> selectedIndices = new List<int>();
        while (selectedIndices.Count < mapCount)
        {
            int randomIndex = Random.Range(0, cells.Count);
            if (!usedCells.Contains(randomIndex))
            {
                if (!selectedIndices.Contains(randomIndex))
                {
                    selectedIndices.Add(randomIndex);
                    usedCells.Add(randomIndex); // 使用済みに追加
                }
            }
        }

        // 選ばれたマスの上にマップを配置
        foreach (int index in selectedIndices)
        {
            //List内の指定座標のcellを選択
            GameObject selectedCell = cells[index];

            // マスの真上にランダムなマップPrefabを配置
            int randomMaps = Random.Range(0, mapCounter);
            int randomRot = Random.Range(-1, 3);
            Vector3 MapRot; InstMapNum = index;

            if (mapPrefabs[randomMaps] != null)
            {
                Vector3 mapPosition = selectedCell.transform.position + Vector3.up * mapHeight;
                InstMaps[InstMapNum] = Instantiate(mapPrefabs[randomMaps], mapPosition, Quaternion.identity, transform);

                //生成したマップをランダムに90°刻みで回転
                MapRot = new Vector3(0, randomRot * 90.0f, 0);

                InstMaps[InstMapNum].transform.rotation = Quaternion.Euler(MapRot);
            }
        }
    }

    // 十字キーで選択中のマスを移動
    void MoveSelection(int deltaX, int deltaY)
    {
        currentX = Mathf.Clamp(currentX + deltaX, 0, gridSizeX - 1);
        currentY = Mathf.Clamp(currentY + deltaY, 0, gridSizeY - 1);
        FocusOnCell(currentX, currentY);
    }

    // 現在選択中のマスにフォーカス
    void FocusOnCell(int x, int y)
    {
        int cellIndex = y * gridSizeX + x; // 1次元配列のインデックス計算
        GameObject cell = cells[cellIndex];

        MoveCameraToCell(cell);

        // ハイライト表示（例：色を変更）
        //foreach (var c in cells) c.GetComponent<Renderer>().material.color = Color.white; // デフォルト色
        //cell.GetComponent<Renderer>().material.color = Color.yellow; // 選択中のマスをハイライト
    }

    // 現在の選択中のマスにマップを配置
    void PlaceMapAtCurrentCell()
    {
        int cellIndex = currentY * gridSizeX + currentX; // 現在のインデックスを計算

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

        usedCells.Add(cellIndex); // 使用済みに追加
    }

    // カメラ移動
    void MoveCameraToCell(GameObject cell)
    {
        Vector3 targetPosition = cell.transform.position + Vector3.up * cameraHeight;
        StartCoroutine(SmoothCameraMove(targetPosition));
    }

    // カメラ移動アニメーション
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

    // セルの上にマップを配置
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
            Debug.LogError("空きマスが足りません！");
            return;
        }

        // ランダムにスタートとゴールを決定
        startCellIndex = availableCells[Random.Range(0, availableCells.Count)];
        usedCells.Remove(startCellIndex);
        goalCellIndex = availableCells[Random.Range(0, availableCells.Count)];
        usedCells.Remove(goalCellIndex);

        // スタート地点を配置
        Instantiate(startPrefab, cells[startCellIndex].transform.position + Vector3.up * mapHeight, Quaternion.identity);
        usedCells.Add(startCellIndex);

        // ゴール地点を配置
        Instantiate(goalPrefab, cells[goalCellIndex].transform.position + Vector3.up * mapHeight, Quaternion.identity);
        usedCells.Add(goalCellIndex);

        SpawnPlayer();
    }

    // プレイヤーをスポーン
    void SpawnPlayer()
    {
        if (startCellIndex == -1) return;
        StartPlayerPos = cells[startCellIndex].transform.position + Vector3.up * mapHeight;
        StartPlayerPos.y = 0.2f;
        player.transform.position = StartPlayerPos;
        playerCellIndex = startCellIndex;
    }
}


