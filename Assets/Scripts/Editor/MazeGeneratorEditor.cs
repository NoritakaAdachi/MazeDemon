using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MazeGenerator mazeGenerator = (MazeGenerator)target;

        if (GUILayout.Button("make"))
        {
            mazeGenerator.GenerateMaze();
        }
        if (GUILayout.Button("delete"))
        {
            mazeGenerator.ClearMaze();
        }
    }
}
