using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceChallenge : Challenge
{
    [SerializeField] private float fine = -0.3f;
    [SerializeField] private float maxAngle = 45f;

    private Vector3 _targetUp = Vector3.up;
    private float _scoreBalance = 0;
    public override void Update()
    {
        float angle = Vector3.Angle(_transform.up, _targetUp);

        if (angle > maxAngle)
        {
            Score += fine / (1f + angle);
        }

        Score += reward / (1f + angle);
    }

    protected override void Initialize(INeuralNetworkAgent agentTransform, TargetRobots target)
    {
        if (_target != null)
        {
            _target.Succes.RemoveListener(ReachingTarget);
        }

        target.Succes.AddListener(ReachingTarget);
    }

    private void ReachingTarget()
    {
       // Score += _scoreBalance;
        //_scoreBalance = 0;
    }
}
