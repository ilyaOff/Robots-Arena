using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LegController))]
public class EvolutionScorer : MonoBehaviour
{
    [SerializeField] private float pointsGameOver = -500f;

    [SerializeField] private float pointsForMoving = 0.001f;
    Vector3 oldPosition;

    [SerializeField] private float pointsForBalance = 0.01f;
    private Vector3 targetUp = Vector3.up;

    [SerializeField] private float pointsForMovingToTarget = 0.1f;
    private Transform target;
    public LegController controller;

    [SerializeField] private float pointsForReachingGoal = 100f;

    public float Score { get; private set; }
    
    public void ChangeTarget(TargetRobots newTarget)
    {
        target = newTarget.transform;
        controller.TryChangeTarget(target);
    }

    private void Awake()
    {
        Score = 0;
        controller = this.GetComponent<LegController>();
    }
    private void OnEnable()
    {
        Score = 0;
        oldPosition = transform.position;
        //Debug.LogError("0 score");
    }

    private void FixedUpdate()
    {        
        //PointsForMoving();
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
        Score += (3f - distance) * pointsForMovingToTarget;
    }

    private void PointsForBalance()
    {
        float angle = Vector3.Angle(transform.up, targetUp);
        //Debug.Log(angle);
        if (angle > 45f)
        {
            Score += ( - angle) * pointsForBalance * Time.fixedDeltaTime;
        }
        
        Score += (5-angle) * pointsForBalance*Time.fixedDeltaTime;
    }

    private void GameOver()
    {
        Score += pointsGameOver;
        this.enabled = false;
    }

    private void PointsForMoving()
    {
        float distance = Vector3.Distance(oldPosition, transform.position);       
        Score += pointsForMoving*(distance- 0.015f);
        
        oldPosition = transform.position;
    }

}
