using UnityEngine;

public class KickScript : MonoBehaviour
{
    //Variaveis de composição do vetor do chute
    private Vector2[] CursorPositions = new Vector2[3];
    private Vector2 auxVector;
    private Vector3 kickVector;

    private bool Call1;
    private bool Call2;

    private float _xScreenSize;
    private float _yScreenSize;

    private float _xMaxDeslocation;
    private float _yMaxDeslocation;

    //Variaveis de composição da física do chute
    private float _fowardForce;
    private float _sideForce;

    public GameObject _ball;
    public int _maxForceFoward;
    public int _maxForceSides;

    //Referências externas
    public CameraBehaviourScript _scriptCamera;

    private void Start()
    {
        _xScreenSize = Screen.width;                
        _yScreenSize = Screen.height;               

        _xMaxDeslocation = _xScreenSize * 0.8f;     
        _yMaxDeslocation = _yScreenSize * 0.8f;    

        Call1 = false;
        Call2 = false;

    }

    void Update()
    {
        ChuteComposto();
    }

    public void VetorChute()
    {

        float yVal = CursorPositions[1].y - CursorPositions[0].y;
        float xVal = CursorPositions[2].x - CursorPositions[0].x;

        if (yVal < -_yMaxDeslocation)
        {
            yVal = _yMaxDeslocation;
        }
        else if (yVal >= 0)
        {
            yVal = 0;
            xVal = 0;
        }
        else if (yVal < 0)
        {
            yVal *= -1;
        }

        if (xVal > 0)
        {
            if (xVal > _xMaxDeslocation)
                xVal = _xMaxDeslocation;
        }
        else
        {
            if (xVal < -_xMaxDeslocation)
                xVal = -_xMaxDeslocation;
        }

        _fowardForce = (yVal * _maxForceFoward) / _yMaxDeslocation;
        _sideForce = (xVal * _maxForceSides) / _xMaxDeslocation;

        kickVector = new Vector3(_sideForce, _fowardForce, _fowardForce);

        if ((yVal / _yMaxDeslocation) > 0.25f)
        {

            ExecutarChute();
            LimpaArray();
        }
        else
        {
            LimpaArray();
        }
    }
    public void LimpaArray()
    {
        CursorPositions[0] = Vector2.zero;
        CursorPositions[1] = Vector2.zero;
        CursorPositions[2] = Vector2.zero;

        Call1 = false;
        Call2 = false;
    }

    private void ChuteComposto()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Call1 == false)
            {
                kickVector = Vector3.zero;
                CursorPositions[0] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                auxVector = CursorPositions[0];
                Call1 = true;
            }

            if (Call2 == false && Call1 == true)
            {

                CursorPositions[1] = new Vector2(0, Input.mousePosition.y);

                if (CursorPositions[1].y > auxVector.y)
                {
                    Call2 = true;
                }
                else
                {
                    auxVector = CursorPositions[1];
                }
            }
        }

        if (Call2 == true)
        {

            CursorPositions[2] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (CursorPositions[2].y > CursorPositions[1].y)
                {
                    VetorChute();
                }
                else
                {
                    LimpaArray();
                }
            }
        }
    }

    private void ExecutarChute()
    {
        _scriptCamera._softDeslocation = true;
        _ball.GetComponent<Rigidbody>().AddRelativeForce(kickVector, ForceMode.Force);
    }

}
