using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveLinePathing : MonoBehaviour
{
    public GameObject target;
    //public GameObject holding;

    [Header("----- General -----")]
    public bool downStarted;
    public bool holded;

    [Header("----- Speed Properties -----")]
    public float moveSpeed;
    [SerializeField] float speedPathing = 1f;
    [SerializeField] float speedChasing = 3f;
    [SerializeField] float speedHolded = 0f;
    [SerializeField] float speedTackle = 1.5f;

    [Header("----- Tackle Properties -----")]
    [SerializeField] float tackleDistance = 1.5f;
    [SerializeField] float tackleSpeed = 2f;

    public GameObject qb;
    Vector3 targetPos;
    Rigidbody rb;

    public DLState state;

    private AnimationTestScript _animControll;
    private bool startAtack;

    private void Start()
    {
        _animControll = GetComponent<AnimationTestScript>();
        startAtack = true;
    }

    public void SetUpPathing()
    {
        rb = GetComponent<Rigidbody>();
        qb = GameObject.Find(PlayerRole.Quarterback);
        state = DLState.Pathing;

        switch (gameObject.name)
        {
            case PlayerRole.DefensiveTackle:
                DefensiveTackleStart();
                break;

            case PlayerRole.DefensiveEnd:
                DefensiveEndStart();
                break;
        }
    }

    public enum DLState
    {
        Pathing,
        Chasing,
        Holded,
        Tackle
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


            if (holded)
                Holded();
            else
            {
                switch (state)
                {
                    case DLState.Pathing:
                        Pathing();
                        break;

                    case DLState.Chasing:
                        Chasing();
                        break;

                    case DLState.Holded:
                        Holded();
                        break;

                    case DLState.Tackle:
                        Tackle();
                        break;
                }
            }
        }
    }

    void Pathing()
    {
        //_animControll.PlayRunAnim();
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
        _animControll.BoolEnemyHoldlFalseAnim();
        _animControll.BoolRunTrueAnim();

        if (Vector3.Distance(transform.position, targetPos) < 0.2f)
            state = DLState.Chasing;
    }

    void Chasing()
    {
        //_animControll.PlayRunAnim();
        moveSpeed = speedChasing;
        transform.position = Vector3.MoveTowards(transform.position, qb.transform.position, moveSpeed * Time.deltaTime);

        _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
        _animControll.BoolEnemyHoldlFalseAnim();
        _animControll.BoolRunTrueAnim();

        if (!holded && Vector3.Distance(transform.position, qb.transform.position) < tackleDistance)
        {
            Tackle();
        }
    }

    void Holded()
    {
        //_animControll.PlayIdleAnim();
        moveSpeed = speedHolded;
        state = DLState.Holded;

        _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
        _animControll.BoolEnemyHoldTrueAnim();
        _animControll.BoolRunFalseAnim();

        if (!holded)
            state = DLState.Chasing;
    }

    void Tackle()
    {
        //_animControll.PlayJumpAnim();
        state = DLState.Tackle;
        moveSpeed = speedTackle;

        _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
        _animControll.TryCatchReciverAnim();
        _animControll.BoolRunFalseAnim();

        if (!holded)
            transform.position = Vector3.Slerp(transform.position, qb.transform.position, Time.deltaTime * tackleSpeed);
    }

    void DefensiveEndStart()
    {
        moveSpeed = speedPathing;

        if (transform.position.x < qb.transform.position.x)
            targetPos = new Vector3(transform.position.x - Random.Range(2f, 4f), transform.position.y, transform.position.z - Random.Range(1.5f, 3f));
        else
            targetPos = new Vector3(transform.position.x + Random.Range(2f, 4f), transform.position.y, transform.position.z - Random.Range(1.5f, 3f));
    }

    void DefensiveTackleStart()
    {
        moveSpeed = speedPathing;

        if (transform.position.x < qb.transform.position.x)
            targetPos = new Vector3(transform.position.x - Random.Range(0, 1f), transform.position.y, transform.position.z - 2f);
        else
            targetPos = new Vector3(transform.position.x + Random.Range(0, 1f), transform.position.y, transform.position.z - 2f);
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

}