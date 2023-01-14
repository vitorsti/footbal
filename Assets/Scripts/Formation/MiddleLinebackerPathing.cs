using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLinebackerPathing : MonoBehaviour
{
    public GameObject target;
    public GameObject ball;

    [Header("----- General -----")]
    public bool holding;

    [Header("----- Speed Properties -----")]
    public float moveSpeed;
    [SerializeField] float speedGuarding = 0.3f;
    [SerializeField] float speedChasingBall = 3f;
    [SerializeField] float speedChasingPlayer = 2f;

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
            target == null &&
           (_trigger.gameObject.name != PlayerRole.Center &&
            _trigger.gameObject.name != PlayerRole.OffensiveGuard &&
            _trigger.gameObject.name != PlayerRole.OffensiveTackle))
        {
            CheckOutsideLinebacker(_trigger.gameObject);
        }

    }

    public void SetUpPathing()
    {
        GuardingArea();
        Invoke("ChooseStrategy", 2f);
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

            if (target != null)
            {
                moveSpeed = speedChasingPlayer;
                targetPos = target.transform.position;
            }
            else
            {
                moveSpeed = speedGuarding;
                if (Vector3.Distance(transform.position, targetPos) < 0.2f)
                    GuardingArea();
            }

            //_animControll.PlayRunAnim();

            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            _animControll._MeshTrans.LookAt(new Vector3(targetPos.x, _animControll.transform.position.y - 1, targetPos.z));
            _animControll.BoolEnemyHoldlFalseAnim();
            _animControll.BoolRunTrueAnim();

        }
    }

    void ChooseStrategy()
    {

    }

    void CheckOutsideLinebacker(GameObject _target)
    {
        OutsideLinebackerPathing[] OL = FindObjectsOfType<OutsideLinebackerPathing>();
        if (OL.Length > 0)
        {
            foreach (OutsideLinebackerPathing script in OL)
            {
                if (script.target != null && script.target != _target)
                {
                    target = _target;
                }
            }
        }
        else
        {
            target = _target;
        }
    }

    void GuardingArea()
    {
        targetPos = new Vector3(transform.position.x - Random.Range(-2f, 2f), transform.position.y, transform.position.z - Random.Range(-2f, 2f));
    }
}
