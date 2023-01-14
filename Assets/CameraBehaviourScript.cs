using UnityEngine;

public class CameraBehaviourScript : MonoBehaviour
{
    //Gatilhos para algumas situações
    public bool _lookToPost;
    public bool _lookToBall;
    public bool _ressetThePosition;
    public bool _softDeslocation;

    //Fica mais facil gravar as posições como GameObject.Transform, já que eles vão sempre estar em movimento constante de translação e rotação.
    //Assim fica facil refêrenciar a rotação da câmera, sem se preocupar dos quaternios dela e da bola estarem sempre alinhados;
    public Transform _initialCamreaPosition;
    public Transform _finalCameraPosition;
    public Transform _post;
    public Transform _ball;

    private Vector3 _initialScale;

    private float speedReturn;
    public float speedSoftTransition;
    public float speedSoftRotation;

    private void Start()
    {
        _initialScale = transform.localScale;

        //_lookToPost = false;
        _lookToBall = false;
        _ressetThePosition = true;
        _softDeslocation = false;

        speedReturn = 1000f;
    }

    private void Update()
    {
        if (_lookToBall)
        {
            Quaternion actualRot = transform.rotation;
            transform.LookAt(_ball.transform);
            Quaternion finalRot = transform.rotation;
            transform.rotation = actualRot;
            transform.rotation = Quaternion.Lerp(actualRot, finalRot, speedSoftRotation * Time.deltaTime);
        }

       /* if (_lookToPost)
        {
            Quaternion actualRot = transform.rotation;
            transform.LookAt(_post.transform);
            Quaternion finalRot = transform.rotation;
            transform.rotation = actualRot;
            transform.rotation = Quaternion.Lerp(actualRot, finalRot, speedSoftRotation * Time.deltaTime);
        }
        */

        if (_ressetThePosition)
        {
            float step = speedReturn * Time.deltaTime;
            Vector3 targetPosition = _initialCamreaPosition.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                transform.rotation = _initialCamreaPosition.rotation;
                transform.localScale = _initialScale;
                //_lookToPost = false;
                _lookToBall = true;
                _softDeslocation = false;
                _ressetThePosition = false;
            }
        }
        else if (_softDeslocation)
        {
            float step = speedSoftTransition * Time.deltaTime;
            Vector3 targetPosition = _finalCameraPosition.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
    }

}
