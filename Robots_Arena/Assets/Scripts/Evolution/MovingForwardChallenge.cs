using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingForwardChallenge : Challenge
{
    private Vector3 _startPosition;

    protected override void Initialize(INeuralNetworkAgent agentTransform, TargetRobots target)
    {
        _startPosition = agentTransform.transform.position;
    }

    public override void Update()
    {
        float distance = _transform.position.z - _startPosition.z;
        Score = reward / (1f + distance);
    }
}


