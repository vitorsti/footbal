using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TeammateController : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("----- EDITOR MODE -----")]
    public bool showTextMode = true;
    public bool showLines = true;
#endif

    [Header("----- General -----")]
    public GameObject receiver;

    [SerializeField] bool holding;
    [SerializeField] bool canHold;

    [Header("----- Speed Properties -----")]
    public float teammateSpeed = 1f;
    public float recoveringSpeed = 1f;
    [SerializeField] float speedRun = 2.3f;
    [SerializeField] float speedGoingHelp = 1.5f;
    [SerializeField] float speedGuard = 0.3f;
    [SerializeField] float speedHold = 0.5f;
    [SerializeField] float speedReceiver;


    [Header("----- Holding Properties -----")]
    [SerializeField] private float holdingDuration = 1f;
    [SerializeField] private float holdingCooldown = 4f;
    [SerializeField] private float holdingDistance = 1.5f;


    [Header("----- Guard Mode -----")]
    public TeammateAction teammateAction;
    public DistanceRelativeToPlayer distanceRelativeToPlayer;

    //Para testes de balanceamento, depois irão se tornar constantes 
    [Header("----- Chasing Values -----")]
    [SerializeField] private float checkDistanceTime = 1f;
    //[SerializeField] private float toFar;
    [SerializeField] private float toMedium = 11;
    [SerializeField] private float toClose = 6f;
    [SerializeField] private float minReceiverDistance = 4f;

    //Positions
    float distanceX;
    float distanceZ;
    float modX;
    float modZ;
    Vector3 target;

    //Vars
    public List<GameObject> enemiesinRange = new List<GameObject>();
    public GameObject holdingEnemy;
    Rigidbody rb;

    private AnimationTestScript _animControll;

    public enum TeammateAction
    {
        Holding, 
        Guard, 
        GoingToHelp, 
        Running
    }

    public enum DistanceRelativeToPlayer
    {
        Far,
        Medium,
        Close
    }

    void Start()
    {
        SphereCollider sphere = gameObject.GetComponent<SphereCollider>();

        if (sphere != null)
            sphere.enabled = false;

        gameObject.GetComponent<Renderer>().material.color = Color.green;
        receiver = GameObject.FindObjectOfType<ReceiverController>().gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        target = receiver.transform.position;
        canHold = true;
        _animControll = GetComponent<AnimationTestScript>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        StartCoroutine(TeammateUpdate());
    }

    IEnumerator TeammateUpdate()
    {
        while (GeneralGameHandler.downStarted)
        {
            CheckDistance();
            CheckAction();

            yield return new WaitForSecondsRealtime(checkDistanceTime);
        }

        StopAllCoroutines();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
            StartCoroutine(TeammateUpdate());

        showLines = false;
#endif
        if (rb.constraints != RigidbodyConstraints.FreezeRotation)
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (GeneralGameHandler.downStarted)
        {
            if (!holding)
            {
                if (teammateAction == TeammateAction.Running)
                {
                    Running();
                }

                else if (teammateAction == TeammateAction.GoingToHelp)
                {
                    GoingToHelp();
                }

                else if (teammateAction == TeammateAction.Guard)
                {
                    Guard();
                }

                else if (teammateAction == TeammateAction.Holding)
                {
                    Hold();
                }
            }
            else if (holding)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.blue; //remover
                if (holdingEnemy != null)
                    holdingEnemy.GetComponent<TackleController>().HoldedBy(holdingDuration, teammateSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider _Collider)
    {
        if (_Collider.gameObject.tag == "EnemyTeam")
        {
            if (!enemiesinRange.Exists(x => x.gameObject == _Collider.gameObject))
                enemiesinRange.Add(_Collider.gameObject);
        }
    }

    private void OnTriggerExit(Collider _Collider)
    {
        if (_Collider.gameObject.tag == "EnemyTeam")
        {
            if (enemiesinRange.Count > 1)
            {
                if (enemiesinRange.Exists(x => x.gameObject == _Collider.gameObject))
                    enemiesinRange.Remove(_Collider.gameObject);
            }
            else
            {
                if (enemiesinRange.Count != 0)
                {
                    enemiesinRange.RemoveAt(0);
                    holdingEnemy = null;
                }
            }
        }
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, receiver.transform.position);
        distanceX = transform.position.x - receiver.transform.position.x;
        distanceZ = transform.position.z - receiver.transform.position.z;

        switch (distance)
        {
            case float _ when (distance > toMedium):
                distanceRelativeToPlayer = DistanceRelativeToPlayer.Far;
                break;

            case float _ when (distance > toClose && distance <= toMedium):
                distanceRelativeToPlayer = DistanceRelativeToPlayer.Medium;
                break;

            case float _ when (distance <= toClose):
                distanceRelativeToPlayer = DistanceRelativeToPlayer.Close;
                break;
        }
    }

    void CheckAction()
    {
        if (distanceRelativeToPlayer == DistanceRelativeToPlayer.Far)
        {
            teammateAction = TeammateAction.Running;
        }

        else if(distanceRelativeToPlayer == DistanceRelativeToPlayer.Medium)
        {
            teammateAction = TeammateAction.GoingToHelp;
        }

        else if (distanceRelativeToPlayer == DistanceRelativeToPlayer.Close)
        {
            teammateAction = GetRandomMode(TeammateAction.Guard, TeammateAction.Holding);

            if (distanceX >= 0 && distanceZ >= 0)
            {
                modX = Random.Range(2f, 4f);
                modZ = Random.Range(2f, 4f);
            }

            else if (distanceX >= 0 && distanceZ < 0)
            {
                modX = Random.Range(2f, 4f);
                modZ = Random.Range(2f, 4f);
            }

            else if (distanceX < 0 && distanceZ < 0)
            {
                modX = Random.Range(-4f, -2f);
                modZ = Random.Range(1f, 3f);
            }

            else if (distanceX < 0 && distanceZ >= 0)
            {
                modX = Random.Range(-4f, -2f);
                modZ = Random.Range(1.5f, 4f);
            }
        }
    }

    void Running()
    {
        teammateSpeed = speedRun;

        if (distanceX > 0)
            target = new Vector3(receiver.transform.position.x + distanceX / 4, transform.position.y, receiver.transform.position.z + 4f);
        else
            target = new Vector3(receiver.transform.position.x - distanceX / 4, transform.position.y, receiver.transform.position.z + 4f);

        //_animControll.PlayRunAnim();
        _animControll.BoolRunTrueAnim();
        _animControll.BoolAllyHoldFalseAnim();
        transform.position = Vector3.MoveTowards(transform.position, target, teammateSpeed * Time.deltaTime);
    }

    void GoingToHelp()
    {
        teammateSpeed = speedGoingHelp;
        target = new Vector3(receiver.transform.position.x, receiver.transform.position.y, receiver.transform.position.z + 4f);

        //_animControll.PlayRunAnim();
        _animControll.BoolRunTrueAnim();
        _animControll.BoolAllyHoldFalseAnim();
        transform.position = Vector3.MoveTowards(transform.position, target, teammateSpeed * Time.deltaTime);
        _animControll._MeshTrans.LookAt(new Vector3 (target.x, transform.position.y, target.z));
    }

    void Guard()
    {
        if(distanceZ < 3)
            teammateSpeed = speedGuard;
        else 
            teammateSpeed = speedGuard - 0.2f;

        target = new Vector3(receiver.transform.position.x + modX, receiver.transform.position.y, receiver.transform.position.z + modZ);

        //_animControll.PlayIdleAnim();
        _animControll.BoolRunFalseAnim();
        _animControll.BoolAllyHoldTrueAnim();

        transform.position = Vector3.MoveTowards(transform.position, target, teammateSpeed * Time.deltaTime);

        _animControll._MeshTrans.LookAt(new Vector3(target.x, transform.position.y, target.z));
    }

    void Hold()
    {
        holdingEnemy = null;
        if (!holding && enemiesinRange.Count != 0)
        {

            holdingEnemy = enemiesinRange[0];


            if (enemiesinRange.Count > 1)
            {
                enemiesinRange.ForEach(ep =>
                {
                    if (Vector3.Distance(transform.position, ep.transform.position) < Vector3.Distance(transform.position, holdingEnemy.transform.position))
                        holdingEnemy = ep.gameObject;
                });
            }

            target = new Vector3(holdingEnemy.transform.position.x, holdingEnemy.transform.position.y, holdingEnemy.transform.position.z);

            _animControll._MeshTrans.LookAt(new Vector3(target.x, transform.position.y, target.z));

            if (Vector3.Distance(transform.position, holdingEnemy.transform.position) < holdingDistance && canHold)
            {
                teammateSpeed = speedHold;
                holding = true;
                canHold = false;
                StopCoroutine(TeammateUpdate());
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                //holdingEnemy.transform.position = transform.position;
                //_animControll.PlayJumpAnim();
                _animControll.BoolRunFalseAnim();
                _animControll.BoolAllyHoldTrueAnim();

                Invoke("ResetHolding", holdingDuration);
            }
            else
            {
                teammateSpeed = speedGuard;
                gameObject.GetComponent<Renderer>().material.color = Color.cyan;  //remover

                //_animControll.PlayRunAnim();
                _animControll.BoolRunFalseAnim();
                _animControll.BoolAllyHoldTrueAnim();

                transform.position = Vector3.MoveTowards(transform.position, target, teammateSpeed * Time.deltaTime);
            }
        }
        else
            teammateAction = TeammateAction.Guard;
    }

    void ResetHolding()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;  //remover
        holding = false;
        teammateAction = TeammateAction.GoingToHelp;

        if (Vector3.Distance(transform.position, holdingEnemy.transform.position) < 0.1f)
            target = new Vector3(transform.position.x + 2f, receiver.transform.position.y, transform.position.z);
        else
            target = new Vector3(transform.position.x - 2f, receiver.transform.position.y, transform.position.z);

        _animControll._MeshTrans.LookAt(new Vector3(target.x, transform.position.y, target.z));

        transform.position = Vector3.Slerp(transform.position, target, 1.3f * Time.deltaTime);
        holdingEnemy = null;
        //rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        Invoke("HoldingCooldownCount", holdingCooldown);
        StartCoroutine(TeammateUpdate());
    }

    void HoldingCooldownCount()
    {
        canHold = true;
    }

    TeammateAction GetRandomMode(TeammateAction a, TeammateAction b)
    {
        int rng = Random.Range(1, 11);
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
                            target,
                            Color.white, teammateAction.ToString(), Color.white);
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
