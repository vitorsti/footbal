using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkTestScript : MonoBehaviour
{

    public AnimationTestScript cubeReference;

    //IK
    private void OnAnimatorIK(int layerIndex)
    {
        cubeReference._playerAnmt.SetLookAtWeight(1);

        if(cubeReference._target != null)
            cubeReference._playerAnmt.SetLookAtPosition(cubeReference._target.transform.position);

        if (Input.GetKey(KeyCode.G))
        {
            cubeReference._playerAnmt.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            cubeReference._playerAnmt.SetIKPosition(AvatarIKGoal.LeftHand, cubeReference._target.transform.position);
            cubeReference._playerAnmt.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            cubeReference._playerAnmt.SetIKPosition(AvatarIKGoal.RightHand, cubeReference._target.transform.position);
        }
    }

    //

}
