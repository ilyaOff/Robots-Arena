using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionarySelection : MonoBehaviour
{
    [SerializeField] private FabricEvolutionRooms fabric;

    [SerializeField] private int countEpochs = 10;
    private int numberEpochs = 0;
    private int round = 0;

    [SerializeField] private int numberAddTimeEpochs = 100;
    [SerializeField] private int countAddTimeEpochs = 100;
    [Min(1)]
    [SerializeField] private int countAgents = 10;

    [Range(5, 50)]
    [SerializeField] private int selectionPercentage = 25;
    public int Survivors => Mathf.Max(1, countAgents * selectionPercentage / 100);

    [Range(0, 75)]
    [SerializeField] private int crossingPercentage = 25;
    private int Crossed => countAgents* crossingPercentage / 100;

    /*[Range(0, 50)]
    [SerializeField] private int randomPercentage = 25;
    private int randomed;
    */

    [Range(1, 75)]
    [SerializeField] private int mutationPercentage = 25;

    [Range(1, 75)]
    [SerializeField] private int mutationLitePercentage = 25;

    private List<EvolutionRoom> rooms;

    private List<NeuralNetwork> brains;
    private List<ScoreBrain> greatBrains;

    [SerializeField] private float MaxTime = 1f;
    [SerializeField] private float AddTime = 0.1f;
    private float timer = 0;

    private NeuralNetwork CreateBrain()
    {
        int countLegs = 6;
        int legParametrs = 10;
        int inputLayer = 3//dimension of Target
                    + 1//Distance to target
                    + 2//Angles between forward and direction to target  
                     + countLegs // in ground
                    + countLegs * legParametrs;// prefab.CountLegs      
       
        int outputLayer = countLegs * 3;//angle of leg

        return new NeuralNetwork(new int[]
                        {
                        inputLayer,
                        //outputLayer*2,
                        12,
                        6,
                        //outputLayer*2,
                        //5, 4,
                        outputLayer
        });
    }
    private void Start()
    {
        brains = new List<NeuralNetwork>();
        greatBrains = new List<ScoreBrain>();

        rooms = fabric.Create(countAgents);

        Random.InitState(123456);
        StartInitialEpochs();
    }

    private void FixedUpdate()
    {        
        timer += Time.fixedDeltaTime;
        
        if (timer >= MaxTime)
        {
            EndEpochs();
            StartEpochs();
            //Invoke(nameof(StartEpochs), 1f);            
            timer = 0;
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

        /*for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].Stop();
        }*/
    }

    private void StartInitialEpochs()
    {
        numberEpochs = 1;
        NewBrains(0);       
        RestartRooms();
    }

    private void StartEpochs()
    {
        round++;
        if (round >= 2)
        {
            numberEpochs++;
            Debug.Log($"Epochs:{numberEpochs}");
            round = 0;
        }

        int newAgent = 0;

        newAgent += BrainSelection();
        switch (round)
        {
            case 0:                
                newAgent += Mutations();
                newAgent += LiteMutations();
                break;
            case 1:
                newAgent += Crossings();
                break;
        }

        NewBrains(newAgent);

        RestartRooms();

        /*int countDead = Mathf.Max(0, greatBrains.Count - Survivors);
       greatBrains.RemoveRange(Survivors, countDead);
       */
        greatBrains.Clear();

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

    private int BrainSelection()
    {
        for (int i = 0; i < brains.Count; i++)
        {
            greatBrains.Add(new ScoreBrain(brains[i], rooms[i].Score));
        }
        brains.Clear();

        for (int i = 0; i < /*Survivors*/ greatBrains.Count; i++)
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
        for (int i = 0; i < Survivors; i++)
        {
            brains.Add(greatBrains[i].Brain);
        }
        

        Debug.Log($"Max Scope:{greatBrains[0].Score}, " +
            $"New brain Scope:{greatBrains[Survivors-1].Score}, " +
            $"Min Scope:{greatBrains[greatBrains.Count-1].Score}");

        return Survivors;
    }

    private int Crossings()
    {
        for (int i = 0; i < Crossed; i++)
        {
            int brain1 = Random.Range(0, Survivors);
            int brain2 = Random.Range(0, Survivors);

            NeuralNetwork brain = NeuralNetwork.Crossing(greatBrains[brain1].Brain,
                                                          greatBrains[brain2].Brain);
            brains.Add(brain);
        }
        return Crossed;
    }

    private int LiteMutations()
    {
        int mutants = 0;

        for (int i = 0; i < greatBrains.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 100) < mutationLitePercentage)
            {
                NeuralNetwork brain = NeuralNetwork.OneMutation(greatBrains[i].Brain);

                brains.Add(brain);
                mutants++;
            }
        }

        return mutants;
    }
    private int Mutations()
    {
        if (Survivors > greatBrains.Count) return 0;

        int mutants = 0;
        
		for (int i = 0; i < Survivors; i++)
        {
            if (Random.Range(0, 100) < mutationPercentage)
            {
                int random = Random.Range(Survivors, greatBrains.Count);
                NeuralNetwork brain = NeuralNetwork.Crossing(
                                 greatBrains[random].Brain,
                                greatBrains[i].Brain);                

                brains.Add(brain);
                mutants++;
            }
        }

        return mutants;
    }

    private void NewBrains(int addedAgents)
    {
        int randomAgents = countAgents - addedAgents;
        if (randomAgents < 0)
        {
            brains.RemoveRange(countAgents, -randomAgents);
            return;
        }

        for (int i = 0; i < randomAgents; i++)
        {
            brains.Add(CreateBrain());
        }
    }
}
