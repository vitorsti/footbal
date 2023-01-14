using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameBallController : MonoBehaviour
{
    public GameObject carryingTheBall;
    [SerializeField] Rigidbody ballRB;

    [Header("----- Indicator -----")]
    [SerializeField] GameObject outOfField_Indicator;
    [SerializeField] GameObject missPass_Indicator;
    [SerializeField] GameObject intercept_Indicator;

    [Header("----- Snap -----")]
    [SerializeField] bool snaping = false;
    [SerializeField] float snapSpeed = 4f;
    [SerializeField] float timeUntilSnap = 2f;

    [Header("----- Ball -----")]
    public bool flying;
    [SerializeField] bool passing;
    public float forceToIntercept = 330f;

    [SerializeField] GameObject qb;
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

    public void BallSnap(GameObject _obj)
    {
        snaping = false;
        carryingTheBall = _obj.gameObject;
        _obj.gameObject.GetComponent<QuarterbackController>().canThrow = true;
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        //Debug.Log("ball colission " + _collision.gameObject.name);

        if (_collision.gameObject.name == PlayerRole.Quarterback)
        {
            if (!_collision.gameObject.GetComponent<QuarterbackController>().afterSnap)
            {
                BallSnap(_collision.gameObject);
                //snaping = false;
                //carryingTheBall = _collision.gameObject;
                ////GetComponent<CapsuleCollider>().enabled = false;
                //_collision.gameObject.GetComponent<QuarterbackController>().canThrow = true;
                //GetComponent<MeshRenderer>().enabled = false;
                //Destroy(gameObject);
            }
        }

        if (flying)
        {
            //Miss Throw
            if (_collision.gameObject.name == "Field" && GeneralGameHandler.downStarted)
            {
                missPass_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                GeneralGameHandler.downStarted = false;
                flying = false;
                Destroy(gameObject, 1.5f);
                handler.EndingDown(handler.startingDownYards);
            }

            //Sucessful Throw
            if (!passing && (_collision.gameObject.name == PlayerRole.WideReceiver || _collision.gameObject.name == PlayerRole.HalfBack || _collision.gameObject.name == PlayerRole.TightEnd))
            {
                flying = false;
                carryingTheBall = _collision.gameObject;
                ballRB.useGravity = true;
                //gameObject.SetActive(false);
                _collision.gameObject.GetComponent<SetUpPlayers>().SetUpReceiver();
                GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.LoadingRunner);
                Destroy(gameObject);
            }          

            //Sucessful Pass
            //if (passing && (_collision.gameObject.name == PlayerRole.HalfBack || _collision.gameObject.name == PlayerRole.TightEnd))
            //{
            //    flying = false;
            //    passing = false;
            //    ballRB.constraints = RigidbodyConstraints.None;
            //    carryingTheBall = _collision.gameObject;
            //    //gameObject.SetActive(false);
            //    _collision.gameObject.GetComponent<SetUpPlayers>().SetUpReceiver();
            //    GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Runner);
            //}

            //Intercept
            if (_collision.gameObject.tag == PlayerRole.EnemyTeam)
            {
                float collisionForce = _collision.impulse.magnitude / Time.fixedDeltaTime;
                Debug.Log("enemy colision force:   " + collisionForce);
                ballRB.constraints = RigidbodyConstraints.None;
                ballRB.useGravity = true;
                passing = false;

                if (collisionForce <= forceToIntercept)
                {
                    GeneralGameHandler.downStarted = false;
                    intercept_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                    Invoke("CallIntercept", 2f);
                }
                else
                {
                    handler.EndingDown(handler.startingDownYards);
                }
            }
        }
    }

    void Start()
    {
        //ballRB.constraints = RigidbodyConstraints.FreezePositionY;
        //ballRB = GetComponent<Rigidbody>();
        qb = GameObject.Find(PlayerRole.Quarterback);
        handler = FindObjectOfType<GeneralGameHandler>();

        if (outOfField_Indicator == null)
            outOfField_Indicator = GameObject.Find("OutOfField_Indicator");

        if (missPass_Indicator == null)
            missPass_Indicator = GameObject.Find("MissPass_Indicator");

        if (intercept_Indicator == null)
            intercept_Indicator = GameObject.Find("Intercept_Indicator");

        if (!qb.GetComponent<QuarterbackController>().afterSnap)
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
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

    public void StartSnap()
    {
        //ballRB.constraints = RigidbodyConstraints.None;
        ballRB.constraints = RigidbodyConstraints.FreezePositionY;
        // gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        snaping = true;
        GetComponent<Collider>().enabled = true;
    }

    void CallIntercept()
    {
        intercept_Indicator.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.LogginEnemyDowns);
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
