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
    [SerializeField] private int numberAddTimeEpochs = 1000;
    private int countAddTimeEpochs = 1000;

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
    private List<NeuralNetwork> brains;
    private List<NeuralNetwork> greatBrains;
    private List<TargetRobots> targets;

    [SerializeField] private float MaxTime = 1f;
    [SerializeField] private float AddTime = 1f;
    private float timer = 0;
    private void Start()
    {
        survivors = Mathf.Max(1, countAgents * selectionPercentage / 100);
        crossed = countAgents * crossingPercentage / 100;

        brains = new List<NeuralNetwork>(countAgents);
        greatBrains = new List<NeuralNetwork>(countAgents);
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
        for (int i = 0; i < countAgents; i++)
        {
            PlaceTarget(targets[i], agents[i]);
        }
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

        int countDead = Mathf.Max(0, brains.Count - countAgents);
        brains.RemoveRange(countAgents, countDead);

        PlaceAgents();

        if (numberEpochs > countAddTimeEpochs)
        {
            countAddTimeEpochs += numberAddTimeEpochs;
            MaxTime += AddTime;
        }
    }

    private void StartAgent(EvolutionScorer agent, Vector3 position, NeuralNetwork brain, TargetRobots target)
    {
        agent.controller.NewBrain(brain);
        agent.transform.position = position;
        agent.transform.rotation = Quaternion.identity;
        agent.enabled = false;
        agent.enabled = true;      

        //PlaceTarget(target, agent);
    }

    private void BrainSelection()
    {
        //brains.AddRange(greatBrains);
        for (int i = 0; i < survivors; i++)
        {
            for (int j = agents.Count - 2; j >= i; j--)
            { 
                if(agents[j].Score < agents[j+1].Score)
                {
                    var agent = agents[j];
                    agents[j] = agents[j+1];
                    agents[j+1] = agent;
                    
                    var brain = brains[j];
                    brains[j ] = brains[j+1];
                    brains[j+1] = brain;
                }
            }
        }
        //greatBrains = brains.GetRange(0, greatBrains.Count);
        //brains.RemoveRange(0, greatBrains.Count);
        Debug.Log($"Max Scope:{agents[0].Score}, Min Scope:{agents[agents.Count-1].Score}");
    }

    private void Crossings()
    {
        int max = brains.Count;
        for (int i = 0; i < crossed; i++)
        {
            int brain1 = UnityEngine.Random.Range(0, max);
            int brain2 = UnityEngine.Random.Range(0, max);

            NeuralNetwork brain = NeuralNetwork.Crossing(brains[brain1], brains[brain2]);
            brains.Add(brain);
        }
    }

    private int Mutations()
    {
        int mutants = 0;
        for (int i = 0; i < brains.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 100) > mutationPercentage)
            {
                //NeuralNetwork brain = NeuralNetwork.OneMutation(brains[i]);
                NeuralNetwork brain = NeuralNetwork.Crossing(brains[i], CreateBrain());
                brains.Add(brain);
                mutants++;
            }
        }

        return mutants;
    }

    private NeuralNetwork CreateBrain()
    {
        int countLegs = 6;
        int inputLayer = 3//dimension of Target
                    + 1//Distance to target
                    + countLegs * 3;// prefab.CountLegs //angle of leg                
        int outputLayer = countLegs * 3;//angle of leg

        return new NeuralNetwork(new int[]
                        {
                        inputLayer,
                        //outputLayer*2,
                        outputLayer*2,
                        //outputLayer*2,
                        //5, 4,
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
            brains.Add(CreateBrain());
        }
    }
    
    private void PlaceAgents()
    {
        Vector3 startPlace = Vector3.up*2;
        Vector3 shift = Vector3.forward * 10 ;
        for (int i = 0; i < countAgents; i++)
        {
            StartAgent(agents[i], startPlace + shift * (i%10) + 10*Vector3.right * (i / 10), brains[i], targets[i]);
        }
    }

    public void PlaceTarget(TargetRobots target, EvolutionScorer robot)
    {
        float R = 5;
        float angle = UnityEngine.Random.Range(0, Mathf.PI*2);
        Vector3 startPlace = robot.transform.position 
            + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * R;

        target.transform.position = startPlace;
        //TargetRobots target = Instantiate(targetPrefab, startPlace, Quaternion.identity);
        target.enabled = true;

        target.SetRobot(robot);
        robot.ChangeTarget(target);
    }
}
