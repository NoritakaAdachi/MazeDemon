using UnityEngine;
//using UnityEngine.Events;
using UnityEngine.SceneManagement;//シーン切り替えに使用するライブラリ

public class RestartPress : MonoBehaviour
{
    [SerializeField] private GameObject panel_false;
    [SerializeField] private GameObject panel;

    public static int change_offer = 0;
    public static bool clear = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (panel_false.activeSelf || panel.activeSelf)
        {
            //MazeGenerator mazeGenerator = GameObject.Find("MazeManager").GetComponent<MazeGenerator>();

            if (Input.GetKeyDown("r"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                clear = false;
                //change_offer = 0;
            }
            else if (Input.GetKeyDown("t"))
            {
                SceneManager.LoadScene("Title", LoadSceneMode.Single);
                clear = true;
                //change_offer = 1;
            }
        }
    }
}
