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
    private float scoreBalance = 0;
    private Vector3 targetUp = Vector3.up;

    [SerializeField] private float pointsForMovingToTarget = 0.1f;
    private Transform target;
    public LegController controller;

    [SerializeField] private float pointsForRotateToTarget = 0.001f;

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
        scoreBalance = 0;
        oldPosition = transform.position;
        //Debug.LogError("0 score");
    }

    private void FixedUpdate()
    {
        //PointsForMoving();
        PointsForBalance();
        PointsForMovingToTarget();
        PointsForRotateToTarget();
    }

    private void PointsForRotateToTarget()
    {
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        Vector3 direction = Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up);
        float angle = Vector3.Angle(forward, direction);
        Score += pointsForRotateToTarget* Mathf.Max(0, (10f - angle) );
    }

    public void PointsForReachingGoal()
    {
        Score += pointsForReachingGoal;
        Score += scoreBalance;
        scoreBalance = 0;
        PointsForBalance();
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
            GameOver();
            //scoreBalance += ( - angle) * pointsForBalance * Time.fixedDeltaTime;
        }
        if (angle < 5f)
        {
            scoreBalance += 2 * pointsForBalance * Time.fixedDeltaTime;
        }
        else if (angle < 45f)
        {
            scoreBalance +=  pointsForBalance * Time.fixedDeltaTime;
        }
        
    }

    private void GameOver()
    {
        Score += pointsGameOver;
        this.enabled = false;
    }

    private void PointsForMoving()
    {
        float distance = Vector3.Distance(oldPosition, transform.position);
        Score += pointsForMoving*Mathf.Max(0, (distance- 0.015f));
        
        oldPosition = transform.position;
    }

}
