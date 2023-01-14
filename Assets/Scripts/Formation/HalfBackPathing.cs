using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfBackPathing : MonoBehaviour
{
    [Header("----- General -----")]
    public bool downStarted;

    [Header("----- Speed Properties -----")]
    public float moveSpeed;
    [SerializeField] float normalSpeed = 2f;
    [SerializeField] float moveSpeedRotating = 1.5f;
    [SerializeField] float rotationSpeed = 5;
    [SerializeField] float speedToCath = 2f;

    [Header("----- Rotating -----")]
    [SerializeField] float angleDefault = 0.1f;
    [SerializeField] float angle90d = 0.2f;
    [SerializeField] float angle135d = 0.4f;
    [SerializeField] float angle180d = 0.6f;

    [Header("----- Jump/Get -----")]
    [SerializeField] float jumpDistance = 1f;
    [SerializeField] float jumpSpeed = 1.7f;

    PlayerPathingInfo pathing;
    public Transform[] keyPositions;
    int targetPoint;
    Vector3 targetPos;
    LastPosPathing lastPos;
    GameBallController ball;

    public enum LastPosPathing
    {
        FrontAligned,
        BehindAligned,
        Aside,
        FrontDiagonal,
        BehindDiagonal
    }

    private AnimationTestScript _animControll;
    private bool startAtack;

    private void Start()
    {
        _animControll = GetComponent<AnimationTestScript>();
        startAtack = true;
    }

    public void SetUpPathing()
    {
        pathing = GetComponent<PlayerPathingInfo>();
        //gameObject.GetComponent<SphereCollider>().enabled = true;
        keyPositions = pathing.GetPathing();
        //downStarted = true;
        targetPoint = 1;
        pathing.enabled = false;

        if (keyPositions.Length > 1)
        {
            targetPos = keyPositions[targetPoint].position;
            transform.rotation = Quaternion.LookRotation((keyPositions[targetPoint].position - transform.position).normalized);
        }
        else
        {
            targetPos = new Vector3(transform.position.x + Random.Range(-4f, 4f), transform.position.y, transform.position.z + 5f);
        }

        moveSpeed = normalSpeed;
        lastPos = LastPosPathing.BehindAligned;
    }

    private void Update()
    {
        if (GeneralGameHandler.downStarted)
        {
            //MovingAcrossPathing();

            //First Initialize in game...
            if (startAtack)
            {
                _animControll.StartWRAnim();
                startAtack = false;
            }


            if (ball == null)
                ball = GameObject.FindObjectOfType<GameBallController>();

            if (ball != null && ball.flying)
            {
                moveSpeed = speedToCath;

                if (Vector3.Distance(transform.position, ball.transform.position) < jumpDistance)
                {
                    transform.position = Vector3.Lerp(transform.position, ball.transform.position, jumpSpeed * Time.deltaTime);
                    return;
                }
                //else if (Vector3.Distance(transform.position, ball.throwPos) < 6f)
                //{
                //    targetPos = ball.throwPos;
                //    transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                //    return;
                //}
                else
                {
                    MovingAcrossPathing();
                }
            }
            else
            {
                MovingAcrossPathing();
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetUpPathing();
            downStarted = true;
        }
#endif
    }

    void MovingAcrossPathing()
    {
        //_animControll.PlayRunAnim();
        transform.position += transform.forward * Time.deltaTime * moveSpeed;

        if(gameObject.transform.position.z < GeneralGameHandler.actualYards)
        {
            moveSpeed = normalSpeed * 1.5f;
        }
        else
        {
            moveSpeed = normalSpeed;
        }

        if (Vector3.Distance(transform.position, targetPos) < 1f)
        {
            targetPoint++;

            if (targetPoint < keyPositions.Length)
            {
                targetPos = keyPositions[targetPoint].position;
                RotatingPenality();
            }
            else
            {
                //Parar ou continuar
                targetPos = new Vector3(transform.position.x + Random.Range(-2, 2f), transform.position.y, transform.position.z + 10f);
                RotatingPenality();
            }
        }

        Quaternion _lookRotation = Quaternion.LookRotation((targetPos - transform.position).normalized);
        transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);

        _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
        _animControll.BoolAllyHoldFalseAnim();
        _animControll.BoolRunTrueAnim();

    }

    void RotatingPenality()
    {
        _animControll.PlayRunAnim();

        float distanceX = transform.position.x - targetPos.x;
        float distanceZ = transform.position.z - targetPos.z;
        LastPosPathing current;

        if (targetPoint != 1)
        {
            if (distanceZ > -2f && distanceZ < 2f)
                current = LastPosPathing.Aside;

            //Out of Range means in Front (> 0) or Behind Receiver (< 0)
            else
            {
                if (distanceZ >= 0f)
                {
                    if (distanceX > -2f && distanceX < 2f)
                        current = LastPosPathing.FrontAligned;
                    else
                        current = LastPosPathing.FrontDiagonal;
                }
                else
                {
                    if (distanceX > -2f && distanceX < 2f)
                        current = LastPosPathing.BehindAligned;
                    else
                        current = LastPosPathing.BehindDiagonal;
                }
            }

            //Reto
            if (lastPos == current)
                SetNormalSpeed();
            else
            {
                moveSpeed = 1f;

                //90 graus
                if ((current == LastPosPathing.Aside && (lastPos == LastPosPathing.FrontAligned || lastPos == LastPosPathing.BehindAligned)) ||
                   ((current == LastPosPathing.FrontAligned || current == LastPosPathing.BehindAligned) && lastPos == LastPosPathing.Aside))
                {
                    Invoke("SetNormalSpeed", angle90d);
                }

                //180 graus
                else if ((current == LastPosPathing.FrontAligned && lastPos == LastPosPathing.BehindAligned) ||
                        (current == LastPosPathing.BehindAligned && lastPos == LastPosPathing.FrontAligned))
                {
                    Invoke("SetNormalSpeed", angle180d);
                }

                //135 graus
                else if ((lastPos == LastPosPathing.BehindAligned || lastPos == LastPosPathing.Aside || lastPos == LastPosPathing.FrontAligned) &&
                    (current == LastPosPathing.FrontDiagonal || current == LastPosPathing.BehindDiagonal))
                {
                    Invoke("SetNormalSpeed", angle135d);
                }

                else
                {
                    Invoke("SetNormalSpeed", angleDefault);
                }
            }

            lastPos = current;
        }
    }

    void SetNormalSpeed()
    {
        moveSpeed = normalSpeed;
    }
}
