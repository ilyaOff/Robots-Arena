using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingForwardChallenge : Challenge
{
    private Vector3 _startPosition = Vector3.zero;

    protected override void Initialize(INeuralNetworkAgent agentTransform, TargetRobots target)
    {
        _startPosition = agentTransform.transform.position;
    }

    public override void Update()
    {
        if (_transform is null)
            return;
        float distance = _transform.position.z - _startPosition.z;
        Score = reward * distance;
    }
}


