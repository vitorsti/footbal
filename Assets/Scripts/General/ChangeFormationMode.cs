using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFormationMode : MonoBehaviour
{
    public GameObject position;
    public GameObject pathing;
    GameObject formation;
    public GameObject pathingOptions;

    private void Start()
    {
        formation = GameObject.Find("FormationHandler");
        pathingOptions = GameObject.Find("PathingsOptions");
        //Positions();
    }

    private void OnEnable()
    {
        Positions();
    }

    public void Positions()
    {
        position.SetActive(true);
        pathing.SetActive(false);
        pathingOptions.SetActive(false);
        formation.GetComponent<FormationZones>().enabled = true;
        formation.GetComponent<FormationPathingHandler>().enabled = false;
    }

    public void Pathings()
    {
        position.SetActive(false);
        pathing.SetActive(true);
        pathingOptions.SetActive(true);
        formation.GetComponent<FormationZones>().enabled = false;
        formation.GetComponent<FormationPathingHandler>().enabled = true;
    }
}
