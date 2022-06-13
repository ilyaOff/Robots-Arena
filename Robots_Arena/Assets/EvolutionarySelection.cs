//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionarySelection : MonoBehaviour
{
    [SerializeField] private LegController prefab;
    [SerializeField] private TargetRobots targetPrefab;

    [SerializeField] private int countEpochs = 10;
    private int numberEpochs = 0;
    [SerializeField] private int countAgents = 10;

    [Range(5, 30)]
    [SerializeField] private int selectionPercentage = 25;
    private int survivors;

    [Range(0, 50)]
    [SerializeField] private int crossingPercentage = 25;
    private int crossed;

    [Range(1, 75)]
    [SerializeField] private int mutationPercentage = 25;

    private List<EvolutionScorer> agents;
    private List<NeuralNetwork> greatBrain;
    private List<TargetRobots> targets;

    [SerializeField] private float MaxTime = 10f;
    private float timer = 0;
    private void Start()
    {
        survivors = Mathf.Max(1, countAgents * selectionPercentage / 100);
        crossed = countAgents * crossingPercentage / 100;

        greatBrain = new List<NeuralNetwork>(countAgents);
        greatBrain.Clear();
        NewBrains(0, 0, 0);

        agents = new List<EvolutionScorer>(countAgents);
        targets = new List<TargetRobots>(countAgents);

        for (int i = 0; i < countAgents; i++)
        {
            LegController newAgent = Instantiate(prefab);
            EvolutionScorer scorer = newAgent.gameObject.AddComponent<EvolutionScorer>();
            agents.Add(scorer);

            TargetRobots target = Instantiate(targetPrefab);
            target.SetSelector(this);
            targets.Add(target);
        }
        PlaceAgents();
        numberEpochs = 1;
    }

    private void FixedUpdate()
    {        
        timer += Time.fixedDeltaTime;
        if (timer >= MaxTime)
        {
            EndEpochs();
            StartEpochs();
            timer = 0;
        }
    }

    private void EndEpochs()
    {
        /*foreach (var agent in agents)
        {
            agent.enabled = false;
            Destroy(agent.gameObject);
        }
        agents.Clear();
        */
        if (numberEpochs >= countEpochs)
        {
            this.enabled = false;
            Debug.LogError("End Evolutions");
        }
    }

    private void StartEpochs()
    {
        numberEpochs++;
        Debug.Log($"Epochs:{numberEpochs}");
        BrainSelection();
        Crossings();
        int mutant = Mutations();
        NewBrains(survivors, crossed, mutant);

        PlaceAgents();
    }

    private void StartAgent(EvolutionScorer agent, Vector3 position, NeuralNetwork brain, TargetRobots target)
    {
        agent.controller.NewBrain(brain);
        agent.transform.position = position;
        agent.transform.rotation = Quaternion.identity;
        agent.enabled = true;      

        target.SetRobot(agent);
        
        PlaceTarget(target, agent);
    }

    private void BrainSelection()
    {        
        for (int i = 0; i < survivors; i++)
        {
            for (int j = agents.Count - 2; j >= i; j--)
            { 
                if(agents[j].Score < agents[j+1].Score)
                {
                    var agent = agents[j];
                    agents[j] = agents[j+1];
                    agents[j+1] = agent;
                    
                    var brain = greatBrain[j];
                    greatBrain[j ] = greatBrain[j+1];
                    greatBrain[j+1] = brain;
                }
            }
        }
        greatBrain.RemoveRange(survivors, greatBrain.Count-1 - survivors);
    }

    private void Crossings()
    {
        for (int i = 0; i < crossed; i++)
        {
            int brain1 = UnityEngine.Random.Range(0, survivors);
            int brain2 = UnityEngine.Random.Range(0, survivors);

            NeuralNetwork brain = NeuralNetwork.Crossing(greatBrain[brain1], greatBrain[brain2]);
            greatBrain.Add(brain);
        }
    }

    private int Mutations()
    {
        int mutants = 0;
        for (int i = 0; i < survivors; i++)
        {
            if (UnityEngine.Random.Range(0, 100) < mutationPercentage)
            {
                NeuralNetwork brain = NeuralNetwork.OneMutation(greatBrain[i]);
                greatBrain.Add(brain);
                mutants++;
            }
        }

        return mutants;
    }

    private NeuralNetwork CreateBrain()
    {
        int countLegs = 6;
        int inputLayer = 3//dimension of Target
                    + countLegs * 3;// prefab.CountLegs //angle of leg                
        int outputLayer = countLegs * 3;//angle of leg

        return new NeuralNetwork(new int[]
                        {
                        inputLayer,
                        5, 4,
                        outputLayer
        });
    }

    private void NewBrains(int survivors, int crossed, int mutant)
    {
        int newAgents = countAgents - survivors - crossed - mutant;
        if (newAgents < 1)
            return;

        for (int i = 0; i < newAgents; i++)
        {
            greatBrain.Add(CreateBrain());
        }
    }
    
    private void PlaceAgents()
    {
        Vector3 startPlace = Vector3.up*2;
        Vector3 shift = Vector3.forward * 10;
        for (int i = 0; i < countAgents; i++)
        {
            StartAgent(agents[i], startPlace + shift * i, greatBrain[i], targets[i]);
        }
    }

    public void PlaceTarget(TargetRobots target, EvolutionScorer robot)
    {
        Vector2 newPlace = UnityEngine.Random.insideUnitCircle * 10;
        Vector3 startPlace = robot.transform.position + new Vector3(newPlace.x, 0, newPlace.y);

        target.transform.position = startPlace;
        //TargetRobots target = Instantiate(targetPrefab, startPlace, Quaternion.identity);
        target.enabled = true;

        robot.ChangeTarget(target);
    }
}
