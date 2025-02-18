using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

public class MazeGenerator : MonoBehaviour
{
    public int width, height;
    private System.Random random = new System.Random();
    private MazeCellModel[,] maze;

    public GameObject mazeCellPrefab;
    public GameObject goal;
    [SerializeField] private Transform root;
    private float cellScale = 5f;

    public float goalPosX;
    public float goalPosZ;

    public static UnityEvent ChangeCourse = new UnityEvent();

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    void Start()
    {
        /*
        ChangeCourse.RemoveAllListeners();

        ChangeCourse.AddListener(() =>
        {
            Debug.Log("TOKYO");
            GenerateMaze();
        });
        */
    }

    // Maze の初期化
    public void ClearMaze()
    {
        List<GameObject> tempList = new List<GameObject>();
        foreach (Transform child in root)
        {
            tempList.Add(child.gameObject);
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            DestroyImmediate(tempList[i]);              // Destroy と違い完全に削除
        }

        // ゴール位置の初期化
        goalPosX = 0;
        goalPosZ = 0;
    }

    // Maze 作成(MazeCell.cs を一つずつ処理していく)
    public void GenerateMaze()
    {
        ClearMaze();

        maze = new MazeCellModel[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = new MazeCellModel();
            }
        }
        GenerateMaze(0, 0);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float posX = x * cellScale;
                float posZ = y * cellScale;

                MazeCell cell = Instantiate(                // オブジェクトを生成する関数
                    mazeCellPrefab,                         // prefab(何を生成するのか) 
                    new Vector3(posX, 0f, posZ),            // 座標
                    Quaternion.identity,                    // 回転
                    root).GetComponent<MazeCell>();         // cell に MazeCell の情報を格納

                cell.transform.localScale *= cellScale;     // cellScale 倍に拡大
                cell.name = $"{x}-{y}";                     // cell の名前を設定
                cell.Setup(maze[x, y]);                     // MazeCell の表示
            }
        }


        // Goal の位置もランダム設定
        goalPosX = Random.Range(0, width * 5.0f);
        goalPosZ = Random.Range(0, height * 5.0f);

        Vector3 pos = new Vector3(goalPosX, 0, goalPosZ);
        goal.transform.position = pos;
    }

    // Maze の start 地点を (x, y) で表現 
    private void GenerateMaze(int x, int y)
    {
        MazeCellModel currentCell = maze[x, y];
        currentCell.visited = true;

        foreach (var direction in ShuffleDirections())
        {
            int newX = x + direction.Item1;
            int newY = y + direction.Item2;
            if (newX >= 0 && newY >= 0 && newX < width && newY < height)
            {
                MazeCellModel neighbourCell = maze[newX, newY];
                if (!neighbourCell.visited)
                {
                    neighbourCell.visited = true;
                    currentCell.RemoveWall(direction.Item3);
                    neighbourCell.RemoveWall(direction.Item4);
                    GenerateMaze(newX, newY);
                }
            }
        }
    }

    // 探索の順番をランダムにシャッフルする関数
    private List<(int, int, MazeCellModel.Wall, MazeCellModel.Wall)> ShuffleDirections()
    {
        List<(int, int, MazeCellModel.Wall, MazeCellModel.Wall)> directions = new List<(int, int, MazeCellModel.Wall, MazeCellModel.Wall)> {
            (0, 1, MazeCellModel.Wall.Top, MazeCellModel.Wall.Bottom),          //(0, 1) 上に 1 だけ移動
            (0, -1, MazeCellModel.Wall.Bottom, MazeCellModel.Wall.Top),         //(0, -1) 上に -1 だけ移動
            (-1, 0, MazeCellModel.Wall.Left, MazeCellModel.Wall.Right),         //(-1, 0) 右に -1 だけ移動
            (1, 0, MazeCellModel.Wall.Right, MazeCellModel.Wall.Left)           //(1, 0) 右に 1 だけ移動
        };
        for (int i = 0; i < directions.Count; i++)                              // direction の順番を変更して返す
        {
            var temp = directions[i];
            int randomIndex = random.Next(i, directions.Count);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }
        return directions;
    }
}