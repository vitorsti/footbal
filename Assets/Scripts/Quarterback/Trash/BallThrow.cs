using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrow : MonoBehaviour
{
    //public GameObject carryingTheBall;
    //public float forceToIntercept = 3f;

    //[Header("----- Snap -----")]
    //[SerializeField] bool snaping = false;
    //[SerializeField] float snapSpeed = 4f;
    //[SerializeField] float timeUntilSnap = 2f;

    //[Header("----- Ball -----")]
    //public bool flying;
    //[SerializeField] bool passing;

    //GameObject target;
    //public GameObject qb;
    //float passForce;
    //[SerializeField] Rigidbody ballRB;
    //GeneralGameHandler handler;

    //public Vector3 throwPos { get; private set; }

    //private void OnTriggerEnter(Collider _trigger)
    //{
    //    if (_trigger.gameObject.tag == "OutOfField" && GeneralGameHandler.downStarted)
    //    {
    //        GeneralGameHandler.downStarted = false;
    //        handler.EndingDown(handler.startingDownYards);
    //    }
    //}

    //private void OnCollisionEnter(Collision _collision)
    //{
    //    //Debug.Log("ball colission " + _collision.gameObject.name);

    //    if (_collision.gameObject.name == PlayerRole.Quarterback)
    //    {
    //        snaping = false;
    //        carryingTheBall = _collision.gameObject;
    //        GetComponent<Collider>().enabled = false;
    //        GetComponent<MeshRenderer>().enabled = false;
    //    }

    //    if (flying)
    //    {
    //        //Miss Throw
    //        if (_collision.gameObject.name == "Football_Camp" && GeneralGameHandler.downStarted)
    //        {
    //            GeneralGameHandler.downStarted = false;
    //            flying = false;
    //            handler.EndingDown(handler.startingDownYards);
    //        }

    //        //Sucessful Throw
    //        if (!passing && (_collision.gameObject.name == PlayerRole.WideReceiver || _collision.gameObject.name == PlayerRole.HalfBack || _collision.gameObject.name == PlayerRole.TightEnd))
    //        {
    //            flying = false;
    //            carryingTheBall = _collision.gameObject;
    //            ballRB.useGravity = true;
    //            gameObject.SetActive(false);
    //            _collision.gameObject.GetComponent<SetUpPlayers>().SetUpReceiver();
    //            GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Runner);
    //        }

    //        if (passing && _collision.gameObject.tag != PlayerRole.EnemyTeam)
    //        {

    //        }

    //        //Sucessful Pass
    //        if (passing && (_collision.gameObject.name == PlayerRole.HalfBack || _collision.gameObject.name == PlayerRole.TightEnd))
    //        {
    //            flying = false;
    //            passing = false;
    //            ballRB.constraints = RigidbodyConstraints.None;
    //            carryingTheBall = _collision.gameObject;
    //            gameObject.SetActive(false);
    //            _collision.gameObject.GetComponent<SetUpPlayers>().SetUpReceiver();
    //            GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Runner);
    //        }

    //        //Intercept
    //        if (_collision.gameObject.tag == PlayerRole.EnemyTeam)
    //        {
    //            float collisionForce = _collision.impulse.magnitude / Time.fixedDeltaTime;
    //            Debug.Log("ball colision force:   " + collisionForce);
    //            passing = false;

    //            //if (collisionForce <= forceToIntercept)
    //            //{
    //            //    GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.LogginEnemyDowns);
    //            //}
    //            //else
    //            //{
    //            //    handler.EndingDown(handler.startingDownYards);
    //            //}
    //        }
    //    }
    //}

    //void Start()
    //{
    //    //ballRB = GetComponent<Rigidbody>();
    //    qb = GameObject.Find(PlayerRole.Quarterback);
    //    handler = FindObjectOfType<GeneralGameHandler>();
    //}

    //void Update()
    //{
    //    if (GeneralGameHandler.downStarted)
    //    {
    //        if (snaping)
    //            transform.position = Vector3.Slerp(transform.position, qb.transform.position, snapSpeed * Time.deltaTime);

    //        if (passing && flying)
    //        {
    //            Passing();
    //        }

    //        if (carryingTheBall != null)
    //        {
    //            //gameObject.SetActive(false);
    //        }

    //    }


    //    if (throwing)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, drawingPoints[index], throwSpeed);

    //        if (Vector3.Distance(transform.position, drawingPoints[index]) < 0.03f)
    //            index++;
    //    }
    //}

    //public void isPass(GameObject _target, float _force)
    //{
    //    ballRB.constraints = RigidbodyConstraints.FreezePositionY;
    //    GetComponent<Collider>().enabled = true;
    //    GetComponent<MeshRenderer>().enabled = true;
    //    carryingTheBall = null;
    //    flying = true;
    //    passing = true;
    //    target = _target;
    //    passForce = _force;
    //    transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    //}

    //void Passing()
    //{
    //    ballRB.useGravity = false;
    //    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 20f * Time.deltaTime);
    //}

    //public void LauchBall(List<Vector3> _points)
    //{
    //    points = _points;
    //    BezierInterpolate();
    //    throwing = true;

    //    GetComponent<Collider>().enabled = true;
    //    GetComponent<MeshRenderer>().enabled = true;

    //    //ballRB.AddForce(_mousepos.x, _ballForce, zPos, ForceMode.Impulse);
    //    ballRB.constraints = RigidbodyConstraints.None;

    //    carryingTheBall = null;
    //    flying = true;
    //    Debug.Log(_points.Count + "    Lauch:  " + _points[1].y) ;
    //}

    //public void StartThePlay()
    //{
    //    ballRB.constraints = RigidbodyConstraints.None;
    //    ballRB.constraints = RigidbodyConstraints.FreezePositionY;
    //    snaping = true;
    //    GetComponent<Collider>().enabled = true;
    //}

    //[Header("Test")]
    //public List<Vector3> points;
    //public List<Vector3> gizmos;
    //public List<Vector3> drawingPoints;
    //public float throwSpeed;
    //public bool throwing;
    //public int index;

    //private void BezierInterpolate()
    //{
    //    BezierPath bezierPath = new BezierPath();
    //    bezierPath.Interpolate(points, .25f);

    //    drawingPoints = bezierPath.GetDrawingPoints2();

    //    gizmos = bezierPath.GetControlPoints();

    //    //SetLinePoints(drawingPoints);
    //}
}
