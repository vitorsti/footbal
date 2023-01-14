using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public bool canSkip;
    public bool onPlay;
    public int steps;

    public int dummies;

    [Header("Message")]
    public GameObject[] messages;

    [Header("Objects")]
    public GameObject stadium;
    public GameObject qb;

    public GameObject runner;
    public GameObject analogic;

    public GameObject dummy;
    public GameObject dummy2;

    public GameObject obstacle;

    public GameObject ol;
    public GameObject defenseTeam;
    public GameObject offensiveTeam;

    public GameObject objectiveLine;
    public GameObject startLine;

    public GameObject camera;
    public GameObject formationHandler;
    public GameObject textPositions;
    public GameObject posButtons;
    public GameObject pathingButton;

    // Start is called before the first frame update
    void Start()
    {
        dummies = 2;
        messages[steps].SetActive(true);
        InvokeRepeating("ResetSkip", 0, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSkip && !onPlay)
        {
            steps++;
            ShowNextText();
        }
    }

    public void ShowNextText()
    {
        canSkip = false;
        messages[steps - 1].SetActive(false);
        messages[steps].SetActive(true);

        switch (steps)
        {
            case 4:
                ShowQB(true);
                break;

            case 7:
                onPlay = true;
                ShowDummies(true);
                break;

            case 8:
                ShowQB(false);
                break;

            case 10:
                analogic.SetActive(true);
                break;

            case 11:
                onPlay = true;
                ShowRunner(true);
                Invoke("ShowObstacle", 1.5f);
                break;

            case 12:
                onPlay = false;
                break;

            case 13:
                onPlay = false;
                obstacle.SetActive(false);
                ShowRunner(false);
                break;

            case 17:
                ShowFormation(true);
                FocusOL();
                break;

            case 18:
                offensiveTeam.SetActive(false);
                qb.SetActive(false);
                break;

            case 20:
                qb.SetActive(true);
                break;

            case 21:
                offensiveTeam.SetActive(true);
                FocusTighetEnd();
                break;

            case 22:
                FocusFormation();
                break;

            case 23:
                formationHandler.SetActive(true);
                break;

            case 26:
                onPlay = true;
               //posButtons.SetActive(true);
                Invoke("ShowPositionsButton", 5f);
                formationHandler.SetActive(true);
                break;

            case 30:
                onPlay = true;
                Invoke("ShowPathingButton", 5f);
                formationHandler.GetComponent<FormationZones>().enabled = false;
                formationHandler.GetComponent<FormationPathingHandler>().enabled = true;
                break;  

            case 31:
                onPlay = false;
                pathingButton.SetActive(false);
                ol.SetActive(true);
                FocusEverything();
                break;

            case 32:
                onPlay = false;
                pathingButton.SetActive(false);
                ol.SetActive(true);
                qb.SetActive(true);
                qb.GetComponent<TutorialQB>().enabled = false;
                defenseTeam.SetActive(true);
                //FocusEverything();
                break;

            case 35:
                ol.SetActive(false);
                qb.SetActive(false);
                offensiveTeam.SetActive(false);
                stadium.SetActive(false);
                defenseTeam.SetActive(false);
                break;

            case 39:
                stadium.SetActive(true);
                objectiveLine.SetActive(true);
                startLine.SetActive(true);
                ol.SetActive(true);
                qb.SetActive(true);
                defenseTeam.SetActive(true);
                break;

            case 41:
                stadium.SetActive(false);
                objectiveLine.SetActive(false);
                startLine.SetActive(false);
                ol.SetActive(false);
                qb.SetActive(false);
                defenseTeam.SetActive(false);
                break;

            case 42:
                PlayerPrefs.SetInt("DONE_TUTORIAL", 1);
                SceneManager.LoadScene(2, LoadSceneMode.Single);
                break;
        }
    }

    void ShowQB(bool toggle)
    {
        stadium.SetActive(toggle);
        qb.SetActive(toggle);
    }

    void ShowDummies(bool toggle)
    {
        dummy.SetActive(toggle);
        dummy2.SetActive(toggle);
    }
    void ShowRunner(bool toggle)
    {
        stadium.SetActive(toggle);
        analogic.SetActive(toggle);
        runner.SetActive(toggle);

        if (toggle == true)
        {
            camera.transform.position = new Vector3(runner.transform.position.x, runner.transform.position.y + 1.5f, runner.transform.position.z - 6f);
            camera.transform.parent = runner.transform;
        }
        else
            camera.transform.parent = transform;
    }

    void ShowFormation(bool toggle)
    {
        stadium.SetActive(toggle);
        ol.SetActive(toggle);
        offensiveTeam.SetActive(toggle);
        qb.SetActive(true);
        qb.GetComponent<TutorialQB>().enabled = false;
    }

    void FocusOL()
    {
        camera.GetComponent<Camera>().orthographic = true;
        camera.transform.position = new Vector3(0, 10, 0);
        camera.transform.Rotate(new Vector3(90, 270, 0));
        camera.GetComponent<Camera>().orthographicSize = 5;
    }

    void FocusTighetEnd()
    {
        camera.GetComponent<Camera>().orthographic = true;
        camera.transform.position = new Vector3(3, 10, 0);
        //camera.transform.Rotate(new Vector3(90, 270, 0));
        camera.GetComponent<Camera>().orthographicSize = 7;
    }

    void FocusFormation()
    {
        camera.GetComponent<Camera>().orthographic = true;
        camera.transform.position = new Vector3(1, 10, 13);
        //camera.transform.Rotate(new Vector3(90, 270, 0));
        camera.GetComponent<Camera>().orthographicSize = 23;
    }
    public void CheckPositions()
    {
        GameObject obj = GameObject.Find("BANK");

        if(obj != null)
        {
            textPositions.SetActive(true);
        }
        else
        {
            textPositions.SetActive(false);
            posButtons.SetActive(false);
            onPlay = false;
            Increment();
        }
    }

    public void FocusEverything()
    {
        camera.GetComponent<Camera>().orthographic = true;
        camera.transform.position = new Vector3(0, 10, 0);
        //camera.transform.Rotate(new Vector3(90, 270, 0));
        camera.GetComponent<Camera>().orthographicSize = 23;
    }

    public void CheckPathings()
    {
        onPlay = false;
        Increment();
    }

    void ShowObstacle()
    {
        obstacle.SetActive(true);
        steps++;
    }

    public void Increment()
    {
        steps++;
        ShowNextText();
    }

    void ShowPathingButton()
    {
        pathingButton.SetActive(true);
    }

    void ShowPositionsButton()
    {
        posButtons.SetActive(true);
    }

    void ResetSkip()
    {
        canSkip = true;
    }

    [SerializeField] GameObject[] enemyTeam;
    [SerializeField] GameObject[] playerTeam;

    void GetEnemyTeam()
    {
        enemyTeam = new GameObject[] { };
        enemyTeam = GameObject.FindGameObjectsWithTag(PlayerRole.EnemyTeam);
    }

    void GetPlayerTeam()
    {
        playerTeam = new GameObject[] { };
        playerTeam = GameObject.FindGameObjectsWithTag(PlayerRole.Teammate);
    }

    void StartPlay()
    {
        GetEnemyTeam();
        GetPlayerTeam();

        foreach (GameObject player in playerTeam)
            player.GetComponent<SetUpPlayers>().SetUpPlayer();

        foreach (GameObject enemy in enemyTeam)
            enemy.GetComponent<SetUpPlayers>().SetUpPlayer();

        GeneralGameHandler.downStarted = true;
    }
}
