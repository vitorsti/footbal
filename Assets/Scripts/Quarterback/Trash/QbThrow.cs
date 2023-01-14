using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QbThrow : MonoBehaviour
{
    //[SerializeField] GameObject ball;

    //[Header("----- General -----")]
    //[SerializeField] bool hasBall;
    //bool willThrow;

    //[Header("----- Speed Properties -----")]
    //public float qbSpeed;
    //[SerializeField] float normalSpeed;
    //[SerializeField] float qbWithBallSpeed;

    //[Header("----- Force Properties -----")]
    //public float ballForce;
    //[SerializeField] float ballForceModifier;
    //[SerializeField] float passForce;

    ////Vars
    //Vector3 mousepos;
    //GeneralGameHandler handler;

    //private void OnCollisionEnter(Collision _collision)
    //{
    //    if (_collision.gameObject.tag == "Ball")
    //    {
    //        ball = _collision.gameObject;
    //        ball.transform.parent = transform;
    //        //ball.GetComponent<Rigidbody>().isKinematic = true;
    //        //ball.SetActive(false);
    //        hasBall = true;
    //    }

    //    if (_collision.gameObject.tag == PlayerRole.EnemyTeam)
    //    {
    //        Debug.Log("BLOCKED");
    //    }
    //}

    //void Start()
    //{
    //    handler = FindObjectOfType<GeneralGameHandler>();
    //    points = new List<Vector3>();

    //}

    //public void SetUpPlayer()
    //{

    //}

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0) && hasBall)
    //    {
    //        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        //RaycastHit hitData;

    //        ////Pass
    //        //if (Physics.Raycast(ray, out hitData, 100) &&
    //        //   (hitData.transform.position.z < (handler.startingDownYards + 3f)) &&
    //        //   (hitData.transform.name == PlayerRole.HalfBack ||
    //        //    hitData.transform.name == PlayerRole.TightEnd))
    //        //{
    //        //    hasBall = false;
    //        //    PassBall(hitData.transform.gameObject);
    //        //}
    //    }

    //    if (Input.GetMouseButton(0) && hasBall)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hitData;

    //        //Throw Ball
    //        if (Physics.Raycast(ray, out hitData, 300, 9) && (hitData.transform.name == "Football_Camp" || hitData.transform.name == PlayerRole.WideReceiver))
    //        {
    //            willThrow = true;
    //            mousepos = hitData.point;
    //            ballForce += ballForceModifier;
    //        }
    //    }

    //    if (Input.GetMouseButtonUp(0) && hasBall && willThrow)
    //    {
    //        ThrowBall();
    //    }

    //    if (hasBall)
    //    {
    //        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (qbWithBallSpeed * Time.deltaTime));
    //    }
    //}

    //void PassBall(GameObject _teammate)
    //{
    //    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    //    gameObject.transform.GetComponent<Collider>().isTrigger = true;
    //    ball.SetActive(true);
    //    ball.GetComponent<GameBallController>().isPass(_teammate, passForce);
    //    hasBall = false;
    //}

    //void ThrowBall()
    //{
    //    points.Add(transform.position);
    //    Vector3 forcePoint = new Vector3((mousepos.x - transform.position.x) / 2, ballForce, (mousepos.z - transform.position.z) / 2);
    //    points.Add(forcePoint);
    //    Vector3 finalPos = new Vector3(mousepos.x, 0f, mousepos.z);
    //    points.Add(finalPos);
    //    ball.GetComponent<BallThrow>().LauchBall(points);

    //    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    //    gameObject.transform.GetComponent<Collider>().isTrigger = true;
    //    ball.SetActive(true);
    //    //ball.GetComponent<GameBallController>().LauchBall(mousepos, ballForce);
    //    hasBall = false;
    //}


    ////void Update()
    ////{
    ////    if (Input.GetMouseButtonDown(0))
    ////    {
    ////        Vector2 screenPosition = Input.mousePosition;
    ////        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 4));

    ////        points.Add(worldPosition);
    ////    }
    ////}

    //public List<Vector3> points;
    //public List<Vector3> gizmos;

    //private void BezierInterpolate()
    //{
    //    BezierPath bezierPath = new BezierPath();
    //    bezierPath.Interpolate(points, .25f);

    //    List<Vector3> drawingPoints = bezierPath.GetDrawingPoints2();

    //    gizmos = bezierPath.GetControlPoints();

    //    SetLinePoints(drawingPoints);
    //}

    //private void SetLinePoints(List<Vector3> drawingPoints)
    //{
    //    //lineRenderer.SetVertexCount(drawingPoints.Count);

    //    //for (int i = 0; i < drawingPoints.Count; i++)
    //    //{
    //    //    lineRenderer.SetPosition(i, drawingPoints[i]);
    //    //}
    //}
}
