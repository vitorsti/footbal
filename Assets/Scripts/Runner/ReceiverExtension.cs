using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverExtension : MonoBehaviour
{
    public ReceiverController receiver;

    //Variaveis de composição de vetor de Swipe
    private Vector2[] CursorPositions = new Vector2[3];
    private bool Call1;
    private bool Call2;
    private bool _swipeCall;

    [SerializeField] float velHorizontal = 3f;
    [SerializeField] float velVertical = 5f;


    //Variaveis de composição da física do Dash
    [Header("Dodge")]
    public float _dashForce = 5;
    public bool _takingBreath;
    public float _timeOfBreath = 1.5f;

    public void OnSwipeAreaPress()
    {
        if (receiver == null)
            receiver = GameObject.Find(PlayerRole.Receiver).GetComponent<ReceiverController>();

        if (_takingBreath == false)
        {
            _swipeCall = true;
        }

        //receiver.OnSwipeAreaPress();
    }

    public void OnSwipeAreaRelease()
    {
        if (receiver == null)
            receiver = GameObject.Find(PlayerRole.Receiver).GetComponent<ReceiverController>();

        if (_swipeCall)
        {
            Dash();
        }
        _swipeCall = false;

        //receiver.OnSwipeAreaRelease();
    }

    void Update()
    {
        if (_swipeCall)
        {
            CalcularVetor();
        }

        if (GameStateManager.GameState != GameStateManager.StateOfTheGame.Runner)
            gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void CalcularVetor()
    {
        if (Input.GetKey(KeyCode.Mouse0))
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
        Vector3 values = new Vector3(xVal, 0, yVal);

        //velocity = Vector3.zero;
        //_rb.AddForce(values.normalized * _dashForce, ForceMode.Impulse);
        Vector3 force = values.normalized * _dashForce;

        if (receiver == null)
            receiver = GameObject.Find(PlayerRole.Receiver).GetComponent<ReceiverController>();

        receiver.Dash(force);

        StartCoroutine(TakingBreath());
    }

    public IEnumerator TakingBreath()
    {
        _takingBreath = true;
        yield return new WaitForSeconds(_timeOfBreath);
        _takingBreath = false;
    }
}
