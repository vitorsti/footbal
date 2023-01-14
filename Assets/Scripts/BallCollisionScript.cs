using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallCollisionScript : MonoBehaviour
{

    Rigidbody _rb;
    public GameObject _kickBase;
    public List<Transform> spawns;
    public int _lastSpawner;
    public Transform target;
    private bool _ressetar;
    public CameraBehaviourScript _cBS;

    public AudioSource _aS;
    public AudioSource _aS2;
    public AudioClip[] _audios;

    [Header("------ Stats ------")]
    public PlaySessionStats stats;
    public int maxKicks;
    public int kicks;
    public int goals;

    void Start()
    {
        kicks = 0;
        goals = 0;
        _rb = GetComponent<Rigidbody>();
        _lastSpawner = Random.Range(0, 15);
        stats = Resources.Load<PlaySessionStats>("Stats/ByPlayStats");
        transform.position = new Vector3(spawns[_lastSpawner].transform.position.x, spawns[_lastSpawner].transform.position.y + 0.4f, spawns[_lastSpawner].transform.position.z);
        _ressetar = true;
    }

    private void Update()
    {
        transform.LookAt(target);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.name == "Stadium" || coll.gameObject.name == "Football_Camp")
        {
            if (_ressetar == true)
            {
                _aS.clip = _audios[1];
                _aS.Play();
                StartCoroutine(ReturnToKiker());
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Point_Area")
        {
            _aS.clip = _audios[0];
            _aS2.clip = _audios[2];
            _aS.Play();
            _aS2.Play();

            if (_ressetar == true)
            {
                StartCoroutine(PointAndReturn());
            }
        }
    }

    IEnumerator ReturnToKiker()
    {
        kicks++;
        _ressetar = false;
        yield return new WaitForSeconds(1);
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
        transform.position = new Vector3(spawns[_lastSpawner].transform.position.x, spawns[_lastSpawner].transform.position.y + 0.36f, spawns[_lastSpawner].transform.position.z);
        _cBS._ressetThePosition = true;
        _cBS._lookToPost = false;
        _cBS._lookToBall = false;
        transform.rotation = Quaternion.identity;
        _rb.isKinematic = false;
        _ressetar = true;
    }

    IEnumerator PointAndReturn()
    {
        kicks++;
        goals++;
        _ressetar = false;
        yield return new WaitForSeconds(1);
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
        _lastSpawner = Random.Range(0, 15);
        transform.position = new Vector3(spawns[_lastSpawner].transform.position.x, spawns[_lastSpawner].transform.position.y + 0.36f, spawns[_lastSpawner].transform.position.z);
        _cBS._ressetThePosition = true;
        _cBS._lookToPost = false;
        _cBS._lookToBall = false;
        transform.rotation = Quaternion.identity;
        _rb.isKinematic = false;
        _ressetar = true;
    }
}
