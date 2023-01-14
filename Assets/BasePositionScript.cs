
using UnityEngine;

public class BasePositionScript : MonoBehaviour
{
    public Transform _ball;
    public float speedSoftTransition;

    public void Update()
    {
        Vector3 _desiredDistanceFromBall = new Vector3(_ball.position.x, _ball.position.y + 1f, _ball.position.z - 6.54f);
        float step = speedSoftTransition * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _desiredDistanceFromBall, step);
        Vector3 _floorLimit = new Vector3(transform.position.x, 0, transform.position.z);

        if (Vector3.Distance(transform.position, _floorLimit) < 0.001f)
        transform.position = _floorLimit;
    }
}
