using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LegController))]
public class EvolutionScorer : MonoBehaviour
{
    [SerializeField] private float pointsGameOver = -50f;

    [SerializeField] private float pointsForMoving = 0.001f;
    Vector3 oldPosition;

    [SerializeField] private float pointsForBalance = 0.01f;
    private float scoreBalance = 0;
    private Vector3 targetUp = Vector3.up;

    [SerializeField] private float pointsForMovingToTarget = 0.01f;
    private Transform target;
    public LegController controller;

    [SerializeField] private float pointsForRotateToTarget = 0.01f;

    [SerializeField] private float pointsForReachingGoal = 1000f;

    public float Score { get; private set; }
    [SerializeField] private float score = 0f;
    
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

    private void OnDisable()
    {
        /*float angle = Vector3.Angle(transform.up, targetUp);
        Score += pointsForBalance / (0.001f + angle);
        */
    }

    private void FixedUpdate()
    {
        //PointsForMoving();
        PointsForBalance();
        PointsForMovingToTarget();
        PointsForRotateToTarget();
        
        score = Score;
    }

    private void PointsForRotateToTarget()
    {
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        Vector3 direction = Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up);
        float angle = Vector3.Angle(forward, direction);
        angle = Mathf.Abs(angle);
        Score += pointsForRotateToTarget/(1f+angle);
    }

    public void PointsForReachingGoal()
    {
        Score += pointsForReachingGoal;
        Score += scoreBalance;
        scoreBalance = 0;
    }

    private void PointsForMovingToTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        Score += 1/(0.001f + distance )* pointsForMovingToTarget;
    }

    private void PointsForBalance()
    {
        float angle = Vector3.Angle(transform.up, targetUp);
        //Debug.Log(angle);
        if (angle > 45f)
        {
            //GameOver();
            //scoreBalance += ( - angle) * pointsForBalance * Time.fixedDeltaTime;
        }
        //if (angle < 5f)
        {
            scoreBalance +=  pointsForBalance / (1f + angle);
        }
        
    }

    private void GameOver()
    {
        Score += pointsGameOver;
        scoreBalance = 0;
        this.enabled = false;
    }

    private void PointsForMoving()
    {
        float distance = Vector3.Distance(oldPosition, transform.position);
        Score += pointsForMoving*Mathf.Max(0, (distance- 0.015f));
        
        oldPosition = transform.position;
    }

}
