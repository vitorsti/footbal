using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GeneralGameHandler : MonoBehaviour
{
    public GameObject receiver;
    public float cdToEnd;

    public static bool downStarted;
    public static float actualYards = 0;       //Jardas no campo
    public static bool isExtraTouchdown;
    public static bool isExtraFieldGoal;
    public static bool touchdown = false;

    [SerializeField] GameObject sack_Indicator;
    [SerializeField] GameObject outOfField_Indicator;
    [SerializeField] GameObject missPass_Indicator;
    [SerializeField] GameObject tackle_Indicator;

    public GameObject[] chancesImg;
    public GameObject chancesUI;

    //public StateOfTheGame state;

    [Header("----- Points -----")]
    public static int playerPoints;
    public static int enemyPoints;

    [Header("----- Downs -----")]
    public int downsCounter;
    public const int maxDowns = 4;

    [Header("----- Yards -----")]
    public float startingDownYards;     //Jardas em que começou o down
    public float thisDownYards;         //Jardas nessa corrida
    //public float downTotalYards;      //Soma dos downs
    public float objectiveYards;        //Jardas pra chegar

    [Header("----- Field -----")]
    public float maxZField;
    public float minZField;
    public float maxXField;
    public float minXField;

    [SerializeField] GameObject gameScorePanel;
    [SerializeField] GameObject touchdownAnimation;

    //Adicionados Danilo --------------------
    public int quarterCount;

    [Header("----- Aux Visual UI-----")]
    public LineRenderer _lineObjective;
    public LineRenderer _lineStartDown;
    public GameObject _objectiveImage;
    public GameObject _startImage;

    [Header("----- HUD-----")]
    TextMeshProUGUI yrdsText;
    TextMeshProUGUI qrtText;
    TextMeshProUGUI downsText;
    TextMeshProUGUI playerScoreText;
    TextMeshProUGUI enemyScoreText;
    //----------------------------------------

    //Adicionados Vitor ---------------------
    [SerializeField]
    private float timer;
    [SerializeField]
    public string niceTime;
    public static bool startTimer;
    public TextMeshProUGUI timerText;
    //----------------------------------------

    void Start()
    {
        playerPoints = 0;
        enemyPoints = 0;
        actualYards = -31.3f;
        thisDownYards = 0;
        StartingNewOffensiveDown();

        if (gameScorePanel == null)
            gameScorePanel = GameObject.Find("GameScorePanel");

        //Modificação Danilo--------
        quarterCount = 1;
        SetStartAndObjectiveLine();
        AtualizaHUD();
        //---------------------------

        //Indicators
        if (outOfField_Indicator == null)
            outOfField_Indicator = GameObject.Find("OutOfField_Indicator");

        if (missPass_Indicator == null)
            missPass_Indicator = GameObject.Find("MissPass_Indicator");

        if (sack_Indicator == null)
            sack_Indicator = GameObject.Find("Sack_Indicator");

        if (tackle_Indicator == null)
            tackle_Indicator = GameObject.Find("Tackle_Indicator");

        // isExtraTouchdown = true;
        //startTimer = true;
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            thisDownYards = actualYards - startingDownYards;
            EndingDown(transform.position.z); ;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            actualYards += 10;
            Debug.Log("actualYards: " + actualYards);

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            downStarted = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject[] a = GameObject.FindGameObjectsWithTag("EnemyTeam");
            foreach (GameObject b in a)
                b.SetActive(false);
        }


    }
