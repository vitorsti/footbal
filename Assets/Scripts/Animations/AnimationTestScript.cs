using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestScript : MonoBehaviour
{

    //[SerializeField]
    //float vel = 6, velRot = 100;

    public Animator _playerAnmt;

    public Transform _MeshTrans;

    [HideInInspector]
    public GameObject _target;

    void Update()
    {
        //Sempre procurar a posição da Bola
        if (_target == null && GameStateManager.GameState != GameStateManager.StateOfTheGame.Formation)
            _target = GameObject.Find("Ball");
            //_target = GameObject.FindObjectOfType<GameBallController>().gameObject.GetComponent<Transform>();

        //float Move = Input.GetAxis("Vertical") * vel;
        //float Rota = Input.GetAxis("Horizontal") * velRot;

        //Move *= Time.deltaTime;
        // Rota *= Time.deltaTime;

        //transform.Rotate(0, Rota, 0);
    }

    //------------ Validador de Bool ------------//
    public void BoolRunTrueAnim()
    {
        _playerAnmt.SetBool("Run",true);
    }
    public void BoolRunFalseAnim()
    {
        _playerAnmt.SetBool("Run", false);
    }
    public void BoolHasBallTrueAnim()
    {
        _playerAnmt.SetBool("HasBall", true);
    }
    public void BoolHasBallFalseAnim()
    {
        _playerAnmt.SetBool("HasBall", false);
    }
    public void BoolAllyHoldTrueAnim()
    {
        _playerAnmt.SetBool("Ally-Hold", true);
    }
    public void BoolAllyHoldFalseAnim()
    {
        _playerAnmt.SetBool("Ally-Hold", false);
    }
    public void BoolEnemyHoldTrueAnim()
    {
        _playerAnmt.SetBool("Enemy-Hold", true);
    }
    public void BoolEnemyHoldlFalseAnim()
    {
        _playerAnmt.SetBool("Enemy-Hold", false);
    }
    //-------------------------------------------//

    public void PlayRunAnim()
    {
        _playerAnmt.SetBool("Correr", true);
        _playerAnmt.SetBool("Jump", false);
    }

    public void PlayJumpAnim()
    {
        _playerAnmt.SetBool("Jump", true);
        _playerAnmt.SetBool("Correr", false);
    }

    public void PlayIdleAnim()
    {
        _playerAnmt.SetBool("Correr", false);
        _playerAnmt.SetBool("Jump", false);
    }

    public void PlayRunWithBallAnim()
    {
        _playerAnmt.SetBool("HasBall", true);
        _playerAnmt.SetBool("Run", true);
    }

    public void GetBallAnim()
    {
        _playerAnmt.SetTrigger("GetBall");
        _playerAnmt.SetTrigger("HasBall");
    }

    public void BreakTackleAnim()
    {
        _playerAnmt.SetTrigger("Dodging");
    }

    /*
    public void StartAtackAnim()
    {
        _playerAnmt.SetTrigger("OL-Start");
    }   
    */
    public void StartWRAnim()
    {
        _playerAnmt.SetTrigger("WR-Start");
    }
    public void QBBackWalkAnim()
    {
        _playerAnmt.SetTrigger("QB-Start");
    }

    public void QBThrowBallAnim()
    {
        _playerAnmt.SetTrigger("QB-Throw");
    }

    public void TryCatchBallAnim()
    {
        _playerAnmt.SetTrigger("Try Catch");
        _playerAnmt.SetBool("OnFloor",false);
    }

    public void OnFloorAnim()
    {
        _playerAnmt.SetBool("OnFloor", true);
    }

    public void TryCatchReciverAnim()
    {
        _playerAnmt.SetTrigger("Try Catch");
        _playerAnmt.SetBool("Enemy-Hold", false);
    }

    public void EndDownAnim()
    {
        _playerAnmt.SetTrigger("EndDown");
    }

}
