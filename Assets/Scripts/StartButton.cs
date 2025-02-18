using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void StartBtn()
    {
        SceneManager.LoadScene("Playground");
    }

    public void OnClick()
    {
        Debug.Log("Button click!");
    }
}
