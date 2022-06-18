using System;
using System.Collections.Generic;
using UnityEngine;

public struct NeuralNetwork 
{
    private NeuralNetworkLayer[] layers;// = null;

    public int Inputs => layers[0].numberInputs;
    public int Outputs => layers[CountLayers-1].numberOutputs;
    public int CountLayers => layers.Length;
    public NeuralNetwork(int[] size)
    {
        if (size is null || size.Length == 0)
            throw new ArgumentException("Invalid size");

        layers = new NeuralNetworkLayer[size.Length-1];
                
        //layers[0] = new NeuralNetworkLayer(size[0]);

        for (int i = 0; i < CountLayers; i++)
        {            
            layers[i] = new NeuralNetworkLayer(size[i], size[i+1]);
        }
    }

    private NeuralNetwork(NeuralNetwork original)
    {
        this.layers = (NeuralNetworkLayer[]) original.layers.Clone();
    }

    private NeuralNetwork(NeuralNetworkLayer[] layers)
    {
        if (layers is null || layers.Length == 0)
            throw new ArgumentException("Invalid size of layers");

        this.layers = layers;
    }

    public static NeuralNetwork Crossing(NeuralNetwork brain1, NeuralNetwork brain2)
    {
        if (NonEquivalentSizes(brain1, brain2))
        {
            throw new ArgumentException("Brains is non equivalent sizes");
        }

        return NeuralNetwork.Crossing(brain1.layers, brain2.layers);
    }

    private static NeuralNetwork Crossing(NeuralNetworkLayer[] first, NeuralNetworkLayer[] second)
    {
        int size = first.Length;
        NeuralNetworkLayer[] layers = new NeuralNetworkLayer[size];
        for (int i = 0; i < size; i++)
        {
            layers[i] = NeuralNetworkLayer.Crossing(first[i], second[i]);
        }
        return new NeuralNetwork(layers);
    }
    
    public static NeuralNetwork OneMutation(NeuralNetwork original)
    {
        NeuralNetwork newBrain = new NeuralNetwork(original);

        int layerMutation = UnityEngine.Random.Range(0, newBrain.CountLayers);
        NeuralNetworkLayer layer = newBrain.layers[layerMutation];

        newBrain.layers[layerMutation] = NeuralNetworkLayer.OneMutation(layer);

        return newBrain;
    }
    private static bool NonEquivalentSizes(NeuralNetwork brain1, NeuralNetwork brain2)
    {
        if (brain1.CountLayers != brain2.CountLayers)
            return true;

        if (brain1.layers[0].numberInputs != brain1.layers[0].numberInputs)
            return true;

        int size = brain1.CountLayers;
        for (int i = 0; i < size; i++)
        {
            if (brain1.layers[i].numberOutputs != brain1.layers[i].numberOutputs)
                return true;
        }

        return false;
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
        Matrix tmpresult = CalculeteOutput(new Matrix(input));
        float[] result = new float[tmpresult.Columns];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = tmpresult[0, i];
        }
        return result;
    }
}
