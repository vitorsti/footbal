using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryAgainTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("AutoDestroy", 2f);
    }

    void AutoDestroy()
    {
        Destroy(gameObject);
    }
}
