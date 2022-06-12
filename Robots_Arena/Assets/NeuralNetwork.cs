using System;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork 
{
    NeuralNetworkLayer[] layers = null;

    public NeuralNetwork(int[] size)
    {
        if (size is null || size.Length == 0)
            throw new InvalidOperationException("Invalid size");

        layers = new NeuralNetworkLayer[size.Length];
        int inputSize = 1;
        for (int i = 0; i < size.Length; i++)
        {            
            layers[i] = new NeuralNetworkLayer(inputSize, size[i]);
            inputSize = size[i];
        }
    }

    private Matrix CalculeteOutput(Matrix input)
    {
        Matrix result = input;
        for (int i = 0; i < layers.Length; i++)
        {
            result = layers[i].CalculeteOutput(result);
        }
        return result;
    }

    public float[] CalculeteOutput(float[] input)
    {
        float[,] tmpresult = CalculeteOutput(new Matrix(input));
        float[] result = new float[tmpresult.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = tmpresult[0, i];
        }
        return result;
    }
}
