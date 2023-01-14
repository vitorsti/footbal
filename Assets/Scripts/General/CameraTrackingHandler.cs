using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackingHandler : MonoBehaviour
{
    [Header("Objets")]
    public GameObject target;
    public GameObject ball;
    public GameObject qb;
    public GameObject receiver;
    public GameObject preSetButton;

    public Animator _animCanvas;
    public GameObject _joyStickCanvas;

    Vector3 finalPos;
    public float speed;
    public bool runnerblock;

    Vector3 posValues;
    //Quaternion rotValues;

    bool ballTarget;
    bool transition;

    [SerializeField] Vector3 velPosition = new Vector3(3, 5, 4);
    [SerializeField] Quaternion velRotation = new Quaternion(3, 5, 4, 0);

    [Header("Camera Formation")]
    public Transform formationCamPos;
    public float formationOrthoSize;

    [Header("Camera PreSet")]
    public Transform preSetCamPos;

    [Header("Camera Quarterback")]
    public Transform quarterCambackPos;
    public Transform ballZoomPos;

    [Header("Camera Runner")]
    public Transform runnerCamPos;

    private void Awake()
    {
        //if(formationCamPos == null)
        //    formationCamPos = Resources.Load<Transform>("Utilities/CameraFormationPosition");

        //if (preSetCamPos == null)
        //    preSetCamPos = Resources.Load<Transform>("Utilities/CameraPreSetPosition");

        //if (quarterCambackPos == null)
        //    quarterCambackPos = Resources.Load<Transform>("Utilities/CameraQuarterbackPosition");

        //if (runnerCamPos == null)
        //    runnerCamPos = Resources.Load<Transform>("Utilities/CameraRunnerPosition");

        //if (ballZoomPos == null)
        //    ballZoomPos = Resources.Load<Transform>("Utilities/CameraBallZoomPosition");
    }

    public void SetTarget()
    {
        if(qb == null)
            qb = GameObject.Find(PlayerRole.Quarterback);

        if (ball == null)
            ball = GameObject.Find("Ball");
            //ball = GameObject.FindObjectOfType<GameBallController>().gameObject;


        if (receiver == null)
            receiver = GameObject.Find(PlayerRole.Receiver);

        switch (GameStateManager.GameState)
        {
            case GameStateManager.StateOfTheGame.Formation:
                setCameraFormation();
                break;

            case GameStateManager.StateOfTheGame.LoadingQB:
                setCameraQuarterback();
                break;

            case GameStateManager.StateOfTheGame.Runner:
                setCameraRunner();
                break;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            SetTarget();

        if (GameStateManager.GameState == GameStateManager.StateOfTheGame.Quarterback && ball != null && ball.GetComponent<GameBallController>().flying)
            target = ball;

        switch (GameStateManager.GameState)
        {
            case GameStateManager.StateOfTheGame.Formation:
                Formation();
                break;

            case GameStateManager.StateOfTheGame.Quarterback:
                Quarterback();
                break;

            case GameStateManager.StateOfTheGame.Runner:
                Runner();
                break;
        }
    }

    void Formation()
    {
        Time.timeScale = 1f;

        //setCameraFormation();

        if (preSetButton == null)
            preSetButton = GameObject.Find("PreSetButton");

        if (preSetButton.activeSelf)
        {
            speed = 0.5f;
            posValues = formationCamPos.position;
            transform.rotation = formationCamPos.rotation;
            //velPosition = new Vector3(formationCamPos.localScale.x, formationCamPos.localScale.y, formationCamPos.localScale.z);
        }
        else
        {
            speed = 0.5f;
            posValues = preSetCamPos.position;
            transform.rotation = preSetCamPos.rotation;
            //velPosition = new Vector3(preSetCamPos.localScale.x, preSetCamPos.localScale.y, preSetCamPos.localScale.z);
        }

        target = qb;
        ballTarget = false;
        runnerblock = false;

        if (!transition)
        {
            finalPos = new Vector3(target.transform.position.x + posValues.x, target.transform.position.y + posValues.y, target.transform.position.z + posValues.z);
            transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref velPosition, speed);
        }
    }


    void Quarterback()
    {
        if (!transition)
        {
            transform.position = new Vector3(qb.transform.position.x, qb.transform.position.y + 5, qb.transform.position.z - 10);

        }
        //StartCoroutine(FadeCanvasPassing());

        //if (GameStateManager.GameState == GameStateManager.StateOfTheGame.Quarterback && ball.GetComponent<GameBallController>().flying && !ballTarget)
        //{
        //    velPosition = new Vector3(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y, (target.transform.position.z - transform.position.z));
        //    ballTarget = true;
        //}
        //else if (ballTarget)
        //{
        //    //if(qb.GetComponent<QuarterbackController>().willThrow)
        //    //    Time.timeScale = 0.8f;
        //    //else
        //    //    Time.timeScale = 0.4f;

        //    target = ball;
        //    posValues = ballZoomPos.position;
        //    speed = 1f;

        //    finalPos = new Vector3(target.transform.position.x + posValues.x, target.transform.position.y + posValues.y, target.transform.position.z + posValues.z);
        //    Vector3 lTargetDir = ball.transform.position - transform.position;
        //    //lTargetDir.y = 0.0f;

        //    if (Vector3.Distance(transform.position, ball.transform.position) > 5f)
        //    {
        //        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * speed);

        //        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(finalPos.x, transform.position.y, transform.position.z), ref velPosition, 0.5f);
        //        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, finalPos.y, transform.position.z), ref velPosition, 1.5f);
        //        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, transform.position.y, finalPos.z), ref velPosition, 3f);
        //    }
        //}
        //else
        //{
        //    target = qb;
        //    speed = 1f;
        //    finalPos = new Vector3(target.transform.position.x + posValues.x, target.transform.position.y + posValues.y, target.transform.position.z + posValues.z);
        //    transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref velPosition, speed);
        //}
    }


    void Runner()
    {
        Time.timeScale = 1f;
        ballTarget = false;
        speed = 1;

        finalPos = new Vector3(target.transform.position.x + posValues.x, target.transform.position.y + posValues.y, target.transform.position.z + posValues.z);
        //rotValues = Quaternion.Euler(new Vector3(runnerCamPos.eulerAngles.x, runnerCamPos.eulerAngles.y, runnerCamPos.eulerAngles.z));

        Vector3 lTargetDir = receiver.transform.position - transform.position;
        //lTargetDir.y = 0.0f;

        //if (!transition)
        //{
        //    if (Vector3.Distance(transform.position, receiver.transform.position) > 3f && !runnerblock)
        //    {
        //        //if (transform.position.z < receiver.transform.position.z)
        //        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * speed);

        //        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(finalPos.x, transform.position.y, transform.position.z), ref velPosition, 0.4f);
        //        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, finalPos.y, transform.position.z), ref velPosition, 0.9f);
        //        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, transform.position.y, finalPos.z), ref velPosition, 3f);
        //    }
        //    else
        //    {
        //        runnerblock = true;
        //    }

        //    if (runnerblock)
        //    {
        //        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref velPosition, 0.2f);
        //    }
        //}
        transform.position = new Vector3(receiver.transform.position.x, receiver.transform.position.y + 3.5f, receiver.transform.position.z - 6);

    }

    void setCameraFormation()
    {
        StartCoroutine(FadeCanvasFormation());

        //rotValues = Quaternion.Euler(new Vector3(formationCamPos.eulerAngles.x, formationCamPos.eulerAngles.y, formationCamPos.eulerAngles.z));
        gameObject.GetComponent<Camera>().orthographic = true;
        gameObject.GetComponent<Camera>().orthographicSize = formationOrthoSize;
    }

    void setCameraQuarterback()
    {
        StartCoroutine(FadeCanvasQuarterback());

        //transform.position = new Vector3(qb.transform.position.x + 5f, qb.transform.position.y + 20f, qb.transform.position.z);
        //posValues = quarterCambackPos.position;
        //rotValues = Quaternion.Euler(new Vector3(quarterCambackPos.eulerAngles.x, quarterCambackPos.eulerAngles.y, quarterCambackPos.eulerAngles.z));
        
        velPosition = new Vector3(quarterCambackPos.localScale.x, quarterCambackPos.localScale.y, quarterCambackPos.localScale.z);
        speed = 2f;

    }

    void setCameraRunner()
    {
        StartCoroutine(FadeCanvasRunning());

        posValues = runnerCamPos.position;
        //rotValues = Quaternion.Euler(new Vector3(runnerCamPos.eulerAngles.x, runnerCamPos.eulerAngles.y, runnerCamPos.eulerAngles.z));
        //velPosition = new Vector3(runnerCamPos.localScale.x, runnerCamPos.localScale.y, runnerCamPos.localScale.z);
        target = receiver;
        velPosition = new Vector3(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y, (target.transform.position.z - transform.position.z) * 2);
    }

    public IEnumerator FadeCanvasQuarterback()
    {
        Time.timeScale = 0.5f;
        transition = true;

        _animCanvas.SetTrigger("Transition");
        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Camera>().orthographic = false;

        transform.position = new Vector3(qb.transform.position.x, qb.transform.position.y + 5, qb.transform.position.z - 10);
        transform.rotation = Quaternion.identity;
        transform.localRotation = Quaternion.Euler(20, 0, 0);

        transition = false;
        Time.timeScale = 1;
        yield return new WaitForSeconds(1f);

        //cTS.enabled = true;
        //pS.enabled = true;
        //_mainCanvas.SetActive(false);
    }

    public IEnumerator FadeCanvasRunning()
    {
        Time.timeScale = 0.5f;
        //_animCanvas.SetTrigger("Transition");
        //yield return new WaitForSeconds(0.5f);

        transform.position = new Vector3(receiver.transform.position.x, receiver.transform.position.y + 3.5f, receiver.transform.position.z - 6);
        //transform.rotation = new Quaternion.
        transform.localRotation = Quaternion.Euler(25, 0, 0);
        transition = false;
        yield return new WaitForSeconds(1f);
        //_mainCanvas.SetActive(false);
        _joyStickCanvas.SetActive(true);
    }

    public IEnumerator FadeCanvasFormation()
    {
        transition = true;
        //Time.timeScale = 0.1f;
        _animCanvas.SetTrigger("Transition");
        yield return new WaitForSeconds(0.7f);

        if (preSetButton.activeSelf)
        {
            speed = 0.5f;
            posValues = formationCamPos.position;
            transform.rotation = formationCamPos.rotation;
            //velPosition = new Vector3(formationCamPos.localScale.x, formationCamPos.localScale.y, formationCamPos.localScale.z);
        }
        else
        {
            speed = 0.5f;
            posValues = preSetCamPos.position;
            transform.rotation = preSetCamPos.rotation;
            //velPosition = new Vector3(preSetCamPos.localScale.x, preSetCamPos.localScale.y, preSetCamPos.localScale.z);
        }

        finalPos = new Vector3(target.transform.position.x + posValues.x, target.transform.position.y + posValues.y, target.transform.position.z + posValues.z);
        transform.position = finalPos;

        yield return new WaitForSeconds(1f);
        transition = false;
        //_mainCanvas.SetActive(false);
        _joyStickCanvas.SetActive(true);
    }

    public IEnumerator FadeCanvas()
    {
        Time.timeScale = 1f;
        _animCanvas.SetTrigger("Transition");
        yield return new WaitForSeconds(2f);
        //yield return new WaitForSeconds(0.5f);
        transition = false;
    }
}
