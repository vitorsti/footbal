using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuoteGenerateScript : MonoBehaviour
{
    private string enemyName = "Cardinals";
    private int actualTry;
    private int actualQuoteType;
    private bool endQuotes;
    [HideInInspector]
    public List<string> frasesDoLog;

    public int yrdsWin;

    private void Start()
    {
        LimparParaGerarNovas();
    }

    public void GerarFrases()
    {
        if (endQuotes == false)
        {
            if (actualQuoteType == 1)
            {
                frasesDoLog.Add(TryQuote());
                actualQuoteType = 2;
            }
            else if (actualQuoteType == 2)
            {
                int rdProbability = Random.Range(0, 10000);

                if (rdProbability < 1000)
                {
                    string[] frases = AcidentQuote();

                    frasesDoLog.Add(frases[0]);
                    frasesDoLog.Add(frases[1]);
                    frasesDoLog.Add(frases[2]);
                }
                else
                {
                    frasesDoLog.Add(CompleteTry());
                }
                actualQuoteType = 1;
            }

            GerarFrases();

        }
        else
        {
            Debug.Log("Todas as consequências foram geradas!");
        }
    }

    public string TryQuote()
    {
        string TryQuote = " ";

        if (actualTry < 4)
        {
            TryQuote = "O time " + enemyName + " vai tentar o " + actualTry + "ºTry";
            actualTry++;
        }
        else if (actualTry == 4)
        {
            int randomNbr = Random.Range(0, 10000);

            if (randomNbr < 2000)
            {
                TryQuote = "O time " + enemyName + " vai tentar o " + actualTry + "ºTry";
            }
            else if (randomNbr < 10000)
            {
                TryQuote = "O time " + enemyName + " chutou a bola, e ela foi interceptada em " + Random.Range(15,40) + "Yrds a frente!";
                endQuotes = true;
            }
        }

        return TryQuote;
    }

    public string CompleteTry()
    {
        int rdPoints = Random.Range(0, 10000);
        string totalPoints = " ";

        if (rdPoints < 100)
        {
            totalPoints = "Em uma jogada incrível, o time " + enemyName + " conseguiu avançar 30 yrds!";
            yrdsWin += 30;
        }
        else if (rdPoints < 1000)
        {
            totalPoints = "Com essa ótima jogada, o time " + enemyName + " conseguiu avançar 25 yrds!";
            yrdsWin += 25;
        }
        else if (rdPoints < 3000)
        {
            totalPoints = "Com essa boa jogada, time " + enemyName + " conseguiu avançar 20 yrds!";
            yrdsWin += 20;
        }
        else if (rdPoints < 5000)
        {
            totalPoints = "O time " + enemyName + " conseguiu avançar 15 yrds!";
            yrdsWin += 15;
        }
        else if (rdPoints < 9000)
        {
            totalPoints = "O time " + enemyName + " conseguiu avançar 10 yrds!";
            yrdsWin += 10;
        }
        else if (rdPoints < 10000)
        {
            totalPoints = "O time " + enemyName + " não conseguiu completar o lance!";
        }

        if (actualTry == 5)
        {
            endQuotes = true;
        }

        return totalPoints;
    }

    public string[] AcidentQuote()
    {
        string teamName = " ";
        string teamName2 = " ";
        string injuryName = " ";
        string bodyPartName = " ";
        string InjuryQuote = " ";
        string FoulQuote = " ";
        string PenalityQuote = " ";

        int rdPlayerNbr = Random.Range(1, 100);
        int rdPlayerNbr2 = Random.Range(1, 100);
        int rdTeamName = Random.Range(0, 1000);
        int rdInjuryName = Random.Range(0, 10000);
        int rdBodyName = Random.Range(0, 10000);

        if (rdTeamName < 500)
        {
            teamName = "Gorilas";
            teamName2 = "Cardinals";
        }
        else
        {
            teamName = "Cardinals";
            teamName2 = "Gorilas";
        }

        if (rdInjuryName < 100)
        {
            injuryName = " quebrou";

            if (teamName == "Gorilas")
            {
                PenalityQuote = "O time " + enemyName + " perdeu 15 Yrds!";
                yrdsWin -= 15;
            }
            else
            {
                PenalityQuote = "O time " + enemyName + " ganhou 15 Yrds!";
                yrdsWin += 15;
            }
        }
        else if (rdInjuryName < 1000)
        {
            injuryName = " lacerou";

            if (teamName == "Gorilas")
            {
                PenalityQuote = "O time " + enemyName + " perdeu 10 Yrds!";
                yrdsWin -= 10;
            }
            else
            {
                PenalityQuote = "O time " + enemyName + " ganhou 10 Yrds!";
                yrdsWin += 10;
            }
        }
        else if (rdInjuryName < 10000)
        {
            injuryName = " contundiu";

            if (teamName == "Gorilas")
            {
                PenalityQuote = "O time " + enemyName + " perdeu 5 Yrds!";
                yrdsWin -= 5;
            }
            else
            {
                PenalityQuote = "O time " + enemyName + " ganhou 5 Yrds!";
                yrdsWin += 5;
            }
        }

        if (rdBodyName < 100)
        {
            bodyPartName = " a cabeça";
        }
        else if (rdBodyName < 1000)
        {
            bodyPartName = " o ombro";
        }
        else if (rdBodyName < 3000)
        {
            bodyPartName = " o braço";
        }
        else if (rdBodyName < 5000)
        {
            bodyPartName = " o peito";
        }
        else if (rdBodyName < 8000)
        {
            bodyPartName = " o joelho";
        }
        else if (rdBodyName < 10000)
        {
            bodyPartName = " a perna";
        }

        InjuryQuote = "O jogador camisa " + rdPlayerNbr + " do time " + teamName + injuryName + bodyPartName + "!";
        FoulQuote = "O jogador camisa " + rdPlayerNbr2 + " do time " + teamName2 + " foi penalizado!";

        string[] acidentQuotes = new string[] { InjuryQuote, FoulQuote, PenalityQuote };

        if (actualTry == 5)
        {
            endQuotes = true;
        }

        return acidentQuotes;
    }
    
    public void LimparParaGerarNovas()
    {
        frasesDoLog = new List<string>();
        frasesDoLog.Clear();
        yrdsWin = 0;
        actualTry = 1;
        actualQuoteType = 1;
        endQuotes = false;
    }
}
