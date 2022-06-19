using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INeuralNetworkAgent  
{
    NeuralNetwork Brain { get; }
    Transform transform { get; }

    void NewBrain(NeuralNetwork brain);
}
