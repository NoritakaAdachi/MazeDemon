using UnityEngine;

public class MazePlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            MazeManager.OnGoal.Invoke();
        }
    }
}
