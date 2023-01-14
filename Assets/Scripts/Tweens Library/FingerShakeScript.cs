using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FingerShakeScript : MonoBehaviour
{

    private Transform _gO;

    void Start()
    {
        _gO = this.GetComponent<Transform>();

        StartCoroutine(RotateToCenter());
        StartCoroutine(GoUp());
    }

    IEnumerator RotateToCenter()
    {
        _gO.DOLocalRotate(new Vector3(0, 0, -30), 0.8f, RotateMode.Fast);
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(RotateToSides());
    }

    IEnumerator RotateToSides()
    {
        _gO.DOLocalRotate(new Vector3(0, 0, 30), 0.8f, RotateMode.Fast);
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(RotateToCenter());

    }

    IEnumerator GoUp()
    {
        _gO.DOLocalMoveY(250, 4, false);
        yield return new WaitForSeconds(4f);
        StartCoroutine(GoDown());

    }

    IEnumerator GoDown()
    {
        _gO.DOLocalMoveY(0, 4, false);
        yield return new WaitForSeconds(4f);
        StartCoroutine(GoUp());

    }
}
