using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayInfo : MonoBehaviour
{
    [Header("InGame")]
    public TextMeshProUGUI kicks;
    public TextMeshProUGUI goals;
    public TextMeshProUGUI task;

    [Header("GameOver")]
    public TextMeshProUGUI Score;
    public GameObject gameOver;

    BallCollisionScript ballScript;

    // Start is called before the first frame update
    void Start()
    {
        ballScript = GameObject.FindObjectOfType<BallCollisionScript>();
    }

    public void GameOverScreen()
    {
        gameOver.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        kicks.text = ballScript.kicks.ToString() + " / " + ballScript.maxKicks.ToString();
        goals.text = ballScript.goals.ToString();
    }
}
