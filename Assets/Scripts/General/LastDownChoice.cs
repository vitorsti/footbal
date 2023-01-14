using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDownChoice : MonoBehaviour
{
    [SerializeField] GameObject kickBall;
    [SerializeField] GameObject fieldGoal;

    private void OnEnable()
    {
        if (GeneralGameHandler.actualYards > 50)
        {
            fieldGoal.SetActive(true);
            kickBall.SetActive(false);
        }
        else
        {
            fieldGoal.SetActive(false);
            kickBall.SetActive(true);
        }
    }

    public void FieldGoal()
    {
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.FieldGoal);
    }

    public void KickBack()
    {
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.LogginEnemyDowns);
    }

    public void TryDown()
    {
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Formation);
    }
}
