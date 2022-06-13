using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetRobots : MonoBehaviour
{
    private EvolutionarySelection selector;
    private EvolutionScorer robot;

    public void SetSelector(EvolutionarySelection selector)
    {
        this.selector = selector;
    }

    public void SetRobot(EvolutionScorer robot)
    {
        this.robot = robot;
    }

    private void OnTriggerEnter(Collider other)
    {
        EvolutionScorer scorer = other.GetComponentInParent<EvolutionScorer>();
        if (scorer is null)
            return;

        if (scorer == robot)
        {
            scorer.PointsForReachingGoal();
            enabled = false;
            //Debug.Log("I'm sleep");
            selector.PlaceTarget(this, scorer);
        }
            
    }
}
