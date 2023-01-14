using UnityEngine;

public class EnableScreenTrailScript : MonoBehaviour
{
    public TrailRenderer _sT;
    public SpriteRenderer _sTR;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _sT.enabled = true;
            _sTR.enabled = true;
            _sT.time = 0.25f;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            _sT.time = 0f;
            _sT.enabled = false;
            _sTR.enabled = false;
        }
    }
}
