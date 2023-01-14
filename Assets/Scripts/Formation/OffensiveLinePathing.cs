using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffensiveLinePathing : MonoBehaviour
{
    public GameObject target;

    [Header("----- OL  -----")]
    public GameObject Center;
    public OffensiveTackle[] OTs;
    public OffensiveGuard[] OGs;

    [Header("----- General -----")]
    public bool downStarted;

    [Header("----- Holding -----")]
    [SerializeField] float energy;

    void Start()
    {
        Center = GameObject.Find(PlayerRole.Center);
        OTs = FindObjectsOfType<OffensiveTackle>();
        OGs = FindObjectsOfType<OffensiveGuard>();
    }
}
