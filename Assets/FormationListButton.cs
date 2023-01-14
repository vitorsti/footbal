using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationListButton : MonoBehaviour
{
    public GameObject formation;

    public void SpawnFormation()
    {
        //GameObject[] old = GameObject.FindGameObjectsWithTag(PlayerRole.Teammate);
        //foreach (GameObject obj in old)
        //    Destroy(obj);

        GameObject[] old = GameObject.FindGameObjectsWithTag(PlayerRole.Formation);
        foreach (GameObject obj in old)
            Destroy(obj);

        //GameObject old = GameObject.FindGameObjectWithTag(PlayerRole.Formation);
        //if(old != null)
        //Debug.Log(old.gameObject.name);
        //else        Debug.Log("4234");

        //Destroy(old);

        Instantiate(formation, new Vector3(0, 0, GeneralGameHandler.actualYards), Quaternion.identity);
    }
}
