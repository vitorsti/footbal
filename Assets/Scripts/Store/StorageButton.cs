using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SecPlayerPrefs;

public class StorageButton : MonoBehaviour
{
    [Header(" ------- ItemData -------")]
    public TextMeshProUGUI objTextName;
    public TextMeshProUGUI description;
    public GameObject icon;
    public GameObject coinIcon;
    GameObject prefab;
    bool hasBought;
    StoreItem data;
    SlotHandler slot;

    [Header(" ------- UI -------")]
    public GameObject popUp;
    public Button confirmBuyButton;
    public Button cancelBuyButton;
    public Image onEquipImage;
    public GameObject parent;

    private bool confirmBuy;
    public bool equip;

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
        data = _item;
        objTextName.text = data.itemName;
        gameObject.name = data.itemName;

        if (data != null)
            icon.GetComponent<Image>().sprite = data.icon;

        if (!SecurePlayerPrefs.HasKey(data.id + "_comprado"))
        {
            SecurePlayerPrefs.SetBool(data.id + "_comprado", false);
        }

        hasBought = SecurePlayerPrefs.GetBool(data.id + "_comprado");
        prefab = data.itemPrefab;
        slot = _slot;

        if (!hasBought)
        {
            description.text = data.price.ToString();
            coinIcon.GetComponent<Image>().sprite = _coinIcon;
        }
        else
        {
            description.text = "Equipar";
            coinIcon.SetActive(false);
        }

        parent = transform.parent.gameObject;

        string nameSideHolder = parent.name;
        string numberSideHolder = nameSideHolder.Substring(nameSideHolder.Length - 2);

        onEquipImage = GameObject.Find("Equip_" + numberSideHolder).GetComponentsInChildren<Image>()[2];

        if (data.id == SecurePlayerPrefs.GetString(data.tipoItem + "_Equip"))
        {
            equip = true;
            EquipItem();
        }
    }

    public void EquipItem()
    {
        slot.slot = prefab;

        for (int i = 0; i < parent.GetComponentsInChildren<StorageButton>().Length; i++)
        {
            StorageButton referenceScript = parent.GetComponentsInChildren<StorageButton>()[i];

            if (referenceScript.equip)
            {
                referenceScript.UnEquipItem();
            }
        }

        equip = true;

        SecurePlayerPrefs.SetString(data.tipoItem + "_Equip", data.id);

        description.text = "Equipado";
        onEquipImage.sprite = icon.GetComponent<Image>().sprite;
        gameObject.GetComponent<Image>().color = new Color(100 / 255f, 180 / 255f, 100 / 255f);
    }

    public void UnEquipItem()
    {
        equip = false;
        description.text = "Equipar";
        gameObject.GetComponent<Image>().color = new Color(50 / 255f, 80 / 255f, 100 / 255f);
    }

    public void OnConfirmBuy()
    {
        hasBought = true;
        SecurePlayerPrefs.SetBool(data.id + "_comprado", true);
        data.hasBought = true;
        coinIcon.SetActive(false);
        EquipItem();
    }

    public void PopUp()
    {
        GameObject parent = transform.root.gameObject;
        popUp.transform.SetParent(parent.transform);
        popUp.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        popUp.SetActive(true);
    }

    public void OnClick()
    {
        if (SecurePlayerPrefs.GetBool(data.id + "_comprado"))
        {
            EquipItem();
        }
        else if(!SecurePlayerPrefs.GetBool(data.id + "_comprado"))
        {
            PopUp();
        }
    }
}
