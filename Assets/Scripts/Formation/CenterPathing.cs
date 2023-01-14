using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPathing : MonoBehaviour
{
    [Header("----- General -----")]
    public GameObject partner;
    public GameObject target;
    public bool holding;
    bool recovering;

    [Header("----- Speed Properties -----")]
    public float moveSpeed;
    [SerializeField] float moveSpeedGuarding = 0.5f;
    [SerializeField] float moveSpeedHelping = 1.5f;
    [SerializeField] float moveSpeedHolding = 0f;

    [Header("----- Hold -----")]
    [SerializeField] float holdDuration = 2f;

    Rigidbody rb;
    GameObject qb;
    Vector3 targetPos;
    public List<GameObject> partners;

    private AnimationTestScript _animControll;
    private bool startAtack;

    private void OnCollisionEnter(Collision _collision)
    {
        if (!holding && _collision.gameObject.tag == PlayerRole.EnemyTeam)
        {
            holding = true;
        }
    }

    private void OnTriggerEnter(Collider _trigger)
    {
        if (_trigger.gameObject.name == PlayerRole.OffensiveGuard )
        {
            partners.Add(_trigger.gameObject);
        }
    }

    private void Start()
    {
        partners = new List<GameObject>();
        _animControll = GetComponent<AnimationTestScript>();
        startAtack = true;
    }

    public void SetUpPathing()
    {
        rb = GetComponent<Rigidbody>();
        qb = GameObject.Find("QB");
        Ward();
        Invoke("ChoosePartners", 1f);
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

            if (holding)
            {
                Holding();
            }
            else
            {
                if(partner == null)
                {
                    Guarding();
                }
                else
                {
                    Helping();
                }
            }
        }
    }

    void Guarding()
    {
        //_animControll.PlayIdleAnim();
        moveSpeed = moveSpeedGuarding;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.3f), moveSpeed * Time.deltaTime);
        _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1, target.transform.position.z));
        _animControll.BoolAllyHoldTrueAnim();
        _animControll.BoolRunFalseAnim();

    }

    void Helping()
    {
        //_animControll.PlayRunAnim();

        _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1, target.transform.position.z));
        _animControll.BoolAllyHoldFalseAnim();
        _animControll.BoolRunTrueAnim();

        moveSpeed = moveSpeedHelping;
        transform.position = Vector3.MoveTowards(transform.position, partner.transform.position, moveSpeed * Time.deltaTime);
    }

    void Holding()
    {
        //_animControll.PlayIdleAnim();
        _animControll._MeshTrans.LookAt(new Vector3(target.transform.position.x, _animControll.transform.position.y - 1, target.transform.position.z));
        _animControll.BoolAllyHoldTrueAnim();
        _animControll.BoolRunFalseAnim();

        moveSpeed = moveSpeedHolding;

        if (Vector3.Distance(transform.position, target.transform.position) < 1f)
        {
            transform.parent = partner.transform;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    void Ward()
    {
        GetComponent<BoxCollider>().enabled = true;

        int rng = Random.Range(0, 100);
        if(rng < 49)
            gameObject.GetComponent<BoxCollider>().center = new Vector3(-2, 0, 0.5f);
        else
            gameObject.GetComponent<BoxCollider>().center = new Vector3(2, 0, 0.5f);
    }

    void ChoosePartners()
    {
        partner = partners[Random.Range(0, partners.Count)];
        
        if(partner.name == PlayerRole.OffensiveGuard)
        {
            target = partner.GetComponent<OffensiveGuard>().target;
            partner.GetComponent<OffensiveGuard>().holdDuration += 1f;
        }

        if (partner.name == PlayerRole.OffensiveTackle)
        {
            target = partner.GetComponent<OffensiveTackle>().target;
            partner.GetComponent<OffensiveTackle>().holdDuration += 1f;
        }
    }

    private void FixedUpdate()
    {
        if (GeneralGameHandler.downStarted)
            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 4f)
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;// gameObject.GetComponent<Rigidbody>().velocity.normalized * 4;
    }
}
