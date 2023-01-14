using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    GeneralGameHandler gameHandler;
    public string gameOverTextMessage, gameWinTextMessage;
    public GameObject saveScoreScreen;
    public GameObject saveScoreButton, sendScoreButton, mainMenuAndResetButtons;
    public TextMeshProUGUI timerText, gameOverText;

    bool enable, start;

    private void Awake()
    {
        gameHandler = FindObjectOfType<GeneralGameHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {

        //StartCoroutine(DestroyTouchDownText());
        start = true;
    }

    void Update()
    {
        if (this.gameObject.GetComponent<Canvas>().enabled == true && start)
        {
            StartCoroutine(DestroyTouchDownText());
        }
    }

    public void SetThings()
    {


        if (gameHandler.downsCounter == 5)
        {
            //if game over
            gameOverText.text = gameOverTextMessage;
            timerText.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 153.48f);
            timerText.text = gameHandler.niceTime;
            sendScoreButton.SetActive(false);
            saveScoreButton.SetActive(false);
        }
        else
        {
            //if game win
            gameOverText.text = gameWinTextMessage;
            timerText.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-185, 153.48f);
            timerText.text = gameHandler.niceTime;
            sendScoreButton.SetActive(true);
            saveScoreButton.SetActive(true);
        }
        start = false;
    }

    public void SendScore()
    {

    }

    public void SaveScore()
    {

        if (!enable)
        {
            saveScoreScreen.SetActive(true);
            mainMenuAndResetButtons.GetComponent<RectTransform>().anchoredPosition = new Vector2(-157f, -270.01f);
            enable = true;
        }
        else
        {
            saveScoreScreen.SetActive(false);
            mainMenuAndResetButtons.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.42729f, -270.01f);
            enable = false;
        }


    }

    public void ResetGame()
    {
        Scene thiScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thiScene.name);
    }

    public void MainMenu()
    {

    }

    IEnumerator DestroyTouchDownText()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject touchDown = GameObject.Find("TouchdownAnimation");
        if (touchDown != null)
        {
            touchDown.SetActive(false);
            SetThings();
        }
        else
            SetThings();
    }

}
