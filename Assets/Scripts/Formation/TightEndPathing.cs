using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TightEndPathing : MonoBehaviour
{
    [SerializeField] GameObject target;

    [Header("----- General -----")]
    [SerializeField] bool defending;
    [SerializeField] bool holding;
    [SerializeField] bool canHold;

    [Header("----- Speed Properties -----")]
    public float moveSpeed;
    [SerializeField] float speedRunning = 1f;
    [SerializeField] float speedGuarding = 0.5f;
    [SerializeField] float speedChasing = 1f;
    [SerializeField] float speedHolding = 0.05f;

    [Header("----- Hold -----")]
    [SerializeField] float holdDuration = 1.5f;

    PlayerPathingInfo pathing;
    Transform[] keyPositions = new Transform[] { };
    Vector3 targetPos;
    BoxCollider ward;
    Rigidbody rb;

    private AnimationTestScript _animControll;
    private bool startAtack; 

    private void Start()
    {
        _animControll = GetComponent<AnimationTestScript>();
        startAtack = true;
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (target != null && _collision.gameObject.name == PlayerRole.DefensiveEnd && !holding && canHold && defending)
        {
            canHold = false;
            holding = true;
            target.GetComponent<DefensiveLinePathing>().holded = true;
            target.transform.parent = this.transform;

            StartCoroutine("Release");
        }

        if (target != null && _collision.gameObject.name == PlayerRole.OutsideLineBacker && !holding && canHold && defending)
        {
            canHold = false;
            holding = true;
            target.GetComponent<OutsideLinebackerPathing>().holded = true;
            target.transform.parent = transform;

            StartCoroutine("Release");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == PlayerRole.DefensiveEnd && !holding && defending)
            target = other.gameObject;

        if (other.gameObject.name == PlayerRole.OutsideLineBacker && target != null)
        {
            //Release DE
            if (target.gameObject.name == PlayerRole.DefensiveEnd)
            {
                target.transform.parent = null;
                target.GetComponent<DefensiveLinePathing>().holded = false;
            }
            StopAllCoroutines();

            //Set OLB
            target = other.gameObject;
            canHold = true;
            holding = false;
        }
        else if(other.gameObject.name == PlayerRole.OutsideLineBacker)
        {
            target = other.gameObject;
            canHold = true;
            holding = false;
        }
    }

    public void SetUpPathing()
    {
        rb = GetComponent<Rigidbody>();
        pathing = GetComponent<PlayerPathingInfo>();
        gameObject.GetComponent<SphereCollider>().enabled = true;
        ward = GetComponent<BoxCollider>();
        keyPositions = pathing.GetPathing();
        //ward.enabled = false;
        //pathing.enabled = false;
        CheckStrategy();
    }

    // Update is called once per frame
    void Update()
    {


        if (GeneralGameHandler.downStarted)
        {

            //First Initialize in game...
            if (startAtack)
            {
                CheckStrategy();
                _animControll.StartWRAnim();
                startAtack = false;
            }

            _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));

            if (defending)
            {
                if (!holding)
                {
                    //Pathing
                    if (target == null)
                    {
                        moveSpeed = speedGuarding;
                        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                        _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1f, targetPos.z));
                        _animControll.BoolRunTrueAnim();
                    }
                    //Move Towards Enemy
                    else
                    {
                        moveSpeed = speedChasing;
                        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
                        _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1f, target.transform.position.z));
                        _animControll.BoolRunTrueAnim();
                    }

                }
                else
                {
                    moveSpeed = speedHolding;
                    _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1f, target.transform.position.z));
                    _animControll.BoolRunFalseAnim();
                    _animControll.BoolAllyHoldTrueAnim();
                }

            }
            //For Pass
            else
            {
                if (transform.position.x == targetPos.x && transform.position.z == targetPos.z)
                {
                    moveSpeed = 0;
                    //transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                    _animControll._MeshTrans.rotation = new Quaternion(0, 0, 0, 1);
                    targetPos = new Vector3(keyPositions[0].position.x, 0, keyPositions[0].position.z);
                    _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1f, targetPos.z));
                    _animControll.BoolRunTrueAnim();
                    _animControll.BoolAllyHoldFalseAnim();
                }
                else
                {
                    moveSpeed = speedRunning;
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                    _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1f, targetPos.z));
                    _animControll.BoolRunTrueAnim();
                    _animControll.BoolAllyHoldFalseAnim();
                }
            }
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetUpPathing();
        }
    #endif
    }

    void CheckStrategy()
    {
        //Player Pathing
        if (keyPositions.Length > 1)
        {
            //Will Defend
            if (transform.position.z > keyPositions[1].position.z)
            {
                defending = true;
                Invoke("ActivateWard", 2f);
                moveSpeed = speedGuarding;
            }
            //Will go for Pass
            else
            {
                moveSpeed = speedRunning;
            }

            targetPos = new Vector3(keyPositions[1].position.x, keyPositions[1].position.y, keyPositions[1].position.z);
        }
        //Random Pathing
        else
        {
            targetPos = new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y, transform.position.z + Random.Range(-2f, 2f));

            //Will Defend
            if (transform.position.z > targetPos.z)
            {
                defending = true;
                Invoke("ActivateWard", 2f);
                moveSpeed = speedGuarding;
            }
            //Will go for Pass
            else
            {
                moveSpeed = speedRunning;
            }
        }
    }

    IEnumerator Release()
    {

        ////_animControll.PlayIdleAnim();
        yield return new WaitForSeconds(holdDuration);

        if (target != null)
        {
            if (target.gameObject.name == PlayerRole.DefensiveEnd)
                target.GetComponent<DefensiveLinePathing>().holded = false;
            
            
            if (target.gameObject.name == PlayerRole.OutsideLineBacker)            
                target.GetComponent<OutsideLinebackerPathing>().holded = false;
            
            target.transform.parent = null;
            canHold = false;
            holding = false;
        }

        _animControll.BoolAllyHoldFalseAnim();
       // _animControll._MeshTrans.LookAt(new Vector3(targetPos.x,transform.position.y, targetPos.z));
    }

    void ActivateWard()
    {
        ward.enabled = true;
    }
}
