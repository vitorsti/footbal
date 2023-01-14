using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuarterbackController : MonoBehaviour
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

    //Referências externas
    [Header("Refs")]
    [SerializeField] GameObject sack_Indicator;
    [SerializeField] GameObject ball;

    public GameObject _targetPrefab;
    [SerializeField] GameObject _ballPrefab;

    private GameObject _instanciaAlvo;
    private GameObject _instanciaBall;

    [SerializeField] bool _alvoOn;
    float _charForce = 25;

    GeneralGameHandler handler;

    public bool canThrow;
    [SerializeField] bool hasBall;
    public bool afterSnap;
    bool sack;

    private AnimationTestScript _animControll;
    private bool _backWalkStart;

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.name == "Ball" && !afterSnap)
        {
            //ball = _collision.gameObject;
            //ball.transform.parent = transform;
            //ball.GetComponent<Rigidbody>().isKinematic = true;
            //ball.SetActive(false);
            _collision.gameObject.GetComponent<GameBallController>().BallSnap(gameObject);
            afterSnap = true;
            hasBall = true;
        }

        if (_collision.gameObject.tag == PlayerRole.EnemyTeam && gameObject.name == PlayerRole.Quarterback && hasBall && !sack)
        {
            sack = true;
            sack_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            GeneralGameHandler.downStarted = false;
            Invoke("Sack", 2f);
        }
    }
    void Start()
    {
        _animControll = GetComponent<AnimationTestScript>();
        _backWalkStart = true;
        handler = FindObjectOfType<GeneralGameHandler>();

        _xScreenSize = Screen.width;
        _yScreenSize = Screen.height;

        _xMaxDeslocation = _xScreenSize * 0.8f;
        _yMaxDeslocation = _yScreenSize * 0.4f;

        Call1 = false;
        Call2 = false;
        _alvoOn = false;

        if (sack_Indicator == null)
            sack_Indicator = GameObject.Find("Sack_Indicator");

    }
    void Update()
    {
        if (hasBall && GeneralGameHandler.downStarted)
        {
            if (_backWalkStart)
            {
                _animControll.QBBackWalkAnim();
                _backWalkStart = false;
            }

            PassingCalculator();
        }
    }

    private void PassingCalculator()
    {
        if (canThrow)
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
    }

    public void MostrarPosicaoNoCampo()
    {
        //Instancia uma versão do alvo logo abaixo do quarterback...
        if (_alvoOn == false)
        {
            _instanciaAlvo = Instantiate(_targetPrefab, this.transform.position, Quaternion.identity);
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
        _fowardPosition = (yVal * _charForce) / _yMaxDeslocation;
        _sidePosition = (xVal * _charForce) / _xMaxDeslocation;

        _instanciaAlvo.transform.position = new Vector3(transform.position.x + _sidePosition, 0, _fowardPosition + transform.position.z);

        _animControll._MeshTrans.LookAt(new Vector3(_instanciaAlvo.transform.position.x,transform.position.y - 1, _instanciaAlvo.transform.position.z));
    }

    public void FinalizarPasse()
    {
        //Executando uma força de arremeço...
        Vector3 vetorCampo = new Vector3(_sidePosition, 0, _fowardPosition);
        Debug.Log(vetorCampo);
        //float _S = vetorCampo.magnitude;
        //float t = 3f / _forcaPersonagem;
        //float 1.5f = val / _forcaPersonagem;
        //float _Vx = _S / 0.5f;
        //float _Vy = (Physics.gravity.magnitude) * _S / (2 * _forcaPersonagem);

        var lookPos = _instanciaAlvo.transform.position - transform.position;
        lookPos.y = 0;

        float _Vy = 0;
        float _Vx = 0;

        //Executa a força do arremesso, relativo a distância do alvo...
        if (vetorCampo.magnitude > 15)
        {
            // _Vy = (_charForce / 2) * Mathf.Sqrt(2);
            // _Vx = (_charForce / 2) * Mathf.Sqrt(2);

            float t = 1.5f;

            _Vx = vetorCampo.magnitude / t;
            _Vy = _Vx / 2f;
        }
        else if (vetorCampo.magnitude > 5 && vetorCampo.magnitude <= 15)
        {
            // _Vy = (_charForce / 4) * Mathf.Sqrt(2);
            // _Vx = (_charForce / 2)* Mathf.Sqrt(2);

            float t = 1f;

            _Vx = vetorCampo.magnitude / t;
            _Vy = _Vx / 2;
        }
        else if (vetorCampo.magnitude > 1 && vetorCampo.magnitude <= 5)
        {
            //_Vy = (_charForce / 8) * Mathf.Sqrt(2);
            //_Vx = (_charForce / 4) * Mathf.Sqrt(2);

            float t = 0.5f;

            _Vx = vetorCampo.magnitude / t;
            _Vy = _Vx / 8;
        }
        else if (vetorCampo.magnitude > 0 && vetorCampo.magnitude <= 1)
        {
            //_Vy = (_charForce / 16) * Mathf.Sqrt(2);
            //_Vx = (_charForce / 8) * Mathf.Sqrt(2);

            float t = 0.15f;

            _Vx = vetorCampo.magnitude / t;
            _Vy = _Vx / 16;
        }

        //_instanciaBall = Instantiate(_ballPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.LookRotation(lookPos));
        ball = Instantiate(_ballPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 1), Quaternion.LookRotation(lookPos));
        ball.name = "Ball";
        ball.GetComponent<GameBallController>().flying = true;
        ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        //ball.GetComponent<CapsuleCollider>().enabled = true;
        //ball.GetComponent<MeshRenderer>().enabled = true;
        //ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //ball.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z + 1f);
        //ball.transform.rotation = Quaternion.LookRotation(lookPos);

        ball.GetComponent<Rigidbody>().AddRelativeForce(0, _Vy, _Vx, ForceMode.Impulse);
        canThrow = false;
        hasBall = false;

        Destroy(_instanciaAlvo);
        LimpaArray();

        _animControll.QBThrowBallAnim();
    }

    public void LimpaArray()
    {
        CursorPositions[0] = Vector2.zero;
        CursorPositions[1] = Vector2.zero;

        Call1 = false;
        Call2 = false;

        _alvoOn = false;
    }

    void Sack()
    {
        handler.EndingDown(handler.startingDownYards);
    }
    public void SetUpPlayer()
    {

    }

    #region Old QuarterBack
    /*
    [SerializeField] GameObject ball;

    [Header("----- General -----")]
    [SerializeField] bool hasBall;
    public bool willThrow;
    [SerializeField] GameObject sack_Indicator;

    [Header("----- Speed Properties -----")]
    public float qbSpeed;
    [SerializeField] float normalSpeed;
    [SerializeField] float qbWithBallSpeed;

    [Header("----- Force Properties -----")]
    public float ballForce;
    [SerializeField] float ballForceModifier;
    [SerializeField] float passForce;

    //Vars
    Vector3 mousepos;
    GeneralGameHandler handler;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && hasBall)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;

            //Pass
            if (Physics.Raycast(ray, out hitData, 100) &&
               (hitData.transform.position.z < (handler.startingDownYards + 3f)) &&
               (hitData.transform.name == PlayerRole.HalfBack ||
                hitData.transform.name == PlayerRole.TightEnd))
            {
                hasBall = false;
                PassBall(hitData.transform.gameObject);
            }

            //Run QB
            else if (Physics.Raycast(ray, out hitData, 300, 9) && (hitData.transform.name == PlayerRole.Quarterback))
            {
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                rb.drag = 2f;
                rb.mass = 100;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.2f, gameObject.transform.position.z);

                hasBall = false;
                gameObject.GetComponent<SetUpPlayers>().SetUpReceiver();
                GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Runner);
            }
        }

        if (Input.GetMouseButton(0) && hasBall)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 1000))
                //Throw Ball
                if (Physics.Raycast(ray, out hitData, 100) && (hitData.transform.name == "Football_Camp" || hitData.transform.name == PlayerRole.WideReceiver))
                {
                    willThrow = true;
                    mousepos = hitData.point;
                    ballForce += ballForceModifier;
                }
        }

        if (Input.GetMouseButtonUp(0) && hasBall && willThrow)
        {
            ThrowBall();
        }

        if (hasBall)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (qbWithBallSpeed * Time.deltaTime));
        }
    }

    void PassBall(GameObject _teammate)
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        gameObject.transform.GetComponent<Collider>().isTrigger = true;
        ball.SetActive(true);
        ball.GetComponent<GameBallController>().isPass(_teammate, passForce);
        hasBall = false;
    }

    void ThrowBall()
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        gameObject.transform.GetComponent<Collider>().isTrigger = true;
        ball.SetActive(true);
        ball.GetComponent<GameBallController>().LauchBall(mousepos, ballForce);
        hasBall = false;
    }
    */
    #endregion
}
