//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionarySelection : MonoBehaviour
{
    [SerializeField] private FabricEvolutionRooms fabric;

    [SerializeField] private int countEpochs = 10;
    private int numberEpochs = 0;

    [SerializeField] private int numberAddTimeEpochs = 100;
    [SerializeField] private int countAddTimeEpochs = 100;

    [SerializeField] private int countAgents = 10;

    [Range(5, 30)]
    [SerializeField] private int selectionPercentage = 25;
    private int survivors;

    [Range(0, 50)]
    [SerializeField] private int crossingPercentage = 25;
    private int crossed;

    [Range(1, 75)]
    [SerializeField] private int mutationPercentage = 25;

    private List<EvolutionRoom> rooms;

    private List<NeuralNetwork> brains;
    private List<ScoreBrain> greatBrains;

    [SerializeField] private float MaxTime = 1f;
    [SerializeField] private float AddTime = 0.1f;
    private float timer = 0;

    private NeuralNetwork CreateBrain()
    {
        int countLegs = 6;
        int inputLayer = 3//dimension of Target
                    + 1//Distance to target
                    + 2//Angles between forward and direction to target
                    + 1;//timer
                    //+ countLegs * 3;// prefab.CountLegs //angle of leg                
        int outputLayer = countLegs * 3;//angle of leg

        return new NeuralNetwork(new int[]
                        {
                        inputLayer,
                        //outputLayer*2,
                        inputLayer,
                        //outputLayer*2,
                        //5, 4,
                        outputLayer
        });
    }
    private void Start()
    {
        survivors = Mathf.Max(1, countAgents * selectionPercentage / 100);
        crossed = countAgents * crossingPercentage / 100;

        brains = new List<NeuralNetwork>(countAgents);
        greatBrains = new List<ScoreBrain>(countAgents);

        rooms = fabric.Create(countAgents);

        StartInitialEpochs();
    }

    private void FixedUpdate()
    {        
        timer += Time.fixedDeltaTime;
        
        if (timer >= MaxTime)
        {
            EndEpochs();
            Invoke(nameof(StartEpochs), 1f);            
            timer = -1f;
        }
    }

    private void EndEpochs()
    {
        if (numberEpochs >= countEpochs)
        {
            this.enabled = false;
            Debug.LogError("End Evolutions");
            return;
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].Stop();
        }
    }

    private void StartInitialEpochs()
    {
        numberEpochs = 1;

        brains.Clear();
        greatBrains.Clear();

        NewBrains(0, 0);

        RestartRooms();
    }

    private void StartEpochs()
    {
        numberEpochs++;
        Debug.Log($"Epochs:{numberEpochs}");

        BrainSelection();
        Crossings();
        int mutant = Mutations();
        NewBrains(crossed, mutant);
        RestartRooms();

        int countDead = Mathf.Max(0, greatBrains.Count - survivors);
        greatBrains.RemoveRange(survivors, countDead);

        if (numberEpochs > countAddTimeEpochs)
        {
            countAddTimeEpochs += numberAddTimeEpochs;
            MaxTime += AddTime;
        }
    }

    private void RestartRooms()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].Restart(brains[i]);
        }
    }

    private void BrainSelection()
    {
        for (int i = 0; i < brains.Count; i++)
        {
            greatBrains.Add(new ScoreBrain(brains[i], rooms[i].Score));
        }
        brains.Clear();

        for (int i = 0; i < survivors; i++)
        {
            for (int j = greatBrains.Count - 2; j >= i; j--)
            { 
                if(greatBrains[j].Score < greatBrains[j+1].Score)
                {                    
                    var brain = greatBrains[j];
                    greatBrains[j ] = greatBrains[j+1];
                    greatBrains[j+1] = brain;
                }
            }
        }
        
        Debug.Log($"Max Scope:{greatBrains[0].Score}, " +
            $"New brain Scope:{greatBrains[survivors].Score}, " +
            $"Min Scope:{greatBrains[greatBrains.Count-1].Score}");
    }

    private void Crossings()
    {
        int max = greatBrains.Count;
        for (int i = 0; i < crossed; i++)
        {
            int brain1 = UnityEngine.Random.Range(0, survivors);
            int brain2 = UnityEngine.Random.Range(0, survivors);

            NeuralNetwork brain = NeuralNetwork.Crossing(greatBrains[brain1].Brain,
                                                          greatBrains[brain2].Brain);
            brains.Add(brain);
        }
    }

    private int Mutations()
    {
        int mutants = 0;
        //int max = Mathf.Min(2*survivors, );
        for (int i = 0; i < greatBrains.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 100) < mutationPercentage)
            {
                NeuralNetwork brain;
                if (i < survivors)
                {
                    brain = NeuralNetwork.OneMutation(greatBrains[i].Brain);
                }
                else
                {
                     brain = NeuralNetwork.Crossing(greatBrains[i- survivors].Brain, CreateBrain());
                }

                brains.Add(brain);
                mutants++;
            }
        }

        return mutants;
    }

    private void NewBrains(int crossed, int mutant)
    {
        int newAgents = countAgents - crossed - mutant;
        if (newAgents < 0)
        {
            brains.RemoveRange(countAgents, -newAgents);
            return;
        }

        for (int i = 0; i < newAgents; i++)
        {
            brains.Add(CreateBrain());
        }
    }
}
