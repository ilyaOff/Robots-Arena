using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionRoom : MonoBehaviour
{
    [SerializeField] private Transform[] goals;
    [SerializeField] private int currentGoal = 0;
    private int startGoal = 1;

    [SerializeField] private Transform startAgent;

    [SerializeField] private TargetRobots target;
    [SerializeField] private INeuralNetworkAgent agent;

    [SerializeField] private List<Challenge> _challenges;
    //public IEnumerable<Challenge> Challenges => _challenges;
    public float Score { get; private set; }

    /* private void OnDrawGizmos()
     {
         Gizmos.color = Color.green;
         int height = 4;
         Gizmos.DrawWireCube(transform.position + Vector3.up* height/2, new Vector3(10, height, 10));
     }*/
    private void Start()
    {
        startGoal = Random.Range(0, goals.Length);
    }

    private void FixedUpdate()
    {
        float sum = 0;
        foreach (var challenge in _challenges)
        {
            challenge.Update();
            sum += challenge.Score;
        }
        Score = sum;
    }
    public void Initialize(INeuralNetworkAgent newAgent, TargetRobots newTarget)
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
        PlaceAgent();
        

        if(target != null)
            target.Succes.RemoveListener(TestListener);

        target = newTarget;
        target.SetRoom(this);
        target.SetRobot(newAgent);
        target.enabled = false;
        PlaceTarget();

        target.Succes.AddListener(TestListener);

        foreach (var challenge in _challenges)
        {
            challenge.StartChallenge(newAgent, newTarget);
        }
    }

    private void TestListener()
    {
        Debug.Log("ListenerSucces");
    }

    public void Restart(NeuralNetwork brain)
    {
        agent.NewBrain(brain);
        PlaceAgent();
        foreach (var challenge in _challenges)
        {
            challenge.Restart();
        }

        currentGoal = startGoal;
        PlaceTarget();
    }

    private void PlaceAgent()
    {
        agent.transform.position = startAgent.position;
        agent.transform.rotation = Quaternion.identity;
    }

    public void PlaceTarget()
    {
        currentGoal = (currentGoal + 1) % goals.Length;
        target.transform.position = goals[currentGoal].position;
        target.enabled = true;
    }
}
