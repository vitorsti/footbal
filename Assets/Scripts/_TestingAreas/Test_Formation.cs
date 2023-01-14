using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Formation : MonoBehaviour
{
    [Header("----- PRE SET -----")]
    public GameObject presetToTest;

    [Header("----- etc -----")]
    public GameObject formationHandler;

    GameObject ball;
    GameObject offensiveLine;
    GameObject QB;

    GameObject changeFormationMode;
    public GameObject pathingOptions;


    private void Awake()
    {
        offensiveLine = Resources.Load<GameObject>("Team/OL");
        QB = Resources.Load<GameObject>("Team/QB");
        ball = Resources.Load<GameObject>("General/GameBall"); 
    }

    void Start()
    {
        // generalHandler.GetComponent<GeneralGameHandler>().StartingDown();

        //if (enemyTeam.Length != 0)
        //    foreach (GameObject player in playerTeam)
        //        Destroy(player);

        //if (enemyTeam.Length != 0)
        //    foreach (GameObject enemy in enemyTeam)
        //        Destroy(enemy);

        // changeFormationMode.SetActive(true);

        Instantiate(offensiveLine, new Vector3(0, 0, GeneralGameHandler.actualYards - 0.35f), Quaternion.identity);
        Instantiate(QB, new Vector3(0, 0, GeneralGameHandler.actualYards - 5f), Quaternion.identity).name = PlayerRole.Quarterback;
        Instantiate(presetToTest, new Vector3(0, 0, GeneralGameHandler.actualYards), Quaternion.identity);

        Invoke("ResetZone", 0.1f);
    }

    void ResetZone()
    {
        formationHandler.SetActive(false);

        Invoke("Render", 0.1f);

    }

    void Render()
    {
        formationHandler.SetActive(true);

    }
}
