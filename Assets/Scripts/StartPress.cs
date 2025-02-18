using UnityEngine;
using UnityEngine.SceneManagement;//シーン切り替えに使用するライブラリ

public class StartPress: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            MazeGenerator.ChangeCourse.Invoke();
            SceneManager.LoadScene("Playground", LoadSceneMode.Single);

        }
    }
    
}