using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReachingTargetChallange : Challenge
{
    public override void Update() { }

    protected override void Initialize(INeuralNetworkAgent agentTransform, TargetRobots target)
    {
        if (_target != null)
        {
            _target.Succes.RemoveListener(ReachingTarget);
        }
        
        target.Succes.AddListener(ReachingTarget);
    }

    public void ReachingTarget()
    {
        Score += reward;
    }
}
