using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCanvasScript : MonoBehaviour
{

    public GameObject _mainCanvas;
    public GameObject _button1;
    public GameObject _button2;

 public void Destory()
    {
        //Destroy(this.gameObject);
        StartCoroutine(DelayButton());
        this.gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void CallTransition()
    {
        _mainCanvas.SetActive(true);
        _button1.SetActive(false);
        _button2.SetActive(false);
        _mainCanvas.GetComponent<Animator>().SetTrigger("Transition");
    }

    public IEnumerator DelayButton()
    {
        yield return new WaitForSeconds(1f);
        _button1.SetActive(true);
        _button2.SetActive(true);
        Destroy(this.gameObject);
    }


}
