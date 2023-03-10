using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTrailScript : MonoBehaviour
{
    void Update()
    {
            Plane objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
             this.transform.position = mRay.GetPoint(rayDistance);
    }
}
