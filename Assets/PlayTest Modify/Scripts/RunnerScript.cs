using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunnerScript : MonoBehaviour
{
    public Joystick _AxisController;
    public Rigidbody _rb;

    public GameObject _JoyStickCanvas;

    //Variaveis de composição de vetor de Swipe
    private Vector2[] CursorPositions = new Vector2[3];

    private bool Call1;
    private bool Call2;

    private bool _swipeCall;
    public bool _takingBreath;

    //Variaveis de composição da física do Dash
    public float _dashForce;
    public float _timeOfBreath;

    void Start()
    {
        Call1 = false;
        Call2 = false;

        _swipeCall = false;
        _takingBreath = false;
    }

    private void OnEnable()
    {
        //_JoyStickCanvas.SetActive(true);
        //  _JoyStickCanvas = GameObject.Find("Canvas_JoyStick").GetComponent<GameObject>();
        _JoyStickCanvas.SetActive(true);
        //_AxisController = _JoyStickCanvas.GetComponent<Joystick>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ObjectsTutorial")
        {
            GameObject manager = GameObject.Find("TutorialManager");
            manager.GetComponent<TutorialManager>().steps = 12;
            manager.GetComponent<TutorialManager>().canSkip = false;
            manager.GetComponent<TutorialManager>().ShowNextText();
            //manager.GetComponent<TutorialManager>().onPlay = false;
            //manager.GetComponent<TutorialManager>().Increment();
        }

    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector3(_AxisController.Horizontal * 5f, _rb.velocity.y, _AxisController.Vertical * 5f);

        if (_swipeCall)
        {
            CalcularVetor();
        }
    }

    public void OnSwipeAreaPress()
    {
        if (_takingBreath == false)
        {
            _swipeCall = true;
        }
    }

    public void OnSwipeAreaRelease()
    {
        if (_swipeCall)
        {
            Dash();
        }
        _swipeCall = false;
    }

    public void CalcularVetor()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Call1 == false)
            {
                CursorPositions[0] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Call1 = true;
            }

            if (Call2 == false && Call1 == true)
            {
                CursorPositions[1] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }
    }

    public void Dash()
    {
        //Dimenciona o VETOR TELA...
        float yVal = CursorPositions[1].y - CursorPositions[0].y;
        float xVal = CursorPositions[1].x - CursorPositions[0].x;
        Vector3 values = new Vector3 (xVal, 0, yVal);

        _rb.velocity = Vector3.zero;
        _rb.AddForce(values.normalized * _dashForce, ForceMode.Impulse);

        StartCoroutine(TakingBreath());
    }

    public IEnumerator TakingBreath()
    {
        _takingBreath = true;
        yield return new WaitForSeconds(_timeOfBreath);
        _takingBreath = false;
    }
}