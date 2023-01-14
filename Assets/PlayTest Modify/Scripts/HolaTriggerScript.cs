using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolaTriggerScript : MonoBehaviour
{

    public Animator _thisAnim;
    private bool trigger;
    private GameObject _torcedor;

    public List<Material> skinColor;
    public List<Material> clotheColor;
    public List<Material> jeansColor;
    public List<Material> hairColor;

    private bool setuped;

    private void Start()
    {
        SetUpColors();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartAnimator();
        }
    }

    public void StartAnimator()
    {
        StartCoroutine(DisableAnimator());
    }

    public IEnumerator DisableAnimator()
    {
        _thisAnim.enabled = true;
        yield return new WaitForSeconds(8.25f);
        _thisAnim.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Torcedor")
        {
            _torcedor = other.gameObject;

            if (setuped == true)
            {
                _torcedor.GetComponent<Animator>().speed = 1;
                _torcedor.GetComponent<Animator>().SetTrigger("Hola");
                StartCoroutine(SetNewIdle());
            }
            else
            {
                SkinnedMeshRenderer renderer = _torcedor.GetComponentInChildren<SkinnedMeshRenderer>();

                Material[] mats = renderer.materials;

                mats[0] = skinColor[Random.Range(0, 5)];
                mats[1] = clotheColor[Random.Range(0, 9)];
                mats[2] = jeansColor[Random.Range(0, 3)];
                mats[3] = hairColor[Random.Range(0, 3)];
                renderer.materials = mats;

                StartCoroutine(SetNewIdle());
            }
        }

    }

    public IEnumerator SetNewIdle()
    {
        GameObject _thisTorcedor = _torcedor;
        Debug.Log("Pegou o torcedor " + _thisTorcedor.name);

        float random = Random.Range(0, 10000);

        yield return new WaitForSeconds(1.1f);

        if (random <= 3333)
        {
            _thisTorcedor.GetComponent<Animator>().SetTrigger("Idle");
        }
        else if (random <= 6666)
        {
            _thisTorcedor.GetComponent<Animator>().SetTrigger("Act1");
            _thisTorcedor.GetComponent<Animator>().speed = Random.Range(0.5f, 1.5f);
        }
        else if (random <= 9999)
        {
            _thisTorcedor.GetComponent<Animator>().SetTrigger("Act2");
            _thisTorcedor.GetComponent<Animator>().speed = Random.Range(0.5f, 1.5f);
        }
    }

    public void SetUpColors()
    {
        setuped = false;
        StartCoroutine(SettingUp());
    }

    public IEnumerator SettingUp()
    {
        _thisAnim.enabled = true;
        yield return new WaitForSeconds(8.25f);
        _thisAnim.enabled = false;
        setuped = true;
    }

}
