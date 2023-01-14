using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StorageController : MonoBehaviour
{
    [Header(" ------- Texto para equipar item -------")]
    public string equipText;

    [Header(" ------- Icone da moeda da Loja -------")]
    public Sprite coinIcon;

    [Header(" ------- Botão de compra -------")]
    public GameObject buttonPrefab;

    [Header(" ------- Lista dos itens dessa aba -------")]
    public StoreItem[] dataList;
    int storeQtd;

    [Header(" ------- Handler  -------")]
    public SlotHandler slot;

    public GameObject panelEquip;
    public GameObject panelStore;

    private void OnEnable()
    {
        if (storeQtd != dataList.Length)
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            storeQtd = 0;

            foreach (StoreItem item in dataList)
            {
                GameObject newItem = Instantiate(buttonPrefab, this.transform) as GameObject;
                newItem.GetComponent<StorageButton>().SetUpItem(item, equipText, coinIcon, slot);

                storeQtd++;
            }

            RectTransform rt = panelStore.GetComponent<RectTransform>();
            float panelStoreSize = Mathf.Round(storeQtd / 2) * 484;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, panelStoreSize);

            StartCoroutine(WaitAllConfigurations());
            
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }

        if (Input.GetKeyDown(KeyCode.W))
        {

        }

        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }

    IEnumerator WaitAllConfigurations()
    {
        yield return new WaitForSeconds(0.25f);

        string nameSideHolder = gameObject.name;
        string numberSideHolder = nameSideHolder.Substring(nameSideHolder.Length - 2);

        if (!panelEquip.GetComponent<Toggle>().isOn)
            transform.parent.parent.gameObject.SetActive(false);
    }
}
