using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject panel_false;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goal;
    public static UnityEvent OnGoal = new UnityEvent();
    public static UnityEvent HitPlayer = new UnityEvent();

    public TextMeshProUGUI distanceText;

    public double distanceToGoal;
    public MazeGenerator mazeGenerator;

    private Animator animator;

    private void Awake()
    {
        animator = GameObject.Find("Demon").GetComponent<Animator>();

        panel.SetActive(false);
        panel_false.SetActive(false);

        OnGoal.RemoveAllListeners();
        HitPlayer.RemoveAllListeners();

        OnGoal.AddListener(() =>
        {
            if (!panel_false.activeSelf)
            {
                panel.SetActive(true);
            }
        });

        HitPlayer.AddListener(() =>
        {
            if (!panel.activeSelf)
            {
                panel_false.SetActive(true);
                animator.SetBool("Punch", true);
                //player.SetActive(false);
            }
        });

        SetDistanceText();
    }

    private void Update()
    {
        distanceToGoal = Math.Round((double)Vector3.Distance(player.transform.position, goal.transform.position), MidpointRounding.AwayFromZero);
        SetDistanceText();
    }

    private void SetDistanceText()
    {
        distanceText.text = "Distance to Goal: " + distanceToGoal.ToString();
    }
}
