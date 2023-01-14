using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //public GameObject carryingTheBall;

    //[Header("----- Snap -----")]
    //[SerializeField] bool snaping;
    //[SerializeField] float snapSpeed = 4f;
    //[SerializeField] float timeUntilSnap = 2f;

    //[Header("----- Ball -----")]
    //public bool flying;
    //[SerializeField] bool passing;

    //GameObject target;
    //float passForce;
    //[SerializeField] Rigidbody ballRB;

    //public Vector3 throwPos { get; private set; }

    //private void OnCollisionEnter(Collision _collision)
    //{
    //    if(_collision.gameObject.name == PlayerRole.Quarterback)
    //    {
    //        snaping = false;
    //        carryingTheBall = _collision.gameObject;
    //        GetComponent<Collider>().enabled = false;
    //        GetComponent<MeshRenderer>().enabled = false;
    //    }

    //    if(flying)
    //    {
    //        //Miss Throw
    //        if (_collision.gameObject.name == "Football_Camp")
    //        {
    //            flying = false;
                
    //        }

    //        //Sucessful Throw
    //        if (_collision.gameObject.name == PlayerRole.WideReceiver || _collision.gameObject.name == PlayerRole.HalfBack || _collision.gameObject.name == PlayerRole.TightEnd)
    //        {
    //            flying = false;
    //            carryingTheBall = _collision.gameObject;
    //            ballRB.useGravity = true;
    //            gameObject.SetActive(false);
    //            _collision.gameObject.name = PlayerRole.Receiver;
    //            GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Runner);
    //        }

    //        //Sucessful Pass
    //        if (_collision.gameObject.name == PlayerRole.HalfBack || _collision.gameObject.name == PlayerRole.TightEnd)
    //        {
    //            flying = false;
    //            passing = false;
    //            ballRB.constraints = RigidbodyConstraints.None;
    //            carryingTheBall = _collision.gameObject;
    //            gameObject.SetActive(false);
    //            _collision.gameObject.name = PlayerRole.Receiver;
    //            GameStateCaller.CallNextState(GameStateManager.StateOfTheGame.Runner);
    //        }

    //        //Intercept
    //        if (_collision.gameObject.tag == PlayerRole.EnemyTeam)
    //        {
                
    //        }
    //    }
    //}
    //private void Awake()
    //{
    //    ballRB = gameObject.GetComponent<Rigidbody>();
    //}

    //void Start()
    //{
    //    ballRB = GetComponent<Rigidbody>();
    //    //Invoke("StartThePlay", timeUntilSnap);
    //}

    //void Update()
    //{
    //    if (GeneralGameHandler.downStarted)
    //    {
    //        if (snaping)
    //            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), snapSpeed * Time.deltaTime);

    //        if (passing && flying)
    //        {
    //            Passing();
    //        }

    //        if (carryingTheBall != null)
    //        {
    //            //gameObject.SetActive(false);
    //        }
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

    //public void LauchBall(Vector3 _mousepos, float _ballForce)
    //{
    //    throwPos = new Vector3(_mousepos.x, 1, _mousepos.z);
    //    transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
    //    GetComponent<Collider>().enabled = true;
    //    GetComponent<MeshRenderer>().enabled = true;
    //    ballRB.AddForce(_mousepos.x, _ballForce, _mousepos.z, ForceMode.Impulse);
    //    ballRB.constraints = RigidbodyConstraints.None;
    //    carryingTheBall = null;
    //    flying = true;
    //    Debug.Log(_mousepos + "    Lauch:  " + _ballForce);
    //}
    
    //public void StartingSnap()
    //{
    //    ballRB.constraints = RigidbodyConstraints.None;
    //    ballRB.constraints = RigidbodyConstraints.FreezePositionY;
    //    snaping = true;
    //    GetComponent<Collider>().enabled = true;
    //}
}
