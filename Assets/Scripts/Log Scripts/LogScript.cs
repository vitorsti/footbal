using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LogScript : MonoBehaviour
{

    private QuoteGenerateScript _qGS;

    public GameObject _TextBox;
    public GameObject _LogTextHolder;
    public Button _GoBtn;
    public Button _SpdBtn;
    public Button _SkpBtn;

    public Scrollbar _verticalScrol;

    private int _speedControll = 3;
    private bool canSkip;

    private List<string> frasesAleatorias = new List<string>();
    private int quoteCnt = 0;
    private List<GameObject> boxJaCriadas = new List<GameObject>();

    private void Awake()
    {
        ClearValuesVariables();
        _qGS = GetComponent<QuoteGenerateScript>();
    }

    public void SpeedBtnControll()
    {
        if (_speedControll == 1)
        {
            _speedControll = 3;
            _SpdBtn.GetComponentInChildren<Text>().text = "Speed x1";
        }
        else if (_speedControll == 2)
        {
            _speedControll = 1;
            _SpdBtn.GetComponentInChildren<Text>().text = "Speed x3";
        }
        else if (_speedControll == 3)
        {
            _speedControll = 2;
            _SpdBtn.GetComponentInChildren<Text>().text = "Speed x2"; 
        }
    }

    public void SkipBtnControll()
    {
        if (canSkip)
        {
            StopAllCoroutines();

            for (int i = 0; i < boxJaCriadas.Count; i++)
            {
                Destroy(boxJaCriadas[i]);
            }

            boxJaCriadas.Clear();
            quoteCnt = 0;

            for (int b = quoteCnt; b < frasesAleatorias.Count; b++)
            {
                GerarBoxFinais();
            }

            canSkip = false;
        }
        else
        {
            Debug.Log("Já precionou skip uma vez!");
        }
    }

    public void GerarBoxFinais()
    {
        if (quoteCnt < frasesAleatorias.Count)
        {
            GameObject NewBox = Instantiate(_TextBox, _LogTextHolder.transform.position, Quaternion.identity, _LogTextHolder.transform);
            boxJaCriadas.Add(NewBox);
            string boxQuote = frasesAleatorias[quoteCnt];
            quoteCnt++;
            NewBox.GetComponentInChildren<Text>().text = boxQuote;

            if (quoteCnt == frasesAleatorias.Count - 1)
            {
                _GoBtn.interactable = true;
            }

            _verticalScrol.value = 0;
        }
    }

    public IEnumerator DelayToGerateBoxs()
    {
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < _qGS.frasesDoLog.Count; i++)
        {
            frasesAleatorias.Add(_qGS.frasesDoLog[i]);
        }

        StartCoroutine(GerarTextBox());
    }

    public IEnumerator GerarTextBox()
    {

        if (quoteCnt < frasesAleatorias.Count)
        {
            GameObject NewBox = Instantiate(_TextBox, _LogTextHolder.transform.position, Quaternion.identity, _LogTextHolder.transform);
            boxJaCriadas.Add(NewBox);
            _verticalScrol.value = 0;
            string boxQuote = frasesAleatorias[quoteCnt];
            quoteCnt++;

            NewBox.GetComponentInChildren<Text>().DOText("...", _speedControll/2, false, ScrambleMode.None, null);
            yield return new WaitForSeconds(_speedControll/2);
            NewBox.GetComponentInChildren<Text>().text = " ";
            NewBox.GetComponentInChildren<Text>().DOText("...", _speedControll/2, false, ScrambleMode.None, null);
            yield return new WaitForSeconds(_speedControll/2);
            NewBox.GetComponentInChildren<Text>().text = " ";
            NewBox.GetComponentInChildren<Text>().DOText("...", _speedControll / 2, false, ScrambleMode.None, null);
            yield return new WaitForSeconds(_speedControll / 2);

            NewBox.GetComponentInChildren<Text>().DOText(boxQuote, _speedControll, false, ScrambleMode.None, null);
            yield return new WaitForSeconds(_speedControll);

            StartCoroutine(GerarTextBox());

            if (quoteCnt == frasesAleatorias.Count -1)
            {
                _GoBtn.interactable = true;
            }
        }
    }

    private void ClearValuesVariables()
    {
        canSkip = true;
        frasesAleatorias.Clear();
        boxJaCriadas.Clear();
        frasesAleatorias = new List<string>();
        boxJaCriadas = new List<GameObject>();
    }

    public void StartLog()
    {
        //MiniGamesTransition transition = GameObject.FindObjectOfType<MiniGamesTransition>();
        //transition.CleanAllTeams();

        ClearValuesVariables();
        gameObject.SetActive(true);
        StartCoroutine(DelayGerarFrases());
    }
    private IEnumerator DelayGerarFrases()
    {
        yield return new WaitForSeconds(0.25f);
        _qGS.GerarFrases();
        StartCoroutine(DelayToGerateBoxs());
    }

    public void OnGoPlay()
    {
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Formation);


        _qGS.LimparParaGerarNovas();
        for (int i = 0; i < boxJaCriadas.Count; i++)
        {
            Destroy(boxJaCriadas[i]);
        }

        ClearValuesVariables();
        gameObject.SetActive(false);
    }
}
