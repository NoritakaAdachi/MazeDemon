using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DemonManager : MonoBehaviour
{
    public Transform player;
    //private float speed = 5f;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private float chaseDistance = 10.0f;
    private System.Random random = new System.Random();

    private float posX;
    private float posZ;

    private Animator animator;

    //private bool isPlaying = false;
    private bool scream = false;
    [SerializeField] private AudioSource monster;
    [SerializeField] private AudioClip shout;
    [SerializeField] private AudioClip monster_win;

    void Start()
    {
        MazeGenerator mazeGenerator = GameObject.Find("MazeManager").GetComponent<MazeGenerator>();
        posX = Random.Range(0, mazeGenerator.width * 5.0f);
        posZ = Random.Range(0, mazeGenerator.height * 5.0f);

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.speed = 4.5f;
        transform.position = new Vector3(posX, transform.position.y, posZ);

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer < chaseDistance)
        {
            navMeshAgent.SetDestination(player.position);
            animator.SetBool("Run", true);
            if (!scream)
            {
                monster.PlayOneShot(shout);
                scream = true;
            }
            

            if (distanceToPlayer <1.0f)
            {
                monster.PlayOneShot(monster_win);
                MazeManager.HitPlayer.Invoke();
            }
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
            animator.SetBool("Run", false);
            scream = false;
            
        }
    }
}
