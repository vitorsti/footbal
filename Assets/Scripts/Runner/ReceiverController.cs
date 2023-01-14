using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
//using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class ReceiverController : MonoBehaviour
{
    public Joystick _AxisController;
    [SerializeField] GameObject _JoyStickCanvas;

    //Variaveis de composição de vetor de Swipe
    private Vector2[] CursorPositions = new Vector2[3];
    private bool Call1;
    private bool Call2;
    private bool _swipeCall;

    [SerializeField] float velHorizontal = 2.4f;
    [SerializeField] float velVertical = 3.7f;

    //Variaveis de composição da física do Dash
    [Header("Dodge")]
    public float _dashForce = 5;
    public bool _takingBreath;
    public float _timeOfBreath = 1.5f;

    [Header("Fall")]
    public float forceToFall = 100f;
    public int tackleCounter;
    public int maxTackles = 2;

    [Header("Refs")]
    public GameObject player;
    [SerializeField] Rigidbody _rb;
    [SerializeField] GameObject outOfField_Indicator;
    [SerializeField] GameObject tackle_Indicator;

    GeneralGameHandler handler;

    private AnimationTestScript _animControll;

    private void OnTriggerEnter(Collider _trigger)
    {
        if (_trigger.gameObject.name == "Touchdown")
        {
            _rb.constraints = RigidbodyConstraints.FreezePosition;
            _JoyStickCanvas.GetComponent<Canvas>().enabled = false;
            handler.TouchDown();
        }

        if (_trigger.gameObject.tag == "OutOfField" && GeneralGameHandler.downStarted)
        {
            outOfField_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            GeneralGameHandler.downStarted = false;
            handler.EndingDown(transform.position.z);
        }
    }

    void Start()
    {
        Call1 = false;
        Call2 = false;

        _swipeCall = false;
        _takingBreath = false;

        gameObject.GetComponent<SphereCollider>().enabled = false;
        handler = FindObjectOfType<GeneralGameHandler>();

        if (outOfField_Indicator == null)
            outOfField_Indicator = GameObject.Find("OutOfField_Indicator");

        if (tackle_Indicator == null)
            tackle_Indicator = GameObject.Find("Tackle_Indicator");

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.tag == PlayerRole.EnemyTeam && GeneralGameHandler.downStarted)
        {
            float collisionForce = _collision.impulse.magnitude / Time.fixedDeltaTime;
            Debug.Log("collisionForce  " + collisionForce);

            if (collisionForce < forceToFall)
            {
                //receiverSpeed = recoveringSpeed;
                tackleCounter++;

                if (tackleCounter > maxTackles)
                {
                    tackle_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                    Fall();
                }
            }
            else
            {
                tackle_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                Fall();
            }
        }
    }

    void Fall()
    {
        _rb.constraints = RigidbodyConstraints.FreezePosition;

        if(handler == null)
            handler = FindObjectOfType<GeneralGameHandler>();

        handler.EndingDown(transform.position.z);
    }

    private void OnEnable()
    {
        //_JoyStickCanvas.SetActive(true);
        //SetUpJoystick();

        //GeneralGameHandler.downStarted = false;

        #region EventTrigger
        //EventTrigger eventTrigger = GameObject.Find("Joystick_Button").GetComponent< EventTrigger>();

        //EventTrigger eventTrigger = GetComponent<EventTrigger>();
        //List<EventTrigger.Entry> triggers = eventTrigger.triggers;
        //EventTrigger.Entry pressEventHandler = triggers.FirstOrDefault(
        //    t => t.eventID == EventTriggerType.PointerDown
        //);
        //if (pressEventHandler == null)
        //{
        //    pressEventHandler = new EventTrigger.Entry();
        //    pressEventHandler.eventID = EventTriggerType.PointerDown;
        //    triggers.Add(pressEventHandler);
        //    eventTrigger.triggers = triggers;
        //}
        //pressEventHandler.callback.AddListener(OnSwipeAreaPress);

        //EventTrigger.Entry releaseEventHandler = triggers.FirstOrDefault(
        //    t => t.eventID == EventTriggerType.PointerUp
        //);
        //if (releaseEventHandler == null)
        //{
        //    releaseEventHandler = new EventTrigger.Entry();
        //    releaseEventHandler.eventID = EventTriggerType.PointerUp;
        //    triggers.Add(releaseEventHandler);
        //    eventTrigger.triggers = triggers;
        //    eventTrigger.
        //}
        //releaseEventHandler.callback.AddListener(OnSwipeAreaRelease);

        // OnSwipeAreaPress
        // _AxisController = _JoyStickCanvas.GetComponent<Joystick>();
        #endregion
    }

    public void SetUpJoystick()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _JoyStickCanvas = GameObject.Find("Canvas_JoyStick"); //.GetComponent<GameObject>();
        _JoyStickCanvas.GetComponent<Canvas>().enabled = true;
        _JoyStickCanvas.SetActive(true);

        _animControll = GetComponent<AnimationTestScript>();

        _animControll.BoolRunTrueAnim();
        _animControll.GetBallAnim();

        _AxisController = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
    }

    private void OnDestroy()
    {
        _JoyStickCanvas.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_rb == null)
            _rb = GetComponent<Rigidbody>();

        _rb.velocity = new Vector3(_AxisController.Horizontal * velHorizontal, _rb.velocity.y, _AxisController.Vertical * velVertical);

        //_animControll._MeshTrans.LookAt(new Vector3(_AxisController.Horizontal, transform.position.y, _AxisController.Vertical));
        
        Vector2 angleVec = new Vector2(_AxisController.Horizontal, _AxisController.Vertical).normalized;
        
        /*
        float hipot = Mathf.Sqrt(Mathf.Pow(angleVec.x, 2) + Mathf.Pow(angleVec.y, 2));
        float sinOfAngleToLook = _AxisController.Vertical / hipot;
        float angleToLook = Mathf.Pow((Mathf.Sin(sinOfAngleToLook)), -1);
        Quaternion target = Quaternion.Euler(0, angleToLook, 0);
        _animControll._MeshTrans.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5f);
        */

        _animControll._MeshTrans.LookAt(new Vector3(transform.position.x + angleVec.x,transform.position.y - 1,transform.position.z + angleVec.y));

        //if (_swipeCall)
        //{
        //    CalcularVetor();
        //}
    }

    //public void OnSwipeAreaPress()
    //{
    //    if (_takingBreath == false)
    //    {
    //        _swipeCall = true;
    //    }
    //}

    //public void OnSwipeAreaRelease()
    //{
    //    if (_swipeCall)
    //    {
    //        //Dash();
    //    }
    //    _swipeCall = false;
    //}

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

    public void Dash(Vector3 force)
    {
        //Dimenciona o VETOR TELA...
        //float yVal = CursorPositions[1].y - CursorPositions[0].y;
        //float xVal = CursorPositions[1].x - CursorPositions[0].x;
        //Vector3 values = new Vector3(xVal, 0, yVal);

        //_rb.velocity = Vector3.zero;
        //_rb.AddForce(values.normalized * _dashForce, ForceMode.Impulse);

        _animControll.BreakTackleAnim();
        _rb.AddForce(force, ForceMode.Impulse);

        //StartCoroutine(TakingBreath());
    }

    //public IEnumerator TakingBreath()
    //{
    //    _takingBreath = true;
    //    yield return new WaitForSeconds(_timeOfBreath);
    //    _takingBreath = false;
    //}


    #region Old Receiver Controller
    /*
    public GameObject player;
    public float forceToFall = 100f;
    public int tackleCounter;
    public int maxTackles = 2;
    [SerializeField] GameObject outOfField_Indicator;
    [SerializeField] GameObject tackle_Indicator;

    [Header("----- Speed Properties -----")]
    public float receiverSpeed = 300f;
    [SerializeField] private float normalSpeed = 300f;
    [SerializeField] private float penalitySpeed = 100f;
    [SerializeField] private float recoveringSpeed = 10f;
    [SerializeField] private float sideModifier = 300f;
    //[SerializeField] private float dodgeSpeed = 5f;
    public Vector3 velocity = new Vector3(0, 0, 0);

    [Header("----- Dodge Properties -----")]
    [SerializeField] private float dodgeCooldown = 2.5f;
    [SerializeField] private float dodgeDistance = 400f;

    [SerializeField] private bool canDodge;
    [SerializeField] private bool dodging;
    [SerializeField] private bool recovering;

    private const float DOUBLE_TAP_TIME = 0.2f;
    private float lastClickTime;

    private float xScreenSize;
    private float yScreenSize;
    private Vector3 dodgeVec;

    Rigidbody rb;

    GeneralGameHandler handler;

    private void OnTriggerEnter(Collider _trigger)
    {
        if (_trigger.gameObject.name == "Touchdown")
        {
            GeneralGameHandler.downStarted = false;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            handler.TouchDown();
        }

        if (_trigger.gameObject.tag == "OutOfField" && GeneralGameHandler.downStarted)
        {
            outOfField_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            GeneralGameHandler.downStarted = false;
            handler.EndingDown(transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.tag == PlayerRole.EnemyTeam && GeneralGameHandler.downStarted)
        {
            float collisionForce = _collision.impulse.magnitude / Time.fixedDeltaTime;
            Debug.Log("collisionForce  " + collisionForce);

            if (collisionForce < forceToFall)
            {
                receiverSpeed = recoveringSpeed;
                tackleCounter++;

                if (tackleCounter > maxTackles)
                {
                    tackle_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                    Fall();
                }
            }
            else
            {
                tackle_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                Fall();
            }
        }
    }

    void Start()
    {
        tackleCounter = 0;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        rb = GetComponent<Rigidbody>();
        player = gameObject;
        handler = FindObjectOfType<GeneralGameHandler>();
        xScreenSize = Screen.width;
        yScreenSize = Screen.height;
        recovering = false;
        canDodge = true;
        gameObject.GetComponent<Renderer>().material.color = Color.black;

        if (outOfField_Indicator == null)
            outOfField_Indicator = GameObject.Find("OutOfField_Indicator");

        if (tackle_Indicator == null)
            tackle_Indicator = GameObject.Find("Tackle_Indicator");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            //DoubleTap
            if (timeSinceLastClick <= DOUBLE_TAP_TIME && canDodge)
            {
                //Dodge Left Side
                if (Input.mousePosition.x <= xScreenSize / 2 && Input.mousePosition.y <= yScreenSize/2)
                {
                    //dodgeVec = new Vector3(player.transform.position.x - dodgeDistance, player.transform.position.y, player.transform.position.z);
                    dodgeVec = new Vector3(-dodgeDistance, 0, velocity.z / 15);

                    SetDodge();
                }
                //Dodge Right Side
                else if (Input.mousePosition.x > xScreenSize / 2 && Input.mousePosition.y <= yScreenSize / 2)
                {
                    //dodgeVec = new Vector3(player.transform.position.x + dodgeDistance, player.transform.position.y, player.transform.position.z);
                    dodgeVec = new Vector3(dodgeDistance, 0, velocity.z / 15);

                    SetDodge();
                }
                //Dodge Left Diagonal
                else if (Input.mousePosition.x <= xScreenSize / 2 && Input.mousePosition.y > yScreenSize / 2)
                {
                    //dodgeVec = new Vector3(player.transform.position.x - (dodgeDistance/2), player.transform.position.y, player.transform.position.z + (dodgeDistance/2));
                    dodgeVec = new Vector3(-dodgeDistance/2, 0, velocity.z / 10 + dodgeDistance / 5);

                    SetDodge();
                }
                //Dodge Right Diagonal
                else if (Input.mousePosition.x > xScreenSize / 2 && Input.mousePosition.y > yScreenSize / 2)
                {
                    //dodgeVec = new Vector3(player.transform.position.x + (dodgeDistance/2), player.transform.position.y, player.transform.position.z + (dodgeDistance/2));
                    dodgeVec = new Vector3(dodgeDistance / 2, 0, velocity.z / 10 + dodgeDistance / 5);

                    SetDodge();
                }
            }

            lastClickTime = Time.time;
        }

        if (Input.GetMouseButton(0) && !dodging)
        {
            //Move Left
            if (Input.mousePosition.x <= xScreenSize / 2)
            {
                //player.transform.position = new Vector3(player.transform.position.x - sideModifier * Time.deltaTime, player.transform.position.y, player.transform.position.z);
                velocity = new Vector3(-sideModifier, 0, receiverSpeed);
                //rb.AddForce(velocity, ForceMode.Force);
            }
            //Move Right
            else
            {
                //player.transform.position = new Vector3(player.transform.position.x + sideModifier * Time.deltaTime, player.transform.position.y, player.transform.position.z);
                velocity = new Vector3(sideModifier, 0, receiverSpeed);
                //rb.AddForce(velocity);
            }
        }
        else
        {
            velocity = new Vector3(0, 0, receiverSpeed);
        }

        if (dodging)
            Dodge();
        else
        {

        }

        if (receiverSpeed < normalSpeed)
            receiverSpeed += recoveringSpeed;         
    }

    private void FixedUpdate()
    {
        //    if (GeneralGameHandler.downStarted)
        //        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 4f)
        //            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;// gameObject.GetComponent<Rigidbody>().velocity.normalized * 4;
        //}

        if (dodging)
            Dodge();
        else
        {
            //if(velocity.magnitude < 100f)
            //    rb.AddForce(velocity, ForceMode.Force);
            //else
            //    rb.AddForce(0, 0, 100, ForceMode.Force);

            rb.AddForce(velocity, ForceMode.Force);
        }
    }

    void Dodge()
    {
        //dodgeVec = new Vector3(dodgeVec.x, player.transform.position.y, dodgeVec.z);
        //player.transform.position = Vector3.Slerp(player.transform.position, dodgeVec, Time.deltaTime * dodgeSpeed);

        rb.AddForce(dodgeVec, ForceMode.Impulse);
        dodging = false;
        receiverSpeed = penalitySpeed;

        //if (Vector3.Distance(player.transform.position, dodgeVec) <= 0.1f)
        //    dodging = false;
    }

    void SetDodge()
    {
        //receiverSpeed = recoveringSpeed;
        canDodge = false;
        dodging = true;
        Invoke("DodgeReset", dodgeCooldown);
    }

    void DodgeReset()
    {
        canDodge = true;
    }

    void Fall()
    {
        Debug.Log("FALL");
        rb.constraints = RigidbodyConstraints.FreezePosition;
        handler.EndingDown(transform.position.z);
    }
    */
    #endregion
}
