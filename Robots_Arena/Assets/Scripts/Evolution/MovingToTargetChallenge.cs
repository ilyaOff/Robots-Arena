using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToTargetChallenge : Challenge
{
    public override void Update()
    {
        float distance = Vector3.Distance(_transform.position, _target.transform.position);
        Score = reward / (0.001f + distance);
    }

}
