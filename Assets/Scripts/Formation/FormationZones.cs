using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationZones : MonoBehaviour
{
    public GameObject selectedPlayer;

    [Header("----- Per Region -----")]
    [SerializeField] int WR_L_qtd;
    [SerializeField] int WR_R_qtd;
    [SerializeField] int TE_qtd;
    [SerializeField] int HB_L_qtd;
    [SerializeField] int HB_R_qtd;
    [SerializeField] int HB_C_qtd;

    [Header("----- Limit Per Region -----")]
    [SerializeField] int WR_R_limit;
    [SerializeField] int WR_L_limit;
    [SerializeField] int TE_limit;
    [SerializeField] int HB_L_limit;
    [SerializeField] int HB_R_limit;
    [SerializeField] int HB_C_limit;

    [Header("----- WideReceiver -----")]
    [SerializeField] Vector3 WR_R_center;
    [SerializeField] Vector3 WR_R;

    [SerializeField] Vector3 WR_L_center;
    [SerializeField] Vector3 WR_L;

    [Header("----- TightEnd -----")]
    [SerializeField] Vector3 TE_center;
    [SerializeField] Vector3 TE;

    [Header("----- HalfBack -----")]
    [SerializeField] Vector3 HB_L_center;
    [SerializeField] Vector3 HB_L;

    [SerializeField] Vector3 HB_R_center;
    [SerializeField] Vector3 HB_R;

    [SerializeField] Vector3 HB_C_center;
    [SerializeField] Vector3 HB_C;

    //Adicionado Danilo------------------------
    public Material[] _Translucid_Mat;
    // [0] - Default
    // [1] - Vermelho
    // [2] - Azul
    // [3] - Amarelo
    //-----------------------------------------


#if UNITY_EDITOR
    //Editor Show Zones
    [SerializeField] bool showZones;
#endif

    void UpdateZonePositions()
    {
        WR_R_center = new Vector3(WR_R_center.x, WR_R_center.y, GeneralGameHandler.actualYards - (WR_R.x / 2));
        WR_L_center = new Vector3(WR_L_center.x, WR_L_center.y, GeneralGameHandler.actualYards - (WR_R.x / 2));
        TE_center = new Vector3(TE_center.x, TE_center.y, GeneralGameHandler.actualYards - (WR_R.x / 2) + 3f);
        HB_L_center = new Vector3(HB_L_center.x, HB_L_center.y, GeneralGameHandler.actualYards - (WR_R.x / 2));
        HB_R_center = new Vector3(HB_R_center.x, HB_R_center.y, GeneralGameHandler.actualYards - (WR_R.x / 2));
        HB_C_center = new Vector3(HB_C_center.x, HB_C_center.y, GeneralGameHandler.actualYards - (WR_R.x / 2) - 2.5f);
    }

    void Start()
    {
        UpdateZonePositions();
        //Adicionado Danilo ---------------------------------------
        // ZonesOfGame();
        //--------------------------------------------------------

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;

            //Verifica um jogador já está selecionado

            if (Physics.Raycast(ray, out hitData, 100))
            {
                //Move Dot
                if (selectedPlayer == null &&
                    hitData.transform.gameObject.tag == PlayerRole.Teammate &&
                    (hitData.transform.gameObject.name == PlayerRole.WideReceiver ||
                    hitData.transform.gameObject.name == PlayerRole.TightEnd ||
                    hitData.transform.gameObject.name == PlayerRole.HalfBack ||
                    hitData.transform.gameObject.name == PlayerRole.TeamBank))
                {
                    selectedPlayer = hitData.transform.gameObject;
                    RemoveZoneQtd(selectedPlayer);
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
                if (selectedPlayer != null)
                {
                    selectedPlayer.transform.position = new Vector3(hitData.point.x, selectedPlayer.transform.position.y, hitData.point.z);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Se um PathingDot está seleciona, ele volta a ser null, pois deve estar pressionado para mover o ponto
            if (selectedPlayer != null)
            {
                ReleasePlayer(selectedPlayer);
                selectedPlayer = null;
            }
        }

    #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Y))
        {
            UpdateZonePositions();
        }
    #endif
    }

    public void SetPreSetRoles(int _wr_R, int _wr_L, int _te, int _hb_L, int _hb_C, int _hb_R)
    {
        WR_L_qtd = _wr_L;
        WR_R_qtd = _wr_R;
        TE_qtd = _te;    
        HB_L_qtd = _hb_L;
        HB_R_qtd = _hb_R;
        HB_C_qtd = _hb_C;
    }

    void VerifyBankPlayer()
    {
        GameObject[] bank = GameObject.FindGameObjectsWithTag(PlayerRole.Teammate);

        if(bank.Length > 0)
        {
            foreach(GameObject player in bank)
            {
                if (player.name == PlayerRole.TeamBank)
                    Debug.Log("Corrija a formação");
            }
        }
    }

    void RemoveScriots(GameObject _player)
    {
        var te = _player.GetComponent<TightEndPathing>();
        var hb = _player.GetComponent<HalfBackPathing>();
        var wr = _player.GetComponent<WideReceiverPathing>();

        if (te != null)
            Destroy(te);
        if (hb != null)
            Destroy(hb);
        if (wr != null)
            Destroy(wr);

        _player.GetComponent<PlayerPathingInfo>().ErasePathing();
    }

    void RemoveZoneQtd(GameObject _player)
    {
        RemoveScriots(_player);

        switch (_player.name)
        {
            case PlayerRole.WideReceiver:

                if ((_player.transform.position.x > (WR_R_center.x - WR_R.x / 2) && _player.transform.position.x < (WR_R_center.x + WR_R.x / 2)) &&
                    (_player.transform.position.z > (WR_R_center.z - WR_R.z / 2) && _player.transform.position.z < (WR_R_center.z + WR_R.z / 2)))
                {
                    WR_L_qtd--;
                }

                if ((_player.transform.position.x > (WR_L_center.x - WR_L.x / 2) && _player.transform.position.x < (WR_L_center.x + WR_L.x / 2)) &&
                    (_player.transform.position.z > (WR_L_center.z - WR_L.z / 2) && _player.transform.position.z < (WR_L_center.z + WR_L.z / 2)))
                {
                    WR_R_qtd--;
                }

                break;

            case PlayerRole.TightEnd:
                TE_qtd--;
                break;

            case PlayerRole.HalfBack:

                if ((_player.transform.position.x > (HB_L_center.x - HB_L.x / 2) && _player.transform.position.x < (HB_L_center.x + HB_L.x / 2)) &&
                    (_player.transform.position.z > (HB_L_center.z - HB_L.z / 2) && _player.transform.position.z < (HB_L_center.z + HB_L.z / 2)))
                {
                    HB_L_qtd--;
                }

                if ((_player.transform.position.x > (HB_R_center.x - HB_R.x / 2) && _player.transform.position.x < (HB_R_center.x + HB_R.x / 2)) &&
                    (_player.transform.position.z > (HB_R_center.z - HB_R.z / 2) && _player.transform.position.z < (HB_R_center.z + HB_R.z / 2)))
                {
                    HB_R_qtd--;
                }

                if ((_player.transform.position.x > (HB_C_center.x - HB_C.x / 2) && _player.transform.position.x < (HB_C_center.x + HB_C.x / 2)) &&
                    (_player.transform.position.z > (HB_C_center.z - HB_C.z / 2) && _player.transform.position.z < (HB_C_center.z + HB_C.z / 2)))
                {
                    HB_C_qtd--;
                }

                break;
        }
    }

    void ReleasePlayer(GameObject _player)
    {
        //Wide Receiver Zone  L/R
        if ((_player.transform.position.x > (WR_R_center.x - WR_R.x / 2) && _player.transform.position.x < (WR_R_center.x + WR_R.x / 2)) &&
            (_player.transform.position.z > (WR_R_center.z - WR_R.z / 2) && _player.transform.position.z < (WR_R_center.z + WR_R.z / 2)) &&
            WR_L_qtd < WR_R_limit)
        {
            _player.gameObject.name = PlayerRole.WideReceiver;
            _player.gameObject.GetComponent<PlayerPathingInfo>().ChangePosition(_player.gameObject.name);
            _player.AddComponent<WideReceiverPathing>();
            WR_L_qtd++;
            return;
        }

        if ((_player.transform.position.x > (WR_L_center.x - WR_L.x / 2) && _player.transform.position.x < (WR_L_center.x + WR_L.x / 2)) &&
            (_player.transform.position.z > (WR_L_center.z - WR_L.z / 2) && _player.transform.position.z < (WR_L_center.z + WR_L.z / 2)) &&
            WR_R_qtd < WR_L_limit)
        {
            _player.gameObject.name = PlayerRole.WideReceiver;
            _player.gameObject.GetComponent<PlayerPathingInfo>().ChangePosition(_player.gameObject.name);
            _player.AddComponent<WideReceiverPathing>();
            WR_R_qtd++;
            return;
        }

        //TE Zone
        if ((_player.transform.position.x > (TE_center.x - TE.x / 2) && _player.transform.position.x < (TE_center.x + TE.x / 2)) &&
            (_player.transform.position.z > (TE_center.z - TE.z / 2) && _player.transform.position.z < (TE_center.z + TE.z / 2)) &&
            TE_qtd < TE_limit)
        {
            _player.gameObject.name = PlayerRole.TightEnd;
            _player.gameObject.GetComponent<PlayerPathingInfo>().ChangePosition(_player.gameObject.name);
            _player.AddComponent<TightEndPathing>();
            TE_qtd++;
            return;
        }

        //HB Zone
        if ((_player.transform.position.x > (HB_L_center.x - HB_L.x / 2) && _player.transform.position.x < (HB_L_center.x + HB_L.x / 2)) &&
            (_player.transform.position.z > (HB_L_center.z - HB_L.z / 2) && _player.transform.position.z < (HB_L_center.z + HB_L.z / 2)) &&
            HB_L_qtd < HB_L_limit)
        {
            _player.gameObject.name = PlayerRole.HalfBack;
            _player.gameObject.GetComponent<PlayerPathingInfo>().ChangePosition(_player.gameObject.name);
            _player.AddComponent<HalfBackPathing>();
            HB_L_qtd++;
            return;
        }

        if ((_player.transform.position.x > (HB_R_center.x - HB_R.x / 2) && _player.transform.position.x < (HB_R_center.x + HB_R.x / 2)) &&
            (_player.transform.position.z > (HB_R_center.z - HB_R.z / 2) && _player.transform.position.z < (HB_R_center.z + HB_R.z / 2)) &&
            HB_R_qtd < HB_R_limit)
        {
            _player.gameObject.name = PlayerRole.HalfBack;
            _player.gameObject.GetComponent<PlayerPathingInfo>().ChangePosition(_player.gameObject.name);
            _player.AddComponent<HalfBackPathing>();
            HB_R_qtd++;
            return;
        }

        if ((_player.transform.position.x > (HB_C_center.x - HB_C.x / 2) && _player.transform.position.x < (HB_C_center.x + HB_C.x / 2)) &&
            (_player.transform.position.z > (HB_C_center.z - HB_C.z / 2) && _player.transform.position.z < (HB_C_center.z + HB_C.z / 2)) &&
            HB_C_qtd < HB_C_limit)
        {
            _player.gameObject.name = PlayerRole.HalfBack;
            _player.gameObject.GetComponent<PlayerPathingInfo>().ChangePosition(_player.gameObject.name);
            _player.AddComponent<HalfBackPathing>();
            HB_C_qtd++;
            return;
        }

        selectedPlayer.name = PlayerRole.TeamBank;
    }

    private void OnDisable()
    {
        int childs = transform.childCount;

        if (childs > 0)
            for (int i = childs - 1; i >= 0; i--)
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
    }

    private void OnEnable()
    {
        UpdateZonePositions();
        ZonesOfGame();
    }

    //Adicionado Danilo --------------------------------------------
    private void ZonesOfGame()
    {
        NewZone("WR_R_Zone", WR_R.x, WR_R.z, 0.1f, WR_R_center, _Translucid_Mat[1]);
        NewZone("WR_L_Zone", WR_L.x, WR_L.z, 0.1f, WR_L_center, _Translucid_Mat[1]);
        NewZone("TE_Zone", TE.x, TE.z, 0.1f, TE_center, _Translucid_Mat[2]);
        NewZone("HB_L_Zone", HB_L.x, HB_L.z, 0.1f, HB_L_center, _Translucid_Mat[3]);
        NewZone("HB_R_Zone", HB_R.x, HB_R.z, 0.1f, HB_R_center, _Translucid_Mat[3]);
        NewZone("HB_C_Zone", HB_C.x, HB_C.z, 0.1f, HB_C_center, _Translucid_Mat[3]);
    }

    private void NewZone(string zoneName, float height, float width, float distanceFromField, Vector3 centralPoint, Material matType)
    {
        GameObject newQuad = new GameObject();
        newQuad.name = zoneName;
        newQuad.AddComponent<QuadCreator>().CreateColoredPlane(height, width, matType);
        newQuad.transform.position = new Vector3(centralPoint.x - (height / 2), distanceFromField, centralPoint.z - (width / 2));
        newQuad.transform.parent = transform;
    }
    //---------------------------------------------------------------
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showZones)
        {
            //WR
            Gizmos.color = Color.red;
            Gizmos.DrawCube(WR_R_center, new Vector3(WR_R.x, WR_R.y, WR_R.z));

            Gizmos.color = Color.red;
            Gizmos.DrawCube(WR_L_center, new Vector3(WR_L.x, WR_L.y, WR_L.z));

            //TE
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(TE_center, new Vector3(TE.x, TE.y, TE.z));

            //HB
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(HB_L_center, new Vector3(HB_L.x, HB_L.y, HB_L.z));

            //Gizmos.color = Color.blue;
            Gizmos.DrawCube(HB_R_center, new Vector3(HB_R.x, HB_R.y, HB_R.z));

            //Gizmos.color = Color.blue;
            Gizmos.DrawCube(HB_C_center, new Vector3(HB_C.x, HB_C.y, HB_C.z));
        }
    }
#endif
}
