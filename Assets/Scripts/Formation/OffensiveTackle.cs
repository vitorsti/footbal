using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffensiveTackle : MonoBehaviour
{
    [Header("----- General -----")]
    public GameObject target;
    [SerializeField] bool holding;
    [SerializeField] bool canHold;

    [Header("----- Speed Properties -----")]
    public float moveSpeed;
    [SerializeField] float moveSpeedChasing = 0.8f;
    [SerializeField] float moveSpeedGuarding = 0.4f;
    [SerializeField] float moveSpeedHolding = 0.05f;

    [Header("----- Hold Properties -----")]
    public float holdDuration = 3f;

    Rigidbody rb;
    GameObject qb;

    private AnimationTestScript _animControll;
    private bool startAtack;

    private void Start()
    {
        _animControll = GetComponent<AnimationTestScript>();

        startAtack = true;
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.transform.tag == PlayerRole.EnemyTeam && _collision.gameObject.name == PlayerRole.DefensiveEnd && target == null)
        {
            target = _collision.gameObject;
            _animControll.PlayIdleAnim();
        }

        if (target != null && _collision.gameObject == target && !holding && canHold)
        {
            canHold = false;
            holding = true;
            target.GetComponent<DefensiveLinePathing>().holded = true;
            target.transform.parent = transform;

            float duration = Random.Range((holdDuration-1f), holdDuration);
            Invoke("Release", duration);
        }
    }

    private void OnTriggerEnter(Collider _trigger)
    {
        if (_trigger.transform.tag == PlayerRole.EnemyTeam && _trigger.gameObject.name == PlayerRole.DefensiveEnd && target == null)
        {
            target = _trigger.gameObject;
            _animControll.PlayIdleAnim();
        }
    }

    public void SetUpPathing()
    {
        Ward();
        rb = GetComponent<Rigidbody>();
        qb = GameObject.Find(PlayerRole.Quarterback);
        canHold = true;
        moveSpeed = moveSpeedGuarding;
    }

    // Update is called once per frame
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

            if (!holding)
            {
                if (target == null)
                {
                    Protecting();
                }
                else
                {
                    Chasing();
                }
            }

            else
            {
                Holding();
            }
        }
    }

    void Protecting()
    {
        moveSpeed = moveSpeedGuarding;
        //_animControll.PlayRunAnim();

        _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1, target.transform.position.z));
        _animControll.BoolAllyHoldFalseAnim();
        _animControll.BoolRunTrueAnim();

        if (transform.position.x < qb.transform.position.x)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z + 0.5f), moveSpeed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 0.5f), moveSpeed * Time.deltaTime);
    }

    void Chasing()
    {
        moveSpeed = moveSpeedChasing;
        //_animControll.PlayRunAnim();

        _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1, target.transform.position.z));
        _animControll.BoolAllyHoldFalseAnim();
        _animControll.BoolRunTrueAnim();

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    void Holding()
    {
        moveSpeed = moveSpeedHolding;
        //_animControll.PlayIdleAnim();

        _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1, target.transform.position.z));
        _animControll.BoolAllyHoldTrueAnim();
        _animControll.BoolRunFalseAnim();

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f), moveSpeed * Time.deltaTime);
    }

    void Release()
    {

        //_animControll.PlayRunAnim();

        _animControll.BoolAllyHoldFalseAnim();
        _animControll.BoolRunTrueAnim();

        if (target != null)
        {
            canHold = false;
            holding = false;

            if (target.transform.parent == transform)
            {
                target.GetComponent<DefensiveLinePathing>().holded = false;
                target.transform.parent = null;
            }
        }
    }

    void Ward()
    {
        if(qb == null)
            qb = GameObject.Find(PlayerRole.Quarterback);

        if (transform.position.x < qb.transform.position.x)
            gameObject.GetComponent<BoxCollider>().center = new Vector3(-1, 0, 0.5f);
        else
            gameObject.GetComponent<BoxCollider>().center = new Vector3(1, 0, 0.5f);
    }

    private void FixedUpdate()
    {
        if (GeneralGameHandler.downStarted)
            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 4f)
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;// gameObject.GetComponent<Rigidbody>().velocity.normalized * 4;
        //                Debug.Log(gameObject.name + "   " + gameObject.GetComponent<Rigidbody>().velocity.magnitude);

    }
}

