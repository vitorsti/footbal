using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingScript : MonoBehaviour
{
    //Variaveis de composição do vetor do chute
    private Vector2[] CursorPositions = new Vector2[2];

    private bool Call1;
    private bool Call2;

    private float _xScreenSize;
    private float _yScreenSize;

    private float _xMaxDeslocation;
    private float _yMaxDeslocation;

    //Variaveis de composição da física do chute
    private float _fowardPosition;
    private float _sidePosition;

    //public GameObject _ball;
    public int _maxForceFoward;
    public int _maxForceSides;

    //Referências externas
    public GameObject _targetPrefab;
    private GameObject _instanciaAlvo;
    public GameObject _ballPrefab;
    private GameObject _instanciaBall;
    public bool _alvoOn;
    public float _forcaPersonagem;

    private void Start()
    {
        _xScreenSize = Screen.width;
        _yScreenSize = Screen.height;

        _xMaxDeslocation = _xScreenSize * 0.8f;
        _yMaxDeslocation = _yScreenSize * 0.4f;

        Call1 = false;
        Call2 = false;
        _alvoOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        PassingCalculator();
    }

    private void PassingCalculator()
    {
        //Quando estiver clicando na tela...
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Faça a primeira validação...
            if (Call1 == false)
            {
                //Guarde a primeira posição que foi clicada na tela...
                CursorPositions[0] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                //Finalize a primeira validação;
                Call1 = true;
            }

            //Quando a primeira validação acabar, começe a segunda validação, pois, agora o plyer vai deslizar o dedo na tela para tras...
            if (Call2 == false && Call1 == true)
            {
                //Marque o segundo ponto da tela, até soltar o dedo...
                CursorPositions[1] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                MostrarPosicaoNoCampo();
            }
        }

        //Quando soltar o dedo da tela, faça os calculos!
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            FinalizarPasse();
        }
    }

    public void MostrarPosicaoNoCampo()
    {
        //Instancia uma versão do alvo logo abaixo do quarterback...
        if (_alvoOn == false)
        {
            //Vector3 qbPos = transform.position;
            _instanciaAlvo = Instantiate(_targetPrefab, this.transform.position, Quaternion.identity);
            //_instanciaAlvo.transform.position = qbPos;
            _alvoOn = true;
        }

        //Converte o vetor da tela, em um vetor em campo...
        //Dimenciona o VETOR TELA...
        float yVal = CursorPositions[0].y - CursorPositions[1].y;
        float xVal = CursorPositions[0].x - CursorPositions[1].x;

        //Não deixa ele ultrapassar um limite estipulado...
        if (yVal > _yMaxDeslocation)
        {
            yVal = _yMaxDeslocation;
        }

        if (xVal > 0)
        {
            if (xVal > _xMaxDeslocation)
                xVal = _xMaxDeslocation;
        }
        else
        {
            if (xVal < -_xMaxDeslocation)
                xVal = -_xMaxDeslocation;
        }

        //Dimenciona o VETOR CAMPO...

        _fowardPosition = (yVal * _maxForceFoward) / _yMaxDeslocation;
        _sidePosition = (xVal * _maxForceSides) / _xMaxDeslocation;

        Vector3 finalPos = new Vector3(_sidePosition, 0, _fowardPosition);
        _instanciaAlvo.transform.position = new Vector3(transform.position.x + _sidePosition, 0, _fowardPosition + transform.position.z);

    }

    public void FinalizarPasse()
    {
        //Executando uma força de arremeço...
        Vector3 vetorCampo = new Vector3(_instanciaAlvo.transform.position.x, 0, _instanciaAlvo.transform.position.z);
        float _S = vetorCampo.magnitude;
        float t =  3f / _forcaPersonagem;
        float _Vx = _S / t;
        float _Vy = (Physics.gravity.magnitude) * _S / (2 * _forcaPersonagem);

        var lookPos = _instanciaAlvo.transform.position - transform.position;
        lookPos.y = 0;

        _instanciaBall = Instantiate(_ballPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.LookRotation(lookPos));

        _instanciaBall.GetComponent<Rigidbody>().AddRelativeForce(0, _Vy, _forcaPersonagem, ForceMode.Impulse);
        Destroy(_instanciaAlvo);
        LimpaArray();
    }

    public void LimpaArray()
    {
        CursorPositions[0] = Vector2.zero;
        CursorPositions[1] = Vector2.zero;

        Call1 = false;
        Call2 = false;

        _alvoOn = false;
    }

}
