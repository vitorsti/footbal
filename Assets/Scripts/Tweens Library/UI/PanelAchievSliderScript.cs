using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelAchievSliderScript : MonoBehaviour
{
    public GameObject _panel;
    private void Start()
    {
        this.transform.localPosition = new Vector2(Screen.width, 0);
    }

    public void GoToCenter()
    {
        transform.DOLocalMoveX(0, 0.5f, true);
    }

    public void BackToSide()
    {
        transform.DOLocalMoveX(Screen.width, 0.5f, true);
    }
}
