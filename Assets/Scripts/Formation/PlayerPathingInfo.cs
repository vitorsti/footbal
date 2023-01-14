using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathingInfo : MonoBehaviour
{
    //[Header("----- PreSet -----")]
    //public bool isPreset;
    //[SerializeField] Vector3[] modifiers = new Vector3[] { };

    [Header("----- Pathings -----")]
    public List<GameObject> dotsButtons = new List<GameObject>();
    public int maxPathingPoints;
    LineRenderer linePathing;
    GameObject dotPrefab;

    Material wr_Material;
    Material hb_Material;
    Material te_Material;

    private void Awake()
    {
        wr_Material = Resources.Load<Material>("Materials/Pathings/WideReceiver");
        hb_Material = Resources.Load<Material>("Materials/Pathings/HalfBack");
        te_Material = Resources.Load<Material>("Materials/Pathings/TightEnd");
        dotPrefab = Resources.Load<GameObject>("Utilities/PathingDot");
    }

    void Start()
    {
        linePathing = gameObject.GetComponent<LineRenderer>();
        dotsButtons.Add(gameObject);

        if(dotsButtons.Count != linePathing.positionCount)
        {
            dotsButtons = new List<GameObject>();
            dotsButtons.Add(gameObject);

            for(int i=1; i < linePathing.positionCount; i++)
            {
                GameObject dot = Instantiate(dotPrefab, new Vector3(linePathing.GetPosition(i).x, 0.66f, linePathing.GetPosition(i).z), Quaternion.identity);
                dot.transform.position = new Vector3(dot.transform.position.x, -0.15f, dot.transform.position.z + GeneralGameHandler.actualYards);
                dot.name = dotPrefab.name;
                dotsButtons.Add(dot);
                dot.SetActive(false);
            }

            DrawPathing();
        }
    }

    void Update()
    {


#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            for(int i=0; i <= linePathing.positionCount; i++)
            {
                Debug.Log(linePathing.GetPosition(i));
            }
        //    isPreset = true;
        //}
        //if (isPreset)
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        CalculateDistance();
        //    }
        }
#endif

    }

    //#if UNITY_EDITOR
    //    public void CalculateDistance()
    //    {
    //        modifiers = new Vector3[dotsButtons.Count];
    //        for(int i=0; i < dotsButtons.Count; i++)
    //        {
    //            modifiers[i] = new Vector3(dotsButtons[i].transform.position.x - transform.position.x, transform.position.y, dotsButtons[i].transform.position.z - transform.position.z);
    //        }
    //    }
    //#endif

    public void DrawPathing()
    {
        dotsButtons[0] = gameObject;

        if (dotsButtons.Count > 0)
        {
            linePathing.positionCount = dotsButtons.Count;
            for (int i = 0; i < dotsButtons.Count; i++)
                linePathing.SetPosition(i, dotsButtons[i].transform.position);
        }
    }

    public void ErasePathing()
    {
        dotsButtons = new List<GameObject>();
        for (int i = 0; i < dotsButtons.Count; i++)
            Destroy(dotsButtons[i]);
        dotsButtons.Add(gameObject);
        linePathing.positionCount = 1;
        //for (int i = 1; i < dotsButtons.Count; i++)
        //    dotsButtons.RemoveAt(i);
    }

    public void ChangePosition(string _role)
    {
        gameObject.name = _role;

        switch (_role)
        {
            case PlayerRole.WideReceiver:
                linePathing.material = wr_Material;
                maxPathingPoints = 2;
                break;
            case PlayerRole.HalfBack:
                linePathing.material = hb_Material;
                maxPathingPoints = 3;
                break;
            case PlayerRole.TightEnd:
                linePathing.material = te_Material;
                maxPathingPoints = 1;
                break;
        }
    }

    //Readicionado Danilo -----------------------------------
    public void EraseLastPointOfPathing()
    {
        if (dotsButtons.Count > 1)
        {
            Destroy(dotsButtons[dotsButtons.Count - 1]);
            dotsButtons.RemoveAt(dotsButtons.Count - 1);
            DrawPathing();
        }
        else if (dotsButtons.Count == 1)
        {
            Debug.Log("Não há mais pontos a serem deletados");
            dotsButtons[0] = gameObject;
        }
    }
    //------------------------------------------------------

    public Transform[] GetPathing()
    {
        Transform[] positions = new Transform[dotsButtons.Count];
        for (int i = 0; i < dotsButtons.Count; i++)
        {
            if(i !=0)
                dotsButtons[i].transform.LookAt(dotsButtons[i - 1].transform);

            positions[i] = dotsButtons[i].transform;
        }

        DisableDots();
        return positions;
    }

    public int ReturnPathingLenght()
    {
        return dotsButtons.Count;
    }

    public void AddNewPoint(GameObject _point)
    {
        dotsButtons.Add(_point);
        //_point.transform.parent = transform;

        DrawPathing();

        //int i = dotsButtons.Count - 1;
        //if (Vector3.Distance(dotsButtons[i].transform.position, _point.transform.position) > 3f)
        //{
        //    dotsButtons.Add(_point);
        //    DrawPathing();
        //}
        //else
        //{
        //    Debug.Log("Muito próximo de outro ponto");
        //    Destroy(_point);
        //}
    }

    private void OnDestroy()
    {
        dotsButtons.ForEach(_dot => Destroy(_dot));
    }

    public void EnableDots()
    {
        foreach (GameObject dots in dotsButtons)
            dots.SetActive(true);
    }

    public void DisableDots()
    {
        foreach (GameObject dots in dotsButtons)
            dots.SetActive(false);

        dotsButtons[0].SetActive(true);
    }
}
