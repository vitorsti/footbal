using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
    public GameObject carryingTheBall;
    [SerializeField] Rigidbody ballRB;
    [SerializeField] GameObject outOfField_Indicator;
    [SerializeField] GameObject missPass_Indicator;

    [Header("----- Snap -----")]
    [SerializeField] bool snaping = false;
    [SerializeField] float snapSpeed = 4f;
    [SerializeField] float timeUntilSnap = 2f;

    [Header("----- Ball -----")]
    public bool flying;
    [SerializeField] bool passing;
    public float forceToIntercept = 3f;

    //[Header("----- Modifiers -----")]
    //[SerializeField] float nearThrow = 1.3f;
    //[SerializeField] float farThrow = 1.25f;

    GameObject target;
    GameObject qb;
    float passForce;
    GeneralGameHandler handler;

    public Vector3 throwPos { get; private set; }

    private void OnTriggerEnter(Collider _trigger)
    {
        if (_trigger.gameObject.tag == "OutOfField" && GeneralGameHandler.downStarted)
        {
            outOfField_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            GeneralGameHandler.downStarted = false;
            handler.EndingDown(handler.startingDownYards);
        }
    }

    private void OnCollisionEnter(Collision _collision)
    {
        //Debug.Log("ball colission " + _collision.gameObject.name);

        if (_collision.gameObject.name == PlayerRole.Quarterback)
        {
            snaping = false;
            carryingTheBall = _collision.gameObject;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject);
        }

        if (flying)
        {
            //Miss Throw
            if (_collision.gameObject.name == "Football_Camp" && GeneralGameHandler.downStarted)
            {
                missPass_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                GeneralGameHandler.downStarted = false;
                flying = false;

                #if UNITY_EDITOR    //Editor
                Debug.Log(analytic);
                #endif

                handler.EndingDown(handler.startingDownYards);
            }

            //Sucessful Throw
            if (!passing && (_collision.gameObject.name == PlayerRole.WideReceiver || _collision.gameObject.name == PlayerRole.HalfBack || _collision.gameObject.name == PlayerRole.TightEnd))
            {
                flying = false;
                carryingTheBall = _collision.gameObject;
                ballRB.useGravity = true;
                gameObject.SetActive(false);
                _collision.gameObject.GetComponent<SetUpPlayers>().SetUpReceiver();
                GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.LoadingRunner);
            }

            if(passing && _collision.gameObject.tag != PlayerRole.EnemyTeam)
            {

            }

            //Sucessful Pass
            if (passing && (_collision.gameObject.name == PlayerRole.HalfBack || _collision.gameObject.name == PlayerRole.TightEnd))
            {
                flying = false;
                passing = false;
                ballRB.constraints = RigidbodyConstraints.None;
                carryingTheBall = _collision.gameObject;
                gameObject.SetActive(false);
                _collision.gameObject.GetComponent<SetUpPlayers>().SetUpReceiver();
                GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.LoadingRunner);
            }

            //Intercept
            if (_collision.gameObject.tag == PlayerRole.EnemyTeam)
            {
                float collisionForce = _collision.impulse.magnitude / Time.fixedDeltaTime;
                Debug.Log("enemy colision force:   " + collisionForce);
                ballRB.constraints = RigidbodyConstraints.None;
                ballRB.useGravity = true;
                passing = false;
                
                //if (collisionForce <= forceToIntercept)
                //{
                //    GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.LogginEnemyDowns);
                //}
                //else
                //{
                //    handler.EndingDown(handler.startingDownYards);
                //}
            }
        }
    }

    void Start()
    {
        //ballRB = GetComponent<Rigidbody>();
        qb = GameObject.Find(PlayerRole.Quarterback);
        handler = FindObjectOfType<GeneralGameHandler>();

        if (outOfField_Indicator == null)
            outOfField_Indicator = GameObject.Find("OutOfField_Indicator");

        if (missPass_Indicator == null)
            missPass_Indicator = GameObject.Find("MissPass_Indicator");
    }

    void Update()
    {
        if (GeneralGameHandler.downStarted)
        {
            if (snaping)
                transform.position = Vector3.Slerp(transform.position, qb.transform.position, snapSpeed * Time.deltaTime);

            //if (passing && flying)
            //{
            //    Passing();
            //}

            if (carryingTheBall != null)
            {
                //gameObject.SetActive(false);
            }
        }

        #if UNITY_EDITOR //Editor
        setAnalytic();
        #endif
    }

    /*
    public void isPass(GameObject _target, float _force)
    {
        ballRB.constraints = RigidbodyConstraints.FreezePositionY;
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        carryingTheBall = null;
        flying = true;
        passing = true;
        target = _target;
        passForce = _force;
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }

    void Passing()
    {
        ballRB.useGravity = false;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 20f * Time.deltaTime);
    }

    public void LauchBall(Vector3 _mousepos, float _ballForce)
    {
        throwPos = new Vector3(_mousepos.x, 1, _mousepos.z);
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        float zPos;

        if ((_mousepos.z - qb.transform.position.z) > 15)
        {
            zPos = (_mousepos.z - qb.transform.position.z) / farThrow;
            _mousepos.x = _mousepos.x / farThrow;
        }
        else
        {
            zPos = (_mousepos.z * nearThrow - qb.transform.position.z * nearThrow);
            _mousepos.x = _mousepos.x * nearThrow;
        }

        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;

        ballRB.AddForce(_mousepos.x, _ballForce, zPos, ForceMode.Impulse);
        ballRB.constraints = RigidbodyConstraints.None;

        carryingTheBall = null;
        flying = true;
        Debug.Log(_mousepos + "    Lauch:  " + _ballForce);
    }
    */

    public void StartThePlay()
    {
        ballRB.constraints = RigidbodyConstraints.None;
        ballRB.constraints = RigidbodyConstraints.FreezePositionY;
        snaping = true;
        GetComponent<Collider>().enabled = true;
    }


#if UNITY_EDITOR //Editor
    public Vector3 analytic;

    void setAnalytic()
    {
        if (transform.position.y > analytic.y)
            analytic.y = transform.position.y;

        if (GeneralGameHandler.downStarted)
        {
            analytic.x = transform.position.x;
            analytic.z = transform.position.z;
        }
    }
#endif
}
