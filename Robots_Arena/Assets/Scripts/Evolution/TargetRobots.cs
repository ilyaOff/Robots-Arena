using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetRobots : MonoBehaviour
{
    private INeuralNetworkAgent robot;
    private EvolutionRoom room;
    private bool NotToch = true;

    public UnityEvent Succes = new UnityEvent();
    public void SetRoom(EvolutionRoom room)
    {
        this.room = room;
    }

    public void SetRobot(INeuralNetworkAgent robot)
    {
        this.robot = robot;
    }

    private void OnEnable()
    {
        NotToch = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        INeuralNetworkAgent scorer = other.GetComponentInParent<INeuralNetworkAgent>();
        if (scorer is null)
            return;

        if (scorer == robot && NotToch)
        {
            //scorer.PointsForReachingGoal();
            this.gameObject.SetActive(false);
            this.enabled = false;
            NotToch = false;
            //Debug.Log("I'm sleep");
            Succes.Invoke();
            //room.PlaceTarget();
        }
            
    }
}
