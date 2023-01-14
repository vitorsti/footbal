using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffensiveGuard : MonoBehaviour
{
    [Header("----- General -----")]
    public GameObject target;
    [SerializeField] bool holding;
    [SerializeField] bool canHold;

    [Header("----- Speed Properties -----")]
    public float moveSpeed;
    [SerializeField] float moveSpeedChasing = 0.8f;
    [SerializeField] float moveSpeedGuarding = 0.4f;
    [SerializeField] float moveSpeedHolding = 0.1f;

    [Header("----- Hold Properties -----")]
    public float holdDuration = 2.5f;

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
        if (_collision.transform.tag == PlayerRole.EnemyTeam && _collision.gameObject.name == PlayerRole.DefensiveTackle && target == null)
        {
            target = _collision.gameObject;
            //_animControll.PlayIdleAnim();
        }

        if(target != null && _collision.gameObject == target && !holding && canHold)
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
        if (_trigger.transform.tag == PlayerRole.EnemyTeam && _trigger.gameObject.name == PlayerRole.DefensiveTackle && target == null)
        {
            target = _trigger.gameObject;
        }
    }

    public void SetUpPathing()
    {
        rb = GetComponent<Rigidbody>();
        qb = GameObject.Find("QB");
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
        //_animControll.PlayIdleAnim();

        _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1, target.transform.position.z));
        _animControll.BoolAllyHoldFalseAnim();
        _animControll.BoolRunTrueAnim();

        moveSpeed = moveSpeedGuarding;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), moveSpeed * Time.deltaTime);
    
    
    }

    void Chasing()
    {
        //_animControll.PlayRunAnim();

        _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1, target.transform.position.z));
        _animControll.BoolAllyHoldFalseAnim();
        _animControll.BoolRunTrueAnim();

        moveSpeed = moveSpeedChasing;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    void Holding()
    {
        //_animControll.PlayIdleAnim();

        _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1, target.transform.position.z));
        _animControll.BoolAllyHoldTrueAnim();
        _animControll.BoolRunFalseAnim();

        moveSpeed = moveSpeedHolding;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f), moveSpeed * Time.deltaTime);
    }

    void Release()
    {
        //_animControll.PlayRunAnim();

        _animControll.BoolAllyHoldFalseAnim();
        _animControll.BoolRunTrueAnim();

        if (target != null)
        {
            canHold = false;
            target.transform.parent = null;
            holding = false;
            target.GetComponent<DefensiveLinePathing>().holded = false;
            rb.isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        if (GeneralGameHandler.downStarted)
            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 4f)
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;// gameObject.GetComponent<Rigidbody>().velocity.normalized * 4;
        //                Debug.Log(gameObject.name + "   " + gameObject.GetComponent<Rigidbody>().velocity.magnitude);

    }
}
