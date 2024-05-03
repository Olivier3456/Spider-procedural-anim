using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetAnchor : MonoBehaviour
{
    public Transform targetAnchor;

    private void FixedUpdate()
    {
        transform.position = targetAnchor.position;
    }
}
