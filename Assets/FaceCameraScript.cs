using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraScript : MonoBehaviour
{

    private Transform _mCamera;
    private void Start()
    {
        _mCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    void Update()
    {
        //transform.LookAt(_mCamera.position);

        var lookPos = _mCamera.position - transform.position;
        //lookPos.z = 0;
        lookPos.x = 0;

        if (transform.position.z < _mCamera.position.z)
            GetComponent<SpriteRenderer>().flipY = true;
        else
            GetComponent<SpriteRenderer>().flipY = false;

        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 15);
    }
}
