using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetInfo : MonoBehaviour
{
    FormationZones zones;

    public int WR_R_qtd;
    public int WR_L_qtd;
    public int TE_qtd;
    public int HB_L_qtd;
    public int HB_C_qtd;
    public int HB_R_qtd;

    // Start is called before the first frame update
    void Start()
    {
        zones = FindObjectOfType<FormationZones>();
        zones.SetPreSetRoles(WR_R_qtd, WR_L_qtd, TE_qtd, HB_L_qtd, HB_C_qtd, HB_R_qtd);
    }
}
