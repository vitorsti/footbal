using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTestScript : MonoBehaviour
{

    public GameObject _mainCanvas;
    public GameObject _joyStickCanvas;
    public GameObject _button1;
    public GameObject _button2;
    public GameObject _mainCamera;
    public GameObject _dummy;
    public Animator _animCanvas;
    public CameraTransitionScript cTS;
    public PassingScript pS;

    private void Start()
    {
        // cTS.enabled = false;
        pS.enabled = false;
    }

    public void OnPressPassing()
    {
        _button1.SetActive(false);
        _button2.SetActive(false);

        StartCoroutine(FadeCanvasPassing());
    }

    public void OnPressRunning()
    {
        _button1.SetActive(false);
        _button2.SetActive(false);

        StartCoroutine(FadeCanvasRunning());
    }

    public IEnumerator FadeCanvasPassing()
    {
        _animCanvas.SetTrigger("Transition");
        yield return new WaitForSeconds(1.1f);
        _mainCamera.transform.position = new Vector3(_dummy.transform.position.x, _dummy.transform.position.y + 5, _dummy.transform.position.z - 10);
        _mainCamera.transform.rotation = Quaternion.identity;
        _mainCamera.transform.localRotation = Quaternion.Euler(20, 0, 0);
        yield return new WaitForSeconds(1f);
        cTS.enabled = true;
        pS.enabled = true;
        _mainCanvas.SetActive(false);
    }

    public IEnumerator FadeCanvasRunning()
    {
        _animCanvas.SetTrigger("Transition");
        yield return new WaitForSeconds(1.1f);
        _mainCamera.transform.position = new Vector3(_dummy.transform.position.x, _dummy.transform.position.y + 5, _dummy.transform.position.z - 10);
        _mainCamera.transform.rotation = Quaternion.identity;
        _mainCamera.transform.localRotation = Quaternion.Euler(20, 0, 0);
        yield return new WaitForSeconds(1f);
        _mainCanvas.SetActive(false);
        _joyStickCanvas.SetActive(true);
    }
}
