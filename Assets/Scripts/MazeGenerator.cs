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

    // Maze �̏�����
    public void ClearMaze()
    {
        List<GameObject> tempList = new List<GameObject>();
        foreach (Transform child in root)
        {
            tempList.Add(child.gameObject);
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            DestroyImmediate(tempList[i]);              // Destroy �ƈႢ���S�ɍ폜
        }

        // �S�[���ʒu�̏�����
        goalPosX = 0;
        goalPosZ = 0;
    }

    // Maze �쐬(MazeCell.cs ������������Ă���)
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

                MazeCell cell = Instantiate(                // �I�u�W�F�N�g�𐶐�����֐�
                    mazeCellPrefab,                         // prefab(���𐶐�����̂�) 
                    new Vector3(posX, 0f, posZ),            // ���W
                    Quaternion.identity,                    // ��]
                    root).GetComponent<MazeCell>();         // cell �� MazeCell �̏����i�[

                cell.transform.localScale *= cellScale;     // cellScale �{�Ɋg��
                cell.name = $"{x}-{y}";                     // cell �̖��O��ݒ�
                cell.Setup(maze[x, y]);                     // MazeCell �̕\��
            }
        }


        // Goal �̈ʒu�������_���ݒ�
        goalPosX = Random.Range(0, width * 5.0f);
        goalPosZ = Random.Range(0, height * 5.0f);

        Vector3 pos = new Vector3(goalPosX, 0, goalPosZ);
        goal.transform.position = pos;
    }

    // Maze �� start �n�_�� (x, y) �ŕ\�� 
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

    // �T���̏��Ԃ������_���ɃV���b�t������֐�
    private List<(int, int, MazeCellModel.Wall, MazeCellModel.Wall)> ShuffleDirections()
    {
        List<(int, int, MazeCellModel.Wall, MazeCellModel.Wall)> directions = new List<(int, int, MazeCellModel.Wall, MazeCellModel.Wall)> {
            (0, 1, MazeCellModel.Wall.Top, MazeCellModel.Wall.Bottom),          //(0, 1) ��� 1 �����ړ�
            (0, -1, MazeCellModel.Wall.Bottom, MazeCellModel.Wall.Top),         //(0, -1) ��� -1 �����ړ�
            (-1, 0, MazeCellModel.Wall.Left, MazeCellModel.Wall.Right),         //(-1, 0) �E�� -1 �����ړ�
            (1, 0, MazeCellModel.Wall.Right, MazeCellModel.Wall.Left)           //(1, 0) �E�� 1 �����ړ�
        };
        for (int i = 0; i < directions.Count; i++)                              // direction �̏��Ԃ�ύX���ĕԂ�
        {
            var temp = directions[i];
            int randomIndex = random.Next(i, directions.Count);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }
        return directions;
    }
}