using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameStateCaller
{
    public static void CallNextState(GameStateManager.StateOfTheGame _state)
    {
        GameObject.FindObjectOfType<MiniGamesTransition>().CallNextStep(_state);
    }
}

public class MiniGamesTransition : MonoBehaviour
{
    CameraTrackingHandler cameraHandler;
    [SerializeField] GameObject inGameBall;

    [Header("Scene Handlers")]
    [SerializeField] GameObject formationHandler;
    [SerializeField] GameObject pathingOptions;
    [SerializeField] GameObject generalHandler;
    [SerializeField] GameObject fieldGoalHandler;

    [Header("UI")]
    [SerializeField] GameObject extraPointChoice;
    [SerializeField] GameObject lastDownChoice;
    [SerializeField] GameObject changeFormationMode;
    //[SerializeField] GameObject gameScorePanel;
    [SerializeField] GameObject startDownButton;
    [SerializeField] GameObject preSetList;
    [SerializeField] GameObject gameOverWindow;
    [SerializeField] GameObject chancesPanel;


    public GameObject EnemyLogUI;

    [Header("Game Objs")]
    [SerializeField] GameObject ball;
    [SerializeField] GameObject offensiveLine;
    [SerializeField] GameObject QB;
    [SerializeField] GameObject qbTarget;

    [Header("PreSet Formations")]
    [SerializeField] List<GameObject> enemyFormationsPreSet = new List<GameObject>();
    [SerializeField] List<GameObject> teamFormationsPreSet = new List<GameObject>();

    [Header("Teams Objs")]
    [SerializeField] GameObject[] enemyTeam;
    [SerializeField] GameObject[] playerTeam;

    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraTrackingHandler>();

        //changeFormationMode = Instantiate(Resources.Load<GameObject>("Handlers/ChangeFormationMode"));
        //extraPointChoice = Instantiate(Resources.Load<GameObject>("Handlers/ExtraPointOptions"));
        //lastDownChoice = Instantiate(Resources.Load<GameObject>("Handlers/LastDownChoices"));
        //pathingOptions = Instantiate(Resources.Load<GameObject>("Handlers/PathingsOptions"));
        //gameScorePanel = Instantiate(Resources.Load<GameObject>("Handlers/GameScorePanel"));
        //fieldGoalHandler = Instantiate(Resources.Load<GameObject>("Handlers/FieldGoalHandler"));

        formationHandler = GameObject.Find("FormationHandler");
        generalHandler = GameObject.Find("GeneralHandler");

        startDownButton = GameObject.Find("StartDownButton");
        preSetList = GameObject.Find("PreSetListHandler");
        changeFormationMode = GameObject.Find("ChangeFormationMode");
        gameOverWindow = GameObject.Find("GameOverWindow");

        //offensiveLine = Resources.Load<GameObject>("Team/OL");
        //QB = Resources.Load<GameObject>("Team/QB");
        //ball = Resources.Load<GameObject>("General/GameBall");

        pathingOptions.name = "PathingsOptions";
        //gameScorePanel.name = "GameScorePanel";

