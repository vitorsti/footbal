using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FormationsListHandler : MonoBehaviour
{
    public GameObject[] preSetList;
    public GameObject button;
    public GameObject listCanvas;
    public GameObject scorePanel;

    public GameObject changeFormationMode;
    public GameObject startPlay;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        scorePanel = GameObject.Find("GameScorePanel");
        changeFormationMode = GameObject.Find("ChangeFormationMode");
        startPlay = GameObject.Find("StartDownButton");

        foreach (GameObject formation in preSetList)
        {
            GameObject obj = Instantiate(button);
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            obj.GetComponent<FormationListButton>().formation = formation;
            obj.transform.parent = listCanvas.transform;
            obj.name = formation.name;
            text.text = obj.name;
        }
    }

    private void OnEnable()
    {
        if(scorePanel == null)
            scorePanel = GameObject.Find("GameScorePanel");

        if (startPlay == null)
            startPlay = GameObject.Find("StartDownButton");

        if (changeFormationMode == null)
            changeFormationMode = GameObject.Find("ChangeFormationMode");
    }

    private void OnDisable()
    {

    }

    public void DisableStartButton()
    {
        if (startPlay == null)
            startPlay = GameObject.Find("StartDownButton");

        startPlay.SetActive(false);
    }

    public void DisableFormationMode()
    {
        if (changeFormationMode == null)
            changeFormationMode = GameObject.Find("ChangeFormationMode");

        changeFormationMode.SetActive(false);
    }

    public void EnableFormationMode()
    {
        changeFormationMode.SetActive(true);
    }

    public void EnbaleStartButton()
    {
        startPlay.SetActive(true);
    }

    public void GameScorePanel()
    {
        //if (scorePanel.activeSelf == true)
        //    scorePanel.SetActive(false);
        //else
        //    scorePanel.SetActive(true);
    }
}
