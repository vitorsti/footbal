using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolutionScript : MonoBehaviour
{

    public Text largura;
    public Text altura;

    public Text larguraReal;
    public Text alturaReal;

    void Update()
    {
        largura.text = Screen.width.ToString();
        altura.text = Screen.height.ToString();

        larguraReal.text = Screen.currentResolution.width.ToString();
        alturaReal.text = Screen.currentResolution.height.ToString();
    }

}
