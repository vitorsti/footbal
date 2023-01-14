using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionsProps : MonoBehaviour
{
    public SpawnPosDistance distance;
    public SpawnPosSide side;

    public enum SpawnPosSide
    {
        Left,
        Right,
        Center
    }

    public enum SpawnPosDistance
    {
        Short,
        SemiShort,
        Medium,
        SemiLong,
        Long
    } 
}
