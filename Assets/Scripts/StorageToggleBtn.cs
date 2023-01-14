using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageToggleBtn : MonoBehaviour
{

    [Header(" ------- ItemData -------")]
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public GameObject icon;
    public GameObject coinIcon;
    GameObject prefab;
    bool hasBought;
    StoreItem data;
    SlotHandler slot;
    string equipText;

    [Header(" ------- UI -------")]
    public GameObject popUp;
    public Button confirmBuyButton;

    private bool confirmBuy;
    public bool ConfirmBuy
    {
        get
        {
            return confirmBuy;
        }
        set
        {
            confirmBuy = value;

            OnConfirmBuy();
        }
    }

    private void Start()
    {
        if (confirmBuyButton != null)
            confirmBuyButton.onClick.AddListener(() => ConfirmBuy = !ConfirmBuy);
       
    }

    public void SetUpItem(StoreItem _item, string _equipText, Sprite _coinIcon, SlotHandler _slot)
    {
        name.text = _item.name;
        gameObject.name = _item.name;

        if (_item != null)
            icon.GetComponent<Image>().sprite = _item.icon;

        prefab = _item.itemPrefab;
        hasBought = _item.hasBought;
        prefab = _item.itemPrefab;
        equipText = _equipText;
        slot = _slot;

        if (!hasBought)
        {
            description.text = _item.price.ToString();
            coinIcon.GetComponent<Image>().sprite = _coinIcon;
        }
        else
        {
            description.text = _equipText;
            coinIcon.SetActive(false);
        }

        //GetComponent<Toggle>().group = GetComponentInParent<ToggleGroup>();

        data = _item;
    }

    public void EquipItem()
    {
        slot.slot = prefab;
        Debug.LogWarning("Equipou: " + gameObject.name);
        gameObject.GetComponent<Image>().color = new Color(100 / 255f, 180 / 255f, 100 / 255f);
    }

    public void UnEquipItem()
    {
        gameObject.GetComponent<Image>().color = new Color(50 / 255f, 80 / 255f, 100 / 255f);
    }

    public void OnConfirmBuy()
    {
        hasBought = true;
        data.hasBought = true;
        description.text = equipText;
        gameObject.GetComponent<Image>().color = new Color(50 / 255f, 80 / 255f, 100 / 255f);
        coinIcon.SetActive(false);
    }

    public void PopUp()
    {
        GameObject parent = transform.root.gameObject;
        popUp.transform.SetParent(parent.transform);
        popUp.GetComponent<RectTransform>().position = new Vector2(Screen.width / 2, Screen.height / 2);
        popUp.SetActive(true);
    }

    public void OnClick()
    {
        if (hasBought && GetComponent<Toggle>().isOn == true)
            EquipItem();
        else if (!hasBought && GetComponent<Toggle>().isOn == true)
        {
            PopUp();
            GetComponent<Toggle>().isOn = false;
        }
        else if (GetComponent<Toggle>().isOn == false)
        {
            UnEquipItem();
        }
    }
}