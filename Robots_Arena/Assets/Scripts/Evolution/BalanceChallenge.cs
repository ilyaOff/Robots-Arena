using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceChallenge : Challenge
{
    [SerializeField] private float fine = -0.3f;
    [SerializeField] private float maxAngle = 45f;

    private Vector3 _targetUp = Vector3.up;

    public override void Update()
    {
        float angle = Vector3.Angle(_transform.up, _targetUp);

        if (angle > maxAngle)
        {
            Score += fine;
        }

        Score += reward / (1f + angle);
    }
}
