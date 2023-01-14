using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpPlayers : MonoBehaviour
{
    public void SetUpPlayer()
    {
        if (GameStateManager.GameState == GameStateManager.StateOfTheGame.Quarterback)
        {
            switch (gameObject.name)
            {
                case PlayerRole.Center:
                    gameObject.GetComponent<CenterPathing>().SetUpPathing();
                    break;
                case PlayerRole.OffensiveGuard:
                    gameObject.GetComponent<OffensiveGuard>().SetUpPathing();
                    break;
                case PlayerRole.OffensiveTackle:
                    gameObject.GetComponent<OffensiveTackle>().SetUpPathing();
                    break;
                case PlayerRole.DefensiveEnd:
                    gameObject.GetComponent<DefensiveLinePathing>().SetUpPathing();
                    break;
                case PlayerRole.DefensiveTackle:
                    gameObject.GetComponent<DefensiveLinePathing>().SetUpPathing();
                    break;
                case PlayerRole.HalfBack:
                    gameObject.GetComponent<HalfBackPathing>().SetUpPathing();
                    gameObject.GetComponent<LineRenderer>().enabled = false;
                    break;
                case PlayerRole.Quarterback:
                    gameObject.GetComponent<QuarterbackController>();
                    break;
                case PlayerRole.WideReceiver:
                    gameObject.GetComponent<WideReceiverPathing>().SetUpPathing();
                    gameObject.GetComponent<LineRenderer>().enabled = false;
                    break;
                case PlayerRole.MiddleLineBacker:
                    gameObject.GetComponent<MiddleLinebackerPathing>().SetUpPathing();
                    break;
                case PlayerRole.OutsideLineBacker:
                    gameObject.GetComponent<OutsideLinebackerPathing>().SetUpPathing();
                    break;
                case PlayerRole.Cornerback:
                    gameObject.GetComponent<CornerbackPathing>().SetUpPathing();
                    break;
                case PlayerRole.TightEnd:
                    gameObject.GetComponent<TightEndPathing>().SetUpPathing();
                    gameObject.GetComponent<LineRenderer>().enabled = false;
                    break;
                case PlayerRole.Safety:
                    gameObject.GetComponent<SafetyPathing>().SetUpPathing();
                    break;
            }
        }

        if (GameStateManager.GameState == GameStateManager.StateOfTheGame.Runner)
        {
            switch (gameObject.name)
            {
                case PlayerRole.Center:
                    gameObject.GetComponent<CenterPathing>().enabled = false;
                    gameObject.GetComponent<TeammateController>().enabled = true;
                    break;
                case PlayerRole.OffensiveGuard:
                    gameObject.GetComponent<OffensiveGuard>().enabled = false;
                    gameObject.GetComponent<TeammateController>().enabled = true;
                    break;
                case PlayerRole.OffensiveTackle:
                    gameObject.GetComponent<OffensiveTackle>().enabled = false;
                    gameObject.GetComponent<TeammateController>().enabled = true;
                    break;
                case PlayerRole.DefensiveEnd:
                    gameObject.GetComponent<DefensiveLinePathing>().enabled = false;
                    gameObject.GetComponent<TackleController>().enabled = true;
                    break;
                case PlayerRole.DefensiveTackle:
                    gameObject.GetComponent<DefensiveLinePathing>().enabled = false;
                    gameObject.GetComponent<TackleController>().enabled = true;
                    break;
                case PlayerRole.HalfBack:
                    gameObject.GetComponent<HalfBackPathing>().enabled = false;
                    gameObject.GetComponent<TeammateController>().enabled = true;
                    break;
                case PlayerRole.Quarterback:
                    gameObject.GetComponent<QuarterbackController>().enabled = false;
                    gameObject.GetComponent<TeammateController>().enabled = true;
                    break;
                case PlayerRole.WideReceiver:
                    gameObject.GetComponent<WideReceiverPathing>().enabled = false;
                    gameObject.GetComponent<TeammateController>().enabled = true;
                    break;
                case PlayerRole.MiddleLineBacker:
                    gameObject.GetComponent<MiddleLinebackerPathing>().enabled = false;
                    gameObject.GetComponent<TackleController>().enabled = true;
                    break;
                case PlayerRole.OutsideLineBacker:
                    gameObject.GetComponent<OutsideLinebackerPathing>().enabled = false;
                    gameObject.GetComponent<TackleController>().enabled = true;
                    break;
                case PlayerRole.Cornerback:
                    gameObject.GetComponent<CornerbackPathing>().enabled = false;
                    gameObject.GetComponent<TackleController>().enabled = true;
                    break;
                case PlayerRole.TightEnd:
                    gameObject.GetComponent<TightEndPathing>().enabled = false;
                    gameObject.GetComponent<TeammateController>().enabled = true;
                    break;
                case PlayerRole.Safety:
                    gameObject.GetComponent<SafetyPathing>().enabled = false;
                    gameObject.GetComponent<TackleController>().enabled = true;
                    break;
                case PlayerRole.Receiver:
                    gameObject.GetComponent<TeammateController>().enabled = false;
                    break;
            }
        }
    }

    public void SetUpReceiver()
    {
        switch (gameObject.name)
        {
            case PlayerRole.HalfBack:
                gameObject.GetComponent<HalfBackPathing>().enabled = false;
                gameObject.AddComponent<ReceiverController>();          
                break;
            case PlayerRole.WideReceiver:
                gameObject.GetComponent<WideReceiverPathing>().enabled = false;
                gameObject.AddComponent<ReceiverController>();
                break;
            case PlayerRole.TightEnd:
                gameObject.GetComponent<TightEndPathing>().enabled = false;
                gameObject.AddComponent<ReceiverController>();
                break;
            case PlayerRole.Quarterback:
                gameObject.GetComponent<QuarterbackController>().enabled = false;
                gameObject.AddComponent<ReceiverController>();

                break;
        }

        gameObject.name = PlayerRole.Receiver;
    }
}
