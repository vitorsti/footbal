using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideLinebackerPathing : MonoBehaviour
{
    public GameObject target;
    public GameObject ball;

    [Header("----- General -----")]
    public bool downStarted;
    public bool holded;
    public float chanceToAttack = 20;

    [Header("----- Speed Properties -----")]
    public float moveSpeed;
    [SerializeField] float speedGuarding = 0.7f;
    [SerializeField] float speedChasingBall = 3f;
    [SerializeField] float speedChasingPlayer = 1.8f;
    [SerializeField] float speedChasingQB = 2.5f;

    [Header("----- Tackle Properties -----")]
    [SerializeField] float tackleDistance;
    [SerializeField] float tackleSpeed;

    Vector3 targetPos;
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
        if (_trigger.gameObject.tag == PlayerRole.Teammate && 
           (_trigger.gameObject.name != PlayerRole.Center &&
            _trigger.gameObject.name != PlayerRole.OffensiveGuard &&
            _trigger.gameObject.name != PlayerRole.OffensiveTackle) &&
            target == null)
        {
            target = _trigger.gameObject;
        }

    }

    public void SetUpPathing()
    {
        ward = GetComponent<BoxCollider>();
        AdjustWardCenter();
        targetPos = transform.position;
        ChooseStrategy();
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

            if (!holded)
            {
                if (target != null)
                {
                    if (target.name == PlayerRole.Quarterback)
                    {
                        moveSpeed = speedChasingQB;

                        if (Vector3.Distance(transform.position, targetPos) < 0.2f)
                            targetPos = target.transform.position;

                        if (Vector3.Distance(transform.position, target.transform.position) < tackleDistance)
                            Tackle();
                    }
                    else
                    {
                        moveSpeed = speedChasingPlayer;
                        targetPos = target.transform.position;
                    }
                }
                else
                {
                    moveSpeed = speedGuarding;
                    //if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                    //    GuardingArea();
                }

                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            }

            _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
            _animControll.BoolEnemyHoldlFalseAnim();
            _animControll.BoolRunTrueAnim();

        }
        else
        {
            moveSpeed = 0.1f;

            _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
            _animControll.BoolEnemyHoldTrueAnim();
            _animControll.BoolRunFalseAnim();

        }
        //_animControll.PlayRunAnim();
    }

    void Tackle()
    {
        _animControll.TryCatchReciverAnim();

        transform.position = Vector3.Lerp(transform.position, target.transform.position, tackleSpeed * Time.deltaTime);

        //_animControll.PlayJumpAnim();

        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    //Attack QB or Protect Area
    void ChooseStrategy()
    {
        int rng = Random.Range(1, 101);

        if (rng <= 101) //chanceToAttack
        {
            target = GameObject.Find(PlayerRole.Quarterback);

            if (transform.position.x < target.transform.position.x)
                targetPos = new Vector3(transform.position.x + Random.Range(0f, 1f), transform.position.y, transform.position.z - 4f);
            else
                targetPos = new Vector3(transform.position.x - Random.Range(0f, 1f), transform.position.y, transform.position.z - 4f);
        }
        else
        {
            targetPos = new Vector3(transform.position.x - Random.Range(-2f, 2f), transform.position.y, transform.position.z - Random.Range(-1.5f, 1.5f));
        }
    }

    void AdjustWardCenter()
    {
        GameObject center = GameObject.Find("C");

        if (transform.position.x > center.transform.position.x)
            ward.center = new Vector3(ward.center.x + 2f, ward.center.y, ward.center.z);
        else
            ward.center = new Vector3(ward.center.x - 2f, ward.center.y, ward.center.z);
    }
}
