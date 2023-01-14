using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FormationPathingHandler : MonoBehaviour
{
    [Header("----- Selected -----")]
    public GameObject selectedPlayer;
    public GameObject selectedDot;

    [Header("----- ETC -----")]
    public GameObject dotPrefab;
    bool lastPointExists;

    Vector3 lastDotPosition;
    Vector3 mousepos;

    //Adicionados Danilo ----------------------------------
    public Button ClearAllBtn;
    public Button ClearLastBtn;
    public PlayerPathingInfo actualSelected;

    public Material _matDefault;
    public Material _matOutline;

    private Grid grid;
    //------------------------------------------------------

    private void Awake()
    {
        dotPrefab = Resources.Load<GameObject>("Utilities/PathingDot");

        //Adicionado Danilo-----------------------------
        grid = FindObjectOfType<Grid>();
        //----------------------------------------------
    }

    void Start()
    {
        lastPointExists = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;


            ////Adicionado Danilo------------------------------------------------
            //PointerEventData cursor = new PointerEventData(EventSystem.current);// This section prepares a list for all objects hit with the raycast
            //cursor.position = Input.mousePosition;
            //List<RaycastResult> objectsHit = new List<RaycastResult>();
            //EventSystem.current.RaycastAll(cursor, objectsHit);
            //int count = objectsHit.Count;
            //--------------------------------------------------------------------

            //Verifica um jogador já está selecionado
            if (selectedPlayer != null)
            {
                if (Physics.Raycast(ray, out hitData, 100))
                {
                    //Move Dot
                    if (selectedDot == null && hitData.transform.gameObject.name == "PathingDot")
                    {
                        selectedDot = hitData.transform.gameObject;
                    }
                    //Adiciona um ponto ao pathing
                    else if (selectedPlayer.GetComponent<PlayerPathingInfo>().ReturnPathingLenght() <= selectedPlayer.GetComponent<PlayerPathingInfo>().maxPathingPoints && hitData.transform.gameObject.name == "Field")
                    {
                        selectedDot = MakeADot(hitData.point);
                    }
                    //Se clicar no mesmo jogador já selecionado, desseleciona ele
                    else if (selectedPlayer == hitData.transform.gameObject)
                    {
                        selectedPlayer.GetComponent<PlayerPathingInfo>().DisableDots();

                        //Adicionado Danilo ----------------------------
                        selectedPlayer.GetComponent<MeshRenderer>().material = _matDefault;
                        //----------------------------------------------

                        selectedPlayer = null;

                        //Adicionado Danilo ----------------------------
                        //ClearAllBtn.onClick.RemoveListener(CustomButton_onClickToClearAll); //subscribe to the onClick event
                        //ClearLastBtn.onClick.RemoveListener(CustomButton_onClickToClearLast); //subscribe to the onClick event
                        //----------------------------------------------
                    }
                    //Se clicar em outro jogador que não o mesmo, seleciona o próximo e desseleciona atual
                    else if (Physics.Raycast(ray, out hitData, 100) && hitData.transform.tag == PlayerRole.Teammate && selectedPlayer != hitData.transform.gameObject)
                    {

                        //Adicionado Danilo ------------------------------------------
                        //ClearAllBtn.onClick.RemoveListener(CustomButton_onClickToClearAll); //subscribe to the onClick event
                        //ClearLastBtn.onClick.RemoveListener(CustomButton_onClickToClearLast); //subscribe to the onClick event
                        selectedPlayer.GetComponent<MeshRenderer>().material = _matDefault;
                        //------------------------------------------------------------------

                        selectedPlayer.GetComponent<PlayerPathingInfo>().DisableDots();
                        selectedPlayer = hitData.transform.gameObject;
                        selectedPlayer.GetComponent<PlayerPathingInfo>().EnableDots();

                        //Adicionado Danilo ------------------------------------------
                        selectedPlayer.GetComponent<MeshRenderer>().material = _matOutline;
                        //actualSelected = selectedPlayer.GetComponent<PlayerPathingInfo>();
                        //ClearAllBtn.onClick.AddListener(CustomButton_onClickToClearAll); //subscribe to the onClick event
                        //ClearLastBtn.onClick.AddListener(CustomButton_onClickToClearLast); //subscribe to the onClick event
                        //------------------------------------------------------------------

                    }
                    //Desseleciona atual
                    else
                    {
                        selectedPlayer.GetComponent<PlayerPathingInfo>().DisableDots();
                        // Adicionado Danilo ------------------------------------------
                        selectedPlayer.GetComponent<MeshRenderer>().material = _matDefault;
                        //------------------------------------------------------------------
                        selectedPlayer = null;
                        //Adicionado Danilo -------------------------------------------------
                        //actualSelected = null;
                        //ClearAllBtn.onClick.RemoveListener(CustomButton_onClickToClearAll); //subscribe to the onClick event
                        //ClearLastBtn.onClick.RemoveListener(CustomButton_onClickToClearLast); //subscribe to the onClick event
                        //-----------------------------------------------------------------
                    }
                }
            }
            else
            {
                //Seleciona jogador
                if (Physics.Raycast(ray, out hitData, 100) && hitData.transform.tag == PlayerRole.Teammate)
                {
                    selectedPlayer = hitData.transform.gameObject;
                    selectedPlayer.GetComponent<PlayerPathingInfo>().EnableDots();

                    //Adicionado Danilo -----------------------------------------------
                    //actualSelected = selectedPlayer.GetComponent<PlayerPathingInfo>();
                    selectedPlayer.GetComponent<MeshRenderer>().material = _matOutline;
                    //ClearAllBtn.onClick.AddListener(CustomButton_onClickToClearAll); //subscribe to the onClick event
                    //ClearLastBtn.onClick.AddListener(CustomButton_onClickToClearLast); //subscribe to the onClick event
                    //------------------------------------------------------------------

                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;

            //Move DotSelecionado
            if (Physics.Raycast(ray, out hitData, 100))
            {
                //mousepos = hitData.point;
                if (hitData.transform.gameObject.name == "PathingDot")
                {
                    selectedDot.transform.position = new Vector3(hitData.point.x, selectedDot.transform.position.y, hitData.point.z);
                    selectedPlayer.GetComponent<PlayerPathingInfo>().DrawPathing();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Se um PathingDot está seleciona, ele volta a ser null, pois deve estar pressionado para mover o ponto
            if (selectedDot != null)
                selectedDot = null;
        }

        if (Input.GetKeyDown(KeyCode.T))
            DisableHandler();
    }

    public void DisableHandler()
    {
        selectedDot = null;
        selectedPlayer = null;
        gameObject.GetComponent<FormationPathingHandler>().enabled = false;
    }

    GameObject MakeADot(Vector3 newDotPosition)
    {
        //Layer Pathings : 8
        GameObject dot = Instantiate(dotPrefab, new Vector3(newDotPosition.x, 0.66f, newDotPosition.z), Quaternion.identity); //use random identity to make dots looks more different
        selectedPlayer.GetComponent<PlayerPathingInfo>().AddNewPoint(dot);
        dot.name = dotPrefab.name;
        return dot;

        #region Criar collider entre pontos
        //dot.transform.parent = transform;
        //dot.layer = 8;

        //if (dotPositions.Count > 1)
        //{
        //    GameObject colliderKeeper = new GameObject("collider");
        //    colliderKeeper.transform.parent = transform;
        //    BoxCollider bc = colliderKeeper.AddComponent<BoxCollider>();
        //    colliderKeeper.layer = 8;
        //    colliderKeeper.transform.position = Vector3.Lerp(newDotPosition, lastDotPosition, 0.5f);
        //    colliderKeeper.transform.LookAt(newDotPosition);
        //    bc.size = new Vector3(0.1f, 0.1f, Vector3.Distance(newDotPosition, lastDotPosition));
        //}

        //lastDotPosition = newDotPosition;
        //lastPointExists = true;
        #endregion
    }


    // Adicionado Danilo -------------------------------------------------
    public void CustomButton_onClickToClearAll()
    {
        actualSelected.ErasePathing();
    }

    public void CustomButton_onClickToClearLast()
    {
        actualSelected.EraseLastPointOfPathing();
    }
    //-------------------------------------------------------------------------
}
