using UnityEngine;
using UnityEngine.UI;

public class ChangeEnableSaleScript : MonoBehaviour
{

    public GameObject salePanel;

    public void OnChange()
    {
        if(this.gameObject.GetComponent<Toggle>().isOn)
        {
            salePanel.SetActive(true);
        }
        else
        {
            salePanel.SetActive(false);
        }
    }
}
