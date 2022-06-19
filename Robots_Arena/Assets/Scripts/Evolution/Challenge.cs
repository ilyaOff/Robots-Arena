using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Challenge : MonoBehaviour
{
    [SerializeField] protected float reward = 0.001f;
    public float Score { get; protected set; }

    protected Transform _transform;
    protected TargetRobots _target;

    public void StartChallenge(INeuralNetworkAgent agentTransform, TargetRobots target)
    {
        Initialize(agentTransform, target);
        _transform = agentTransform.transform;
        _target = target;
        
        Restart();
    }

    public void Restart()
    {
        Score = 0;
    }

    protected virtual void Initialize(INeuralNetworkAgent agentTransform, TargetRobots target) { }

    public abstract void Update();

}
