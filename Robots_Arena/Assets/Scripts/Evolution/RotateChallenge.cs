using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateChallenge : Challenge
{
    public override void Update()
    {
        Vector3 forward = Vector3.ProjectOnPlane(_transform.forward, Vector3.up);
        Vector3 direction = Vector3.ProjectOnPlane(_target.transform.position - _transform.position, Vector3.up);
        
        float angle = Vector3.Angle(forward, direction);
        angle = Mathf.Abs(angle);
        
        Score = reward / (1f + angle);
    }
}
