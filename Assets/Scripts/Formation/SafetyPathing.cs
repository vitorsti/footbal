using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyPathing : MonoBehaviour
{
    public GameObject target;
    public GameObject ball;

    [Header("----- General -----")]
    public bool downStarted;
    public bool holding;
    public bool nearBall;

    [Header("----- Speed Properties -----")]
    public float moveSpeed;
    [SerializeField] float speedGuarding = 0.4f;
    [SerializeField] float speedChasingBall = 3f;
    [SerializeField] float speedChasingPlayer = 1.7f;

    Vector3 targetPos;

    private AnimationTestScript _animControll;
    private bool startAtack;

    private void Start()
    {
        _animControll = GetComponent<AnimationTestScript>();
        startAtack = true;
    }

    private void OnTriggerEnter(Collider _trigger)
    {
        if (_trigger.gameObject.tag == PlayerRole.Teammate)
        {
            if (target != null && _trigger.transform.position.z > target.transform.position.z)
                target = _trigger.gameObject;
            else if (target == null)
                target = _trigger.gameObject;
        }
        else if(_trigger.gameObject.tag == "Ball")
        {
            nearBall = true;
        }
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

    public void SetUpPathing()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        targetPos = transform.position;
        Invoke("WalkingBack", 2f);
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

            if (nearBall) //ball.GetComponent<GameBallController>().flying == true && 
            {
                moveSpeed = speedChasingBall;
                //_animControll.PlayRunAnim();
                targetPos = new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z + 2f);
            }
            else if (target != null)
            {
                moveSpeed = speedChasingPlayer;
                //_animControll.PlayRunAnim();
                targetPos = new Vector3(target.transform.position.x / 2, target.transform.position.y, transform.position.z); // target.transform.position.z + 4f);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
            _animControll.BoolEnemyHoldlFalseAnim();
            _animControll.BoolRunTrueAnim();

        }
    }

    void Guarding()
    {
        moveSpeed = speedGuarding;
        //_animControll.PlayIdleAnim();
        targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3f);
    }
}