        LoadPreSets();
    }

    // Start is called before the first frame update
    void Start()
    {
        formationHandler.SetActive(false);

        enemyTeam = new GameObject[] { };
        playerTeam = new GameObject[] { };

        extraPointChoice.SetActive(false);
        lastDownChoice.SetActive(false);

        CallNextStep(GameStateManager.StateOfTheGame.Formation);
    }

    public void CallNextStep(GameStateManager.StateOfTheGame _state)
    {
        //Debug.Log("foi: " + _state);
        lastDownChoice.SetActive(false);

        //formationHandler.SetActive(false);
        //generalHandler.SetActive(false);

        switch (_state)
        {
            case GameStateManager.StateOfTheGame.Formation:
                SetMiniGameFormation();
                break;

            case GameStateManager.StateOfTheGame.LoadingQB:
                SetLoadingQB();
                break;

            case GameStateManager.StateOfTheGame.Quarterback:
                SetMiniGameQuarterback();
                break;

            case GameStateManager.StateOfTheGame.Runner:
                SetMiniGameRunner();
                break;

            case GameStateManager.StateOfTheGame.LoadingRunner:
                SetLoadingRunner();
                break;

            case GameStateManager.StateOfTheGame.FieldGoal:
                //SetMiniGameFieldGoal();
                break;

            case GameStateManager.StateOfTheGame.RunningBack:
                //SetMiniGameRunningBack();
                break;

            case GameStateManager.StateOfTheGame.KickingBack:
                //SetMiniGameKickingBack();
                break;

            case GameStateManager.StateOfTheGame.Touchdown:
                SetTouchdown();
                break;

            case GameStateManager.StateOfTheGame.ExtraGoal:
               // SetMiniGameExtraGoal();
                break;

            case GameStateManager.StateOfTheGame.ExtraTouchDown:
                SetMiniGameExtraTouchDown();
                break;

            case GameStateManager.StateOfTheGame.ChosingLastDownAction:
                //SetChosingLastDownAction();
                break;

            case GameStateManager.StateOfTheGame.ChosingExtraPoint:
                //SetChosingExtraPoint();
                break;

            case GameStateManager.StateOfTheGame.LogginEnemyDowns:
                //SetLogginEnemyDowns();
                break;

            case GameStateManager.StateOfTheGame.GameOver:
                SetGameOver();
                break;
        }
    }

    private void SetLoadingRunner()
    {
        GameStateManager.GameState = GameStateManager.StateOfTheGame.LoadingRunner;
        Time.timeScale = 0.1f;
        
        //SetMiniGameRunner();
        Invoke("SetMiniGameRunner", 0.4f);
    }

    private void SetGameOver()
    {
        GameStateManager.GameState = GameStateManager.StateOfTheGame.GameOver;

        gameOverWindow.GetComponent<Canvas>().enabled = true;

        if (GeneralGameHandler.touchdown)
        {

        }
        else
        {

        }
    }

    void SetMiniGameFormation()
    {
        GameStateManager.GameState = GameStateManager.StateOfTheGame.Formation;

        //generalHandler.GetComponent<GeneralGameHandler>().StartingDown();
        Random.InitState(System.DateTime.Now.Millisecond);

        GameObject[] dots = GameObject.FindGameObjectsWithTag("PathingDots");

        //------------ Quarterback
        GameObject qb = Instantiate(QB, new Vector3(0, 0, GeneralGameHandler.actualYards - 6f), Quaternion.identity);
        qb.name = PlayerRole.Quarterback;
        qb.GetComponent<QuarterbackController>()._targetPrefab = qbTarget;

        cameraHandler.GetComponent<CameraTrackingHandler>().SetTarget();

        CleanPreSets();
        //------------ Teams
        if (enemyTeam.Length != 0)
            foreach (GameObject player in playerTeam)
                Destroy(player);

        if (enemyTeam.Length != 0)
            foreach (GameObject enemy in enemyTeam)
                Destroy(enemy);

        //------------ Handlers
        startDownButton.SetActive(true);
        preSetList.SetActive(true);
        generalHandler.SetActive(true);
        changeFormationMode.SetActive(true);
        formationHandler.SetActive(true);
        //Invoke("HideScorePanel", 3f);

        //------------ Offensive Line
        GameObject ol = GameObject.Find(offensiveLine.name);
        if (ol != null)
            Destroy(ol);

        ol = Instantiate(offensiveLine, new Vector3(0, 0, GeneralGameHandler.actualYards - 1.5f), Quaternion.identity);
        ol.name = offensiveLine.name;

        Instantiate(teamFormationsPreSet[Random.Range(0, teamFormationsPreSet.Count)], new Vector3(0, 0, GeneralGameHandler.actualYards), Quaternion.identity);
    }


    public void SetLoadingQB()
    {
        GameStateManager.GameState = GameStateManager.StateOfTheGame.LoadingQB;
        cameraHandler.GetComponent<CameraTrackingHandler>().SetTarget();

        //------------ Handlers
        changeFormationMode.SetActive(false);
        formationHandler.SetActive(false);
        pathingOptions.SetActive(false);
        //gameScorePanel.SetActive(true);
        chancesPanel.SetActive(true);


        //------------ Teams
        GetPlayerTeam();
        Instantiate(enemyFormationsPreSet[Random.Range(0, enemyFormationsPreSet.Count)], new Vector3(0, 0, GeneralGameHandler.actualYards + 0.5f), Quaternion.identity);
        GetEnemyTeam();

        //------------ Ball
        inGameBall = GameObject.Find("Ball");
        if (inGameBall != null)
            Destroy(inGameBall);

        inGameBall = Instantiate(ball, new Vector3(0, 1, GeneralGameHandler.actualYards - 2.5f), Quaternion.identity);//.name = "Ball";
        inGameBall.name = "Ball";

        Time.timeScale = 1f;
        Invoke("SetMiniGameQuarterback", 3f);
    }

    void SetMiniGameQuarterback()
    {
        GameStateManager.GameState = GameStateManager.StateOfTheGame.Quarterback;
        cameraHandler.GetComponent<CameraTrackingHandler>().SetTarget();

        //GameObject inGameBall = GameObject.FindObjectOfType<GameBallController>().gameObject;

        //------------ Players
        foreach (GameObject player in playerTeam)
            player.GetComponent<SetUpPlayers>().SetUpPlayer();
            
        foreach (GameObject enemy in enemyTeam)
            enemy.GetComponent<SetUpPlayers>().SetUpPlayer();

        //------------ Handlers
        generalHandler.SetActive(true);
        //gameScorePanel.SetActive(false);
        chancesPanel.SetActive(false);


        //------------ Start
        GeneralGameHandler.downStarted = true;
        GeneralGameHandler.startTimer = true;
        QB.GetComponent<QuarterbackController>().enabled = true;
        inGameBall.GetComponent<GameBallController>().StartSnap();
        Time.timeScale = 1f;


    }

    void SetMiniGameRunner()
    {
        GameStateManager.GameState = GameStateManager.StateOfTheGame.Runner;

        GameObject obj = GameObject.Find("Receiver");
        obj.GetComponent<ReceiverController>().SetUpJoystick();

        GeneralGameHandler.downStarted = true;
        Debug.Log(GameStateManager.GameState);

        cameraHandler.GetComponent<CameraTrackingHandler>().SetTarget();

        foreach (GameObject player in playerTeam)
            player.GetComponent<SetUpPlayers>().SetUpPlayer();

        foreach (GameObject enemy in enemyTeam)
            enemy.GetComponent<SetUpPlayers>().SetUpPlayer();


        //GameObject.Find(PlayerRole.Receiver).AddComponent<ReceiverController>();
    }

    public void SetMiniGameFieldGoal()
    {
        //GameStateManager.GameState = GameStateManager.StateOfTheGame.FieldGoal;
        GeneralGameHandler.playerPoints++;
        SetLogginEnemyDowns();

    }

    void SetMiniGameRunningBack()
    {
         GameStateManager.GameState = GameStateManager.StateOfTheGame.RunningBack;

    }

    void SetMiniGameKickingBack()
    {
        //GameStateManager.GameState = GameStateManager.StateOfTheGame.KickingBack;
        SetLogginEnemyDowns();
    }

    void SetTouchdown()
    {
        GameStateManager.GameState = GameStateManager.StateOfTheGame.Touchdown;

        
    }

    void SetMiniGameExtraGoal()
    {
        extraPointChoice.SetActive(false);

        SetLogginEnemyDowns();

        //GameStateManager.GameState = GameStateManager.StateOfTheGame.ExtraGoal;

        //extraPointChoice.SetActive(false);
        //GeneralGameHandler.isExtraFieldGoal = true;

        //CallNextStep(GameStateManager.StateOfTheGame.FieldGoal);
    }

    void SetMiniGameExtraTouchDown()
    {
        GameStateManager.GameState = GameStateManager.StateOfTheGame.ExtraTouchDown;

        //GeneralGameHandler.actualYards = 42;
        generalHandler.GetComponent<GeneralGameHandler>().SetExtraTouchDown();
        extraPointChoice.SetActive(false);
        GeneralGameHandler.isExtraTouchdown = true;

        CallNextStep(GameStateManager.StateOfTheGame.Formation);
    }

    void SetLogginEnemyDowns()
    {
        GameStateManager.GameState = GameStateManager.StateOfTheGame.LogginEnemyDowns;

        GeneralGameHandler.actualYards = Random.Range(-20, 0);
        generalHandler.GetComponent<GeneralGameHandler>().StartingNewOffensiveDown();

        CleanAllTeams();

        preSetList.SetActive(false);
        startDownButton.SetActive(false);
        changeFormationMode.SetActive(false);
        extraPointChoice.SetActive(false);

        EnemyLogUI.GetComponent<LogScript>().StartLog();
    }

    void SetChosingLastDownAction()
    {
        GameStateManager.GameState = GameStateManager.StateOfTheGame.ChosingLastDownAction;
        lastDownChoice.SetActive(true);
    }

    void SetChosingExtraPoint()
    {
        extraPointChoice.SetActive(true);
    }

    void GetEnemyTeam()
    {
        enemyTeam = new GameObject[] { };
        enemyTeam = GameObject.FindGameObjectsWithTag(PlayerRole.EnemyTeam);
    }

    void LoadPreSets()
    {
        if (enemyFormationsPreSet.Count < 10)
        {
            enemyFormationsPreSet = new List<GameObject>();
            enemyFormationsPreSet = Resources.LoadAll<GameObject>("EnemyFormations/").ToList();
        }

        if (teamFormationsPreSet.Count < 10)
        {
            teamFormationsPreSet = new List<GameObject>();
            teamFormationsPreSet = Resources.LoadAll<GameObject>("TeamFormations/").ToList();
        }
    }

    void GetPlayerTeam()
    {
        playerTeam = new GameObject[] { };
        playerTeam = GameObject.FindGameObjectsWithTag(PlayerRole.Teammate);
    }

    void HideScorePanel()
    {
        //if (gameScorePanel == null)
        //    GameObject.Find("GameScorePanel");
        //gameScorePanel.SetActive(false);

        if (chancesPanel == null)
            GameObject.Find("ChancesUI");

        chancesPanel.SetActive(false);
    }

    public void CleanAllTeams()
    {
        GetEnemyTeam();
        GetPlayerTeam();

        if (enemyTeam.Length != 0)
            foreach (GameObject player in playerTeam)
                Destroy(player);

        if (enemyTeam.Length != 0)
            foreach (GameObject enemy in enemyTeam)
                Destroy(enemy);
    }

    void CleanPreSets()
    {
        GameObject[] presets;
        presets = new GameObject[] { };
        presets = GameObject.FindGameObjectsWithTag(PlayerRole.Formation);
        

    }
}