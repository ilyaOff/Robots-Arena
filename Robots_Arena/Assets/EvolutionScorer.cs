using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LegController))]
public class EvolutionScorer : MonoBehaviour
{
    [SerializeField] private float pointsGameOver = -100f;

    [SerializeField] private float pointsForTime = -0.001f;

    [SerializeField] private float pointsForBalance = -0.002f;
    private Vector3 targetUp = Vector3.up;

    [SerializeField] private float pointsForMovingToTarget = 0.1f;
    private float oldDistance;
    private Transform target;
    public LegController controller;

    [SerializeField] private float pointsForReachingGoal = 10f;

    public float Score { get; private set; }
    
    public void ChangeTarget(TargetRobots newTarget)
    {
        target = newTarget.transform;
        controller.TryChangeTarget(target);
        oldDistance = Vector3.Distance(target.position, transform.position);
    }

    private void Awake()
    {
        Score = 0;
        controller = this.GetComponent<LegController>();
    }
    private void OnEnable()
    {
        Score = 0;
        //Debug.LogError("0 score");
    }

    private void FixedUpdate()
    {        
        PointsForTime();
        PointsForBalance();
        PointsForMovingToTarget();
    }

    public void PointsForReachingGoal()
    {
        Score += pointsForReachingGoal;
    }
    private void PointsForMovingToTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        Score += (oldDistance - distance) * pointsForMovingToTarget;
    }

    private void PointsForBalance()
    {
        float angle = Vector3.Angle(transform.up, targetUp);
        //Debug.Log(angle);
        if (angle > 45f)
        {
            GameOver();
        }
        Score += angle * pointsForBalance;
    }

    private void GameOver()
    {
        Score += pointsGameOver;
        this.enabled = false;
    }

    private void PointsForTime()
    {
        Score += pointsForTime;        
    }

}
