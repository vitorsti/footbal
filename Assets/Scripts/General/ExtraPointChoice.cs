using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraPointChoice : MonoBehaviour
{
    public void ExtraKick()
    {
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.ExtraGoal);
    }

    public void ExtraTouchdown()
    {
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.ExtraTouchDown);
    }
}
