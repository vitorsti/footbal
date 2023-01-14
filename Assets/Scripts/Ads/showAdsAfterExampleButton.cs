using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class showAdsAfterExampleButton : MonoBehaviour
{
    NonRewardAds nrad;
    TextMeshProUGUI txt;
    Button button;

    private void Awake()
    {
        nrad = FindObjectOfType<NonRewardAds>();
        txt = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();

    }

    // Start is called before the first frame update
    void Start()
    {
        txt.text = "An ad will show after: " + nrad.pCounter + " clicks";
        button.onClick.AddListener(nrad.ShowAdsAfter);
        button.onClick.AddListener(SetText);

    }

    void SetText(){
        txt.text = "An ad will show after: " + nrad.pCounter + " clicks";
    }

    // Update is called once per frame

}
