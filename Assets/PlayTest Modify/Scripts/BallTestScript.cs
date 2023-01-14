using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTestScript : MonoBehaviour
{

    private CameraTransitionScript _cTS;

    private void Start()
    {
        _cTS = GameObject.Find("Main Camera").GetComponent<CameraTransitionScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Field")
        {
            StartCoroutine(DestroyBall());
        }

        if (collision.gameObject.name == "Dummy(Runner)")
        {
            _cTS._isFocusBall = false;
            Destroy(gameObject);
            collision.gameObject.GetComponent<RunnerScript>().enabled = true;
        }

    }

    public IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(3f);
        _cTS._isFocusBall = false;
        Destroy(gameObject);
    }

}
