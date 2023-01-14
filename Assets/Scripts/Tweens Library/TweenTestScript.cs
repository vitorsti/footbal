using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TweenTestScript : MonoBehaviour
{

    private bool inverter;

    private Transform _panel;
    private AudioSource _aSfx;

    void Start()
    {
        inverter = false;   
        _aSfx = GameObject.Find("SFXController").GetComponent<AudioSource>();
        _panel = GetComponent<Transform>();
    }

    public void PressConfigBtn()
    {
        if (!inverter)
        {
            //Abre
            _panel.DOKill(false);
            _panel.DOLocalMoveY(40, 0.25f, true);

            if (_aSfx.mute == false)
            {
                _aSfx.Play();
            }

            inverter = !inverter;

            StartCoroutine(AfterTimeDisableConfig());

        }
        else if (inverter)
        {
            //Fecha
            StopCoroutine(AfterTimeDisableConfig());
            _panel.DOKill(false);

            _panel.DOLocalMoveY(768, 0.25f, true);

            if (_aSfx.mute == false)
            {
                _aSfx.Play();
            }

            inverter = !inverter;

        }
    }

    public IEnumerator AfterTimeDisableConfig()
    {
        //Fecha
        yield return new WaitForSeconds(5f);
        _panel.DOLocalMoveY(768, 0.5f, true);
        inverter = !inverter;
    }
}
