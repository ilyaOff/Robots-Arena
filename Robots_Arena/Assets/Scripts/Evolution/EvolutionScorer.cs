using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(INeuralNetworkAgent))]
public class EvolutionScorer : MonoBehaviour
{
    /*
         Этот класс используется, так как он обладает свойством Transpform
    */
    [SerializeField] private float pointsGameOver = -50f;
    [SerializeField] private float pointsForReachingGoal = 1000f;

    public float Score { get; private set; }

    private INeuralNetworkAgent controller;

    private void Awake()
    {
        Score = 0;
        controller = this.GetComponent<INeuralNetworkAgent>();
    }

   /* private void FixedUpdate()
    {
        float sum = 0;
        foreach (var challenge in Challenges)
        {
            challenge.Update();
            sum += challenge.Score;
        }
        Score = sum;
    }*/

    /*public void NewChallanges(List<Challenge> challenges)
    {
        if (challenges is null || challenges.Count < 1)
            throw new System.ArgumentException("Invalid challanges");

        _challenges = challenges;
    }*/

    public void Restart(NeuralNetwork brain)
    {
        Score = 0;
        controller.NewBrain(brain);

        /*foreach (var challenge in _challenges)
        {
            challenge.Restart();
        }*/
    }

    public void PointsForReachingGoal()
    {
        Score += pointsForReachingGoal;
    }
    
    private void GameOver()
    {
        Score += pointsGameOver;
        this.enabled = false;
    }

}
