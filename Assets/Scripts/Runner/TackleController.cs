using System;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TackleController : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("----- EDITOR MODE -----")]
    public bool showTextMode = true;
    public bool showLines = true;
#endif

    [Header("----- General -----")]
    public GameObject receiver;
    [SerializeField] bool recovering;
    [SerializeField] bool onTackle;
    [SerializeField] bool holded;

    [Header("----- Speed Properties -----")]
    public float defensorSpeed = 1f;
    [SerializeField] float speedFrontTackle = 2.4f;
    [SerializeField] float speedSideTackle = 2.9f;
    [SerializeField] float speedProtectingArea = 2.2f;
    [SerializeField] float speedSideToSide = 2.6f;
    [SerializeField] float speedChasingBehind = 2.7f;
    [SerializeField] float speedBehindTackle = 2.4f;
    [SerializeField] float speedJustRun = 3.2f;
    [SerializeField] float speedTowards = 2.5f;

    [Header("----- Tackle Properties -----")]
    [SerializeField] private float tackleDistance = 3f;
    [SerializeField] private float tackleSpeed = 3f;
    [SerializeField] private float tackleCooldown = 3f;
    [SerializeField] private float recoveryCooldown = 1f;


    [Header("----- Chasing Mode -----")]
    public ChasingMode chasingMode;
    public DistanceRelativeToReceiver distanceToReceiver;
    public SideRelativeToReceiver sideRelativeToReceiver;

    //Para testes de balanceamento, depois irão se tornar constantes 
    [Header("----- Chasing Values -----")]
    [SerializeField] private float checkDistanceTime = 0.7f;
    //[SerializeField] private float toFar;
    [SerializeField] private float toCanChase = 12f;
    [SerializeField] private float toNear = 7f;
    [SerializeField] private float toOnRange = 4f;

    [Header("----- Distance Range -----")]
    [SerializeField] private float maxToAside = 3f;
    [SerializeField] private float maxToAlign = 3f;

    //Vars
    float distanceX;
    float distanceZ;
    Vector3 target;
    Vector3 newPos;
    Rigidbody rb;

    private AnimationTestScript _animControll;

    public enum ChasingMode
    {
        FrontTackle,
        SideTackle,
        ProtectingArea,
        SideToSide,
        ChasingBehind,
        BehindTackle,
        JustRun,
        Towards,
        Holded
    }

    public enum DistanceRelativeToReceiver
    {
        Far,
        CanChase,
        Near,
        OnRange
    }

    public enum SideRelativeToReceiver
    {
        FrontAligned,
        BehindAligned,
        Aside,
        FrontDiagonal,
        BehindDiagonal
    }
    void Start()
    {
        receiver = GameObject.FindObjectOfType<ReceiverController>().gameObject;
        rb = gameObject.GetComponent<Rigidbody>();

        _animControll = GetComponent<AnimationTestScript>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        StartCoroutine(DefensorUpdate());
    }

    IEnumerator DefensorUpdate()
    {
        while (GeneralGameHandler.downStarted)
        {
            CheckDistance();
            CheckPositionRelativeToReceiver();
            ChangeChasingMode();

            yield return new WaitForSecondsRealtime(checkDistanceTime);
        }

        StopAllCoroutines();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
            StartCoroutine(DefensorUpdate());

        showLines = true;
#endif

        if (rb.constraints != RigidbodyConstraints.FreezeRotation)
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (GeneralGameHandler.downStarted)
        {
            if (!recovering && !holded)
            {

                _animControll.BoolEnemyHoldlFalseAnim();

                if (chasingMode == ChasingMode.JustRun)
                {
                    JustRun();
                }

                if (chasingMode == ChasingMode.ChasingBehind)
                {
                    ChasingBehind();
                }

                if (chasingMode == ChasingMode.SideToSide)
                {
                    SideToSide();
                }

                if (chasingMode == ChasingMode.ProtectingArea)
                {
                    ProtectingArea();
                }

                if (chasingMode == ChasingMode.Towards)
                {
                    Towards();
                }

                if (chasingMode == ChasingMode.SideTackle)
                {
                    SideTackle();
                }

                if (chasingMode == ChasingMode.FrontTackle)
                {
                    FrontTackle();
                }

                if (chasingMode == ChasingMode.BehindTackle)
                {
                    BehindTackle();
                }
            }
            else if (holded)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, defensorSpeed * Time.deltaTime);

                _animControll._MeshTrans.LookAt(new Vector3(newPos.x, _animControll.transform.position.y - 1, newPos.z));
                _animControll.BoolEnemyHoldTrueAnim();
                _animControll.BoolRunFalseAnim();

            }
            else if (recovering)
            {
                transform.position = Vector3.Slerp(transform.position, target, 1f * Time.deltaTime);

                _animControll.BoolEnemyHoldlFalseAnim();
                _animControll.BoolRunTrueAnim();
            }
        }
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, receiver.transform.position);

        switch (distance)
        {
            case float _ when (distance > toCanChase):
                distanceToReceiver = DistanceRelativeToReceiver.Far;
                break;

            case float _ when (distance > toNear && distance <= toCanChase):
                distanceToReceiver = DistanceRelativeToReceiver.CanChase;
                break;

            case float _ when (distance > toOnRange && distance <= toNear):
                distanceToReceiver = DistanceRelativeToReceiver.Near;
                break;

            case float _ when (distance <= toOnRange):
                distanceToReceiver = DistanceRelativeToReceiver.OnRange;
                break;
        }
    }

    void CheckPositionRelativeToReceiver()
    {
        distanceX = transform.position.x - receiver.transform.position.x;
        distanceZ = transform.position.z - receiver.transform.position.z;

        //Check is on Range to be Aside
        if (distanceZ > -maxToAside && distanceZ < maxToAside)
        {
            sideRelativeToReceiver = SideRelativeToReceiver.Aside;
        }

        //Out of Range means in Front (> 0) or Behind Receiver (< 0)
        else
        {
            if (distanceZ >= 0f)
            {
                if (distanceX > -maxToAlign && distanceX < maxToAlign)
                {
                    sideRelativeToReceiver = SideRelativeToReceiver.FrontAligned;
                }
                else
                {
                    sideRelativeToReceiver = SideRelativeToReceiver.FrontDiagonal;
                }
            }
            else
            {
                if (distanceX > -maxToAlign && distanceX < maxToAlign)
                {
                    sideRelativeToReceiver = SideRelativeToReceiver.BehindAligned;
                }
                else
                {
                    sideRelativeToReceiver = SideRelativeToReceiver.BehindDiagonal;
                }
            }
        }
    }

    void ChangeChasingMode()
    {
        //Just Run
        if (distanceToReceiver == DistanceRelativeToReceiver.Far && (sideRelativeToReceiver == SideRelativeToReceiver.BehindAligned || sideRelativeToReceiver == SideRelativeToReceiver.BehindDiagonal))
        {
            chasingMode = ChasingMode.JustRun;
        }

        else if (distanceToReceiver == DistanceRelativeToReceiver.Far && (sideRelativeToReceiver == SideRelativeToReceiver.Aside))
        {
            chasingMode = ChasingMode.JustRun;
        }

        //Chasing Behind
        else if (distanceToReceiver == DistanceRelativeToReceiver.CanChase && (sideRelativeToReceiver == SideRelativeToReceiver.BehindAligned || sideRelativeToReceiver == SideRelativeToReceiver.BehindDiagonal))
        {
            chasingMode = ChasingMode.ChasingBehind;
        }

        else if (distanceToReceiver == DistanceRelativeToReceiver.OnRange && (sideRelativeToReceiver == SideRelativeToReceiver.BehindDiagonal))
        {
            chasingMode = ChasingMode.ChasingBehind;
        }

        //Side To Side
        else if (distanceToReceiver == DistanceRelativeToReceiver.CanChase && (sideRelativeToReceiver == SideRelativeToReceiver.Aside))
        {
            chasingMode = ChasingMode.SideToSide;
        }

        //Side Tackle
        else if (distanceToReceiver == DistanceRelativeToReceiver.OnRange && (sideRelativeToReceiver == SideRelativeToReceiver.Aside))
        {
            chasingMode = ChasingMode.SideTackle;
        }

        //Front Tackle
        else if (distanceToReceiver == DistanceRelativeToReceiver.OnRange && (sideRelativeToReceiver == SideRelativeToReceiver.FrontAligned || sideRelativeToReceiver == SideRelativeToReceiver.FrontDiagonal))
        {
            chasingMode = ChasingMode.FrontTackle;
        }

        //Protecting Area
        else if (distanceToReceiver == DistanceRelativeToReceiver.Far && (sideRelativeToReceiver == SideRelativeToReceiver.FrontAligned || sideRelativeToReceiver == SideRelativeToReceiver.FrontDiagonal))
        {
            chasingMode = ChasingMode.ProtectingArea;
        }

        //Behind Tackle
        else if (distanceToReceiver == DistanceRelativeToReceiver.OnRange && (sideRelativeToReceiver == SideRelativeToReceiver.BehindAligned))
        {
            chasingMode = ChasingMode.BehindTackle;
        }

        //Towards
        else if (distanceToReceiver == DistanceRelativeToReceiver.CanChase && (sideRelativeToReceiver == SideRelativeToReceiver.FrontAligned))
        {
            chasingMode = ChasingMode.Towards;
        }

        //Random Cases
        else if (distanceToReceiver == DistanceRelativeToReceiver.CanChase && (sideRelativeToReceiver == SideRelativeToReceiver.FrontDiagonal))
        {
            chasingMode = GetRandomMode(ChasingMode.ProtectingArea, ChasingMode.Towards);
        }

        else if (distanceToReceiver == DistanceRelativeToReceiver.Near && (sideRelativeToReceiver == SideRelativeToReceiver.FrontDiagonal))
        {
            chasingMode = GetRandomMode(ChasingMode.ProtectingArea, ChasingMode.Towards);
        }

        else if (distanceToReceiver == DistanceRelativeToReceiver.Near && (sideRelativeToReceiver == SideRelativeToReceiver.Aside))
        {
            chasingMode = GetRandomMode(ChasingMode.SideToSide, ChasingMode.SideTackle);
        }

        else if (distanceToReceiver == DistanceRelativeToReceiver.Near && (sideRelativeToReceiver == SideRelativeToReceiver.BehindAligned || sideRelativeToReceiver == SideRelativeToReceiver.BehindDiagonal))
        {
            chasingMode = GetRandomMode(ChasingMode.ChasingBehind, ChasingMode.BehindTackle);
        }
    }

    void FrontTackle()
    {
        defensorSpeed = speedFrontTackle;
        newPos = new Vector3(receiver.transform.position.x, receiver.transform.position.y, receiver.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPos, defensorSpeed * Time.deltaTime);
        Tackle();
    }

    void SideTackle()
    {
        defensorSpeed = speedSideTackle;
        newPos = new Vector3(receiver.transform.position.x, receiver.transform.position.y, receiver.transform.position.z + 2f);
        transform.position = Vector3.MoveTowards(transform.position, newPos, defensorSpeed * Time.deltaTime);
        Tackle();
    }

    void BehindTackle()
    {
        defensorSpeed = speedBehindTackle;
        newPos = new Vector3(receiver.transform.position.x, receiver.transform.position.y, receiver.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPos, defensorSpeed * Time.deltaTime);
        Tackle();
    }

    void ProtectingArea()
    {
        defensorSpeed = speedProtectingArea;
        //newPos = new Vector3(receiver.transform.position.x / UnityEngine.Random.Range(0.9f, 1.5f), receiver.transform.position.y, transform.position.z - UnityEngine.Random.Range(0, 3f));
        newPos = new Vector3(receiver.transform.position.x / 1.3f, receiver.transform.position.y, transform.position.z - 0.5f);
        transform.position = Vector3.MoveTowards(transform.position, newPos, defensorSpeed * Time.deltaTime);

        if (distanceToReceiver == DistanceRelativeToReceiver.Near || distanceToReceiver == DistanceRelativeToReceiver.OnRange)
            Tackle();
    }

    void SideToSide()
    {
        defensorSpeed = speedSideToSide;

        if (distanceX < 0)
            newPos = new Vector3(receiver.transform.position.x - distanceX / 7, transform.position.y, receiver.transform.position.z + 2f);
        else
            newPos = new Vector3(receiver.transform.position.x + distanceX / 7, transform.position.y, receiver.transform.position.z + 2f);

        transform.position = Vector3.MoveTowards(transform.position, newPos, defensorSpeed * Time.deltaTime);
    }

    void ChasingBehind()
    {
        defensorSpeed = speedChasingBehind;

        if (distanceX < 0)
            newPos = new Vector3(receiver.transform.position.x - 3f, receiver.transform.position.y, receiver.transform.position.z);
        else
            newPos = new Vector3(receiver.transform.position.x + 3f, receiver.transform.position.y, receiver.transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, newPos, defensorSpeed * Time.deltaTime);

        _animControll._MeshTrans.LookAt(new Vector3(newPos.x, _animControll.transform.position.y - 1, newPos.z));
        _animControll.BoolEnemyHoldlFalseAnim();
        _animControll.BoolRunTrueAnim();

    }

    void JustRun()
    {
        defensorSpeed = speedJustRun;

        if (distanceX < 0)
            newPos = new Vector3(receiver.transform.position.x - distanceX/3, transform.position.y, receiver.transform.position.z + 7f);
        else
            newPos = new Vector3(receiver.transform.position.x + distanceX/3, transform.position.y, receiver.transform.position.z + 7f);

        transform.position = Vector3.MoveTowards(transform.position, newPos, defensorSpeed * Time.deltaTime);

        _animControll._MeshTrans.LookAt(new Vector3(newPos.x, _animControll.transform.position.y - 1, newPos.z));
        _animControll.BoolEnemyHoldlFalseAnim();
        _animControll.BoolRunTrueAnim();

    }

    void Towards()
    {
        defensorSpeed = speedTowards;
        newPos = new Vector3(receiver.transform.position.x, receiver.transform.position.y, receiver.transform.position.z + 4f);
        transform.position = Vector3.MoveTowards(transform.position, newPos, defensorSpeed * Time.deltaTime);

        if (distanceToReceiver == DistanceRelativeToReceiver.Near || distanceToReceiver == DistanceRelativeToReceiver.OnRange)
            Tackle();

        _animControll._MeshTrans.LookAt(new Vector3(newPos.x, _animControll.transform.position.y - 1, newPos.z));
        _animControll.BoolEnemyHoldlFalseAnim();
        _animControll.BoolRunTrueAnim();
    }

    void Tackle()
    {

        if (Vector3.Distance(transform.position, receiver.transform.position) < tackleDistance)
        {
            if (!onTackle)
            {
                onTackle = true;
                target = new Vector3(receiver.transform.position.x, receiver.transform.position.y, receiver.transform.position.z);
                Invoke("ResetTackle", tackleCooldown);
            }
            else
            {
                transform.position = Vector3.Slerp(transform.position, target, Time.deltaTime * tackleSpeed);

                if (Vector3.Distance(transform.position, target) <= 0.1f)
                {
                    recovering = true;
                    Invoke("ResetRecovery", recoveryCooldown);
                }

                _animControll._MeshTrans.LookAt(new Vector3(target.x, _animControll.transform.position.y - 1, target.z));
                _animControll.TryCatchReciverAnim();

                gameObject.GetComponent<Renderer>().material.color = Color.red;  //remover
            }
        }

        //ResetTackle();
    }

    public void HoldedBy(float _duration, float _speed)
    {
        if (!holded)
        {
            holded = true;
            defensorSpeed = _speed + 0.1f;
            StopCoroutine(DefensorUpdate());
            chasingMode = ChasingMode.Holded;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            Invoke("ResetFromHolded", _duration);
        }
    }

    void ResetFromHolded()
    {
        if (distanceX > 0)
            target = new Vector3(transform.position.x + 3f, receiver.transform.position.y, transform.position.z);
        else if (distanceX <= 0)
            target = new Vector3(transform.position.x + 3f, receiver.transform.position.y, transform.position.z);

        holded = false;
        recovering = true;
        //rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        StartCoroutine(DefensorUpdate());
        Invoke("ResetRecovery", 1f);
    }

    void ResetRecovery()
    {
        recovering = false;
    }

    void ResetTackle()
    {
        onTackle = false;
        gameObject.GetComponent<Renderer>().material.color = Color.white;  //remover
    }

    ChasingMode GetRandomMode(ChasingMode a, ChasingMode b)
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        int rng = UnityEngine.Random.Range(1, 11);

        if (rng <= 5)
            return a;
        else
            return b;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showLines)
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;
            style.fontStyle = FontStyle.Bold;
            GizmoDrawLine(transform.position,
                            newPos,
                            Color.black, chasingMode.ToString(), Color.red);
        }
    }

    private void GizmoDrawLine(Vector3 from, Vector3 to, Color lineColor, string text, Color textColor)
    {
        Handles.color = lineColor;
        Handles.DrawAAPolyLine(5f, from, to);
        Vector3 dir = (to - from).normalized;
        float distance = Vector3.Distance(from, to);

        for (float i = 0; i < distance; i += 1f)
        {
            Handles.DrawAAPolyLine(
                5f,
                from + dir * i,
                from + (dir * (i - .15f)) + Quaternion.AngleAxis(Time.realtimeSinceStartup * 360f, dir.normalized * 300f) * Vector3.up * .05f
            );
            Handles.DrawAAPolyLine(
                5f,
                from + dir * i,
                from + (dir * (i - .15f)) + Quaternion.AngleAxis(Time.realtimeSinceStartup * 360f + 180, dir.normalized * 300f) * Vector3.up * .05f
            );
        }

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = textColor;

        if(showTextMode)
            Handles.Label(from + (dir * distance * .5f), text, style);
    }
#endif
}