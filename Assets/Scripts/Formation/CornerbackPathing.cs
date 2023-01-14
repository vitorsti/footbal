using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerbackPathing : MonoBehaviour
{
    public GameObject target;
    public GameObject ball;

    [Header("----- General -----")]
    public bool holding;
    public bool predicting;
    public bool guarding;

    [Header("----- Speed Properties -----")]
    public float moveSpeed;
    [SerializeField] float speedGuarding = 1f;
    [SerializeField] float speedChasingBall = 2f;
    [SerializeField] float speedChasingPlayer = 1.8f;
    [SerializeField] float speedPredictWR = 0.3f;
    [SerializeField] float speedPredictHB = 0.2f;

    [Header("----- Pos -----")]
    [SerializeField] Vector3 targetPos;

    BoxCollider ward;

    private AnimationTestScript _animControll;
    private bool startAtack;

    private void Start()
    {
        _animControll = GetComponent<AnimationTestScript>();
        startAtack = true;
    }
    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.transform.tag == PlayerRole.Teammate)
        {
            target = _collision.gameObject;
            //_animControll.PlayIdleAnim();

            _animControll.BoolEnemyHoldTrueAnim();
            _animControll.BoolRunFalseAnim();
            //_animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1, target.transform.position.z));
        }
    }

    private void OnTriggerEnter(Collider _trigger)
    {
        if(target == null)
        {
            if(_trigger.gameObject.name == PlayerRole.WideReceiver)
            {
                target = _trigger.gameObject;
                ChooseDefensePos();
            }

            if (_trigger.gameObject.name == PlayerRole.HalfBack)
            {
                target = _trigger.gameObject;
                ChooseDefensePos();
            }
        }
        else
        {
            //target = _trigger.gameObject;
            //ChooseDefensePos();
        }

    }

    public void SetUpPathing()
    {
        ward = GetComponent<BoxCollider>();
        ball = GameObject.FindGameObjectWithTag("Ball");
        moveSpeed = speedGuarding;
        ChooseDefensePos();

    }

    void Update()
    {
        if (GeneralGameHandler.downStarted)
        {

            //First Initialize in game...
            if (startAtack)
            {
                _animControll.StartWRAnim();
                startAtack = false;
            }

            if (ball == null)
                ball = GameObject.Find("Ball");
                //ball = GameObject.FindObjectOfType<GameBallController>().gameObject;

            if (ball != null && ball.GetComponent<GameBallController>().flying == true)
            {
                //_animControll.PlayRunAnim();
                moveSpeed = speedChasingBall;
                targetPos = new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z + 7f);

                _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
                _animControll.BoolEnemyHoldlFalseAnim();
                _animControll.BoolRunTrueAnim();

            }
            else if(predicting)
            {
                if (Vector3.Distance(transform.position, targetPos) < 1f && predicting)
                {
                    targetPos = target.transform.position;
                }
                else if (target.transform.position.z > transform.position.z)
                    targetPos = target.transform.position;

                //_animControll.PlayRunAnim();
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
                _animControll.BoolEnemyHoldlFalseAnim();
                _animControll.BoolRunTrueAnim();

            }
            else if (guarding)
            {
                //_animControll.PlayIdleAnim();
                moveSpeed = speedGuarding;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
                _animControll.BoolEnemyHoldlFalseAnim();
                _animControll.BoolRunTrueAnim();

            }

        }
    }

    void Guarding()
    {
        //_animControll.PlayIdleAnim();
        GameObject center = GameObject.Find("C");
        moveSpeed = speedGuarding;
        guarding = true;

        if (transform.position.x > center.transform.position.x)
            targetPos = new Vector3(transform.position.x - Random.Range(4, 10), transform.position.y, transform.position.z - 3f);
        else
            targetPos = new Vector3(transform.position.x + Random.Range(4, 10), transform.position.y, transform.position.z - 3f);
    }

    void ChooseDefensePos()
    {
        if (target != null)
        {
            predicting = true;
            guarding = false;

            if (target.name == PlayerRole.WideReceiver && target.GetComponent<WideReceiverPathing>().keyPositions.Length > 1)
            {
                targetPos = target.GetComponent<WideReceiverPathing>().keyPositions[1].position;
                moveSpeed = target.GetComponent<WideReceiverPathing>().moveSpeed - speedPredictWR;
            }

            else if(target.name == PlayerRole.HalfBack)
            {
                int rng = Random.Range(0, target.GetComponent<HalfBackPathing>().keyPositions.Length);

                moveSpeed = target.GetComponent<HalfBackPathing>().moveSpeed - speedPredictHB;

                switch (rng)
                {
                    case 1:
                        targetPos = target.GetComponent<HalfBackPathing>().keyPositions[1].position;
                        break;
                    case 2:
                        targetPos = target.GetComponent<HalfBackPathing>().keyPositions[2].position; 
                        break;
                    case 3:
                        targetPos = target.GetComponent<HalfBackPathing>().keyPositions[3].position;
                        break;
                }
            }
            else
            {
                targetPos = target.transform.position;
            }
        }
        else
        {
            //targetPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y, transform.position.z - 2f);
            //moveSpeed = speedGuarding;
            Guarding();
        }
    }
}