#endif
    void FixedUpdate()
    {
        //start the timer
        if (downStarted)//startTimer)
        {
            timer += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
            niceTime = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            timerText.text = niceTime;
        }
        //
    }

    public void StartingDown()
    {
        startingDownYards = actualYards;
    }

    public void EndingDown(float _yards)
    {
        downStarted = false;
        actualYards = _yards;
        thisDownYards = _yards - startingDownYards;

        Invoke("CallNextStep", cdToEnd);
    }

    void CallNextStep()
    {
        //UI
        outOfField_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        sack_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        missPass_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        tackle_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

        chancesImg[downsCounter - 1].SetActive(false);

        downsCounter++;

        if (downsCounter == 5)
        {   
            startTimer = false;
            GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.GameOver);
        }
        else
            CallNextDown();
        //downsCounter++;
        //if (downsCounter < 5)
        //{
        //    CallNextDown();
        //}
        //else if (downsCounter == 4)
        //{
        //    LastDownChoices();
        //}
        //else
        //{
        //    GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.LogginEnemyDowns);
        //}
    }

    void LastDownChoices()
    {
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.ChosingLastDownAction);
    }

    void CallNextDown()
    {
        startingDownYards = actualYards;

        //Modificado Daniloo----------------------
        SetStartAndObjectiveLine();
        AtualizaHUD();
        //----------------------------------------

        //StartingNewOffensiveDown();
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Formation);


        //if (actualYards >= objectiveYards)
        //{
        //    StartingNewOffensiveDown();
        //    GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Formation);
        //}
        //else if(downsCounter == 4)
        //{
        //    //LastDownChoices();
        //}
        //else
        //{
        //    thisDownYards = 0;
        //    GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Formation);
        //}
    }

    public void StartingNewOffensiveDown()
    {
        downsCounter = 1;
        objectiveYards = actualYards + 10f;
        startingDownYards = actualYards;

        SetStartAndObjectiveLine();

        //Se o objetivo for dentro do touchdown, ajusta linha
        if (objectiveYards > maxZField)
            objectiveYards = maxZField + 1f;

        thisDownYards = 0;
    }

    public void SetExtraTouchDown()
    {
        actualYards = 42;
        objectiveYards = actualYards + 10f;
        startingDownYards = actualYards;

        //Se o objetivo for dentro do touchdown, ajusta linha
        if (objectiveYards > maxZField)
            objectiveYards = maxZField + 1f;

        thisDownYards = 0;

        SetStartAndObjectiveLine();
    }

    public void TouchDown()
    {
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.GameOver);
        downStarted = false;
        startTimer = false;
        touchdownAnimation.SetActive(true);
        touchdown = true;

        //if (!isExtraTouchdown)
        //{
        //    if (downStarted)
        //    {
        //        playerPoints += 6;
        //        AtualizaHUD();
        //        downStarted = false;
        //        touchdownAnimation.SetActive(true);
        //        downStarted = false;
        //        downsCounter = 4;
        //        Invoke("waitAndCallExtraPoint", 5f);
        //    }

        //}

        //else        
        //{
        //    if (downStarted)
        //    {
        //        playerPoints += 2;
        //        AtualizaHUD();
        //        isExtraTouchdown = false;
        //        //downStarted = false;
        //        touchdownAnimation.SetActive(true);
        //        downStarted = false;
        //        Invoke("waitAndCallLogginEnemyDowns", 5f);
        //    }

        //}
    }

    public void FieldGoal()
    {
        if (!isExtraFieldGoal)
        {
            playerPoints += 2;
            AtualizaHUD();
            GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.ChosingExtraPoint);
        }

        else
        {
            isExtraFieldGoal = false;
            playerPoints += 1;
            AtualizaHUD();
            GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.LogginEnemyDowns);
        }
    }

    //Modificação Danilo ----------------------------------
    public void SetStartAndObjectiveLine()
    {
        _lineObjective.SetPosition(0, new Vector3(-24f, -0.5f, objectiveYards));
        _lineObjective.SetPosition(1, new Vector3(24f, -0.5f, objectiveYards));
        _objectiveImage.transform.position = new Vector3(0, 1.1f, objectiveYards + 0.7f);


        _lineStartDown.SetPosition(0, new Vector3(-24f, -0.5f, startingDownYards));
        _lineStartDown.SetPosition(1, new Vector3(24f, -0.5f, startingDownYards));
        _startImage.transform.position = new Vector3(0, 1.1f, startingDownYards + 0.7f);
    }

    public void AtualizaHUD()
    {
        //if (!gameScorePanel.activeSelf)
        //    gameScorePanel.SetActive(true);

        //yrdsText = GameObject.Find("Yrds_Text").GetComponent<TextMeshProUGUI>();
        //downsText = GameObject.Find("Downs_Text").GetComponent<TextMeshProUGUI>();
        //qrtText = GameObject.Find("Qrt_Text").GetComponent<TextMeshProUGUI>();
        //playerScoreText = GameObject.Find("Player_Score_Text").GetComponent<TextMeshProUGUI>();
        //enemyScoreText = GameObject.Find("Enemy_Score_Text").GetComponent<TextMeshProUGUI>();

        //yrdsText.text = actualYards.ToString() + " Yrds";
        //downsText.text = downsCounter.ToString() + "° Try";
        //qrtText.text = quarterCount.ToString() + "Qrt";
        //playerScoreText.text = playerPoints.ToString();
        //enemyScoreText.text = enemyPoints.ToString();
    }

    void waitAndCallExtraPoint()
    {
        touchdownAnimation.SetActive(false);
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.ChosingExtraPoint);
    }

    void waitAndCallLogginEnemyDowns()
    {
        touchdownAnimation.SetActive(false);
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.LogginEnemyDowns);
    }

    //------------------------------------------------------

#if UNITY_EDITOR
    //private void OnDrawGizmos()
    //{
    //    GUIStyle style = new GUIStyle();
    //    style.alignment = TextAnchor.MiddleCenter;
    //    style.normal.textColor = Color.white;
    //    style.fontStyle = FontStyle.Bold;

    //    //Objective
    //    GizmoDrawLine(new Vector3(-24f, 1f, objectiveYards), new Vector3(24f, 1f, objectiveYards), Color.yellow, "Down", Color.yellow);

    //    //Actual
    //    GizmoDrawLine(new Vector3(-24f, 1f, startingDownYards), new Vector3(24f, 1f, startingDownYards), Color.blue, "StartingDown", Color.blue);
    //}

    //private void GizmoDrawLine(Vector3 from, Vector3 to, Color lineColor, string text, Color textColor)
    //{
    //    Handles.color = lineColor;
    //    Handles.DrawAAPolyLine(5f, from, to);
    //    Vector3 dir = (to - from).normalized;
    //    float distance = Vector3.Distance(from, to);
    //    GUIStyle style = new GUIStyle();
    //    style.alignment = TextAnchor.MiddleCenter;
    //    style.normal.textColor = textColor;

    //    Handles.Label(from + (dir * distance * .5f), text, style);
    //}
#endif
}
