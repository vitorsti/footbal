using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionScript : MonoBehaviour
{
    public float _transitionSpeed;
    [HideInInspector]
    public bool _isFocusBall;
    private Transform _target;

    void Start()
    {
        
    }

    private void Update()
    {
        if (_isFocusBall == false)
        {
            _target = GameObject.Find("BallTest(Clone)").GetComponent<Transform>();
            _isFocusBall = true;
        }
    }

    void LateUpdate()
    {
        if (_isFocusBall)
            transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _transitionSpeed);
    }
}
