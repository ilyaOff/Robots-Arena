using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetRobots : MonoBehaviour
{
    private EvolutionScorer robot;
    private EvolutionRoom room;

    //public UnityEvent Succes = new UnityEvent();
    public void SetRoom(EvolutionRoom room)
    {
        this.room = room;
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
            //Succes?.Invoke();
            room.PlaceTarget();
        }
            
    }
}
