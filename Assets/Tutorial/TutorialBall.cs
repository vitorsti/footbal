using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBall : MonoBehaviour
{
    public GameObject tryagain;
    GameObject manager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Receiver")
        {
            Destroy(collision.gameObject);
            manager.GetComponent<TutorialManager>().dummies--;

            if (manager.GetComponent<TutorialManager>().dummies == 0) 
            {
                manager.GetComponent<TutorialManager>().onPlay = false;
            }

            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Field")
        {
            if(manager.GetComponent<TutorialManager>().steps == 7)
                Instantiate(tryagain);

            Destroy(gameObject);
        }
    }

    private void Start()
    {
        manager = GameObject.Find("TutorialManager");
    }
}
