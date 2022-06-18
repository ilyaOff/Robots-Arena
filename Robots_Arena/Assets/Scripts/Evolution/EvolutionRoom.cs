using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionRoom : MonoBehaviour
{
    [SerializeField] private Transform[] goals;
    [SerializeField] private int currentGoal = 0;
    private int startGoal = -1;
    [SerializeField] private Transform StartAgent;

    [SerializeField] private TargetRobots target;
    [SerializeField] private EvolutionScorer agent;

    public float Score => agent.Score;

   /* private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        int height = 4;
        Gizmos.DrawWireCube(transform.position + Vector3.up* height/2, new Vector3(10, height, 10));
    }*/

    public void Initialize(EvolutionScorer newAgent, TargetRobots newTarget)
    {
        if (newAgent is null)
        {
            throw new System.ArgumentNullException("Null agent");
        }

        if (newTarget is null)
        {
            throw new System.ArgumentNullException("Null target");
        }

        agent = newAgent;
        agent.ChangeTarget(newTarget);
        agent.enabled = false;
        PlaceAgent();

        target = newTarget;
        /*if (target != null)
        {
            target.Succes.RemoveListener(PlaceTarget);
        }        
        target.Succes.AddListener(PlaceTarget);*/
        target.SetRoom(this);
        target.SetRobot(newAgent);
        target.enabled = false;
        //startGoal = currentGoal = Random.Range(0, goals.Length);
        PlaceTarget();
    }

    public void Restart(NeuralNetwork brain)
    {
        RestartAgent(brain);
        currentGoal = startGoal;
        PlaceTarget();
        /*
        target.SetRobot(robot);
       robot.ChangeTarget(target);
       */
    }

    private void PlaceAgent()
    {
        agent.transform.position = StartAgent.position;
        agent.transform.rotation = Quaternion.identity;
    }

    public void Stop()
    {
        //agent.Score += agent.transform.position.z - StartAgent.position.z;
       
        agent.controller.InitialPosition();
        PlaceAgent();
        agent.enabled = false;
    }

    private void RestartAgent(NeuralNetwork brain)
    {
        agent.controller.NewBrain(brain);
        
        agent.enabled = true;

        //if (Vector3.Distance(agent.transform.position, StartAgent.position) > 15f)
        {
            PlaceAgent();
        }
    }

    public void PlaceTarget()
    {
        currentGoal = (currentGoal + 1) % goals.Length;
        target.transform.position = goals[currentGoal].position ;
        target.enabled = true;
    }
}
