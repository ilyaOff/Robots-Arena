using System;
public class NeuralNetworkLayer
{
    private Matrix weight = null;
    public float[,] Weight => weight;
    public int numberInputs => weight.Rows;
    public int numberOutputs => weight.Columns;

    //Input layers
    public NeuralNetworkLayer(int numberOutputs)
    {
        weight = new Matrix(numberOutputs, numberOutputs);
        for (int i = 0; i < numberOutputs; i++)
        {
            weight[i, i] = 1;
        }
    }
    
    public NeuralNetworkLayer(int numberInputs, int numberOutputs)
    {
        weight = new Matrix(numberInputs, numberOutputs);
        RandomWeight();
    }

    public Matrix CalculeteOutput(Matrix input)
    {
        if (input.Rows != 1)
            throw new InvalidOperationException($"{input} is not vector!");

        return Activate(input * weight);
    }

    private void RandomWeight()
    {
        for (int i = 0; i < numberInputs; i++)
        {
            for (int j = 0; j < numberOutputs; j++)
            {
                weight[i, j] = UnityEngine.Random.Range(-1, 1);
            }
        }
    }

    private Matrix Activate(Matrix input)
    {
        Matrix result = input;
        for (int i = 0; i < input.Rows; i++)
        {
            for (int j = 0; i < input.Columns; j++)
            {
                result[i, j] = Sigmoid(input[i, j]);
            }
        }
        return result;
    }

    private float Relu(float input)
    {
        return Math.Max(input, 0);
    }
    private float Sigmoid(float input)
    {
        float result = 1 + (float)Math.Exp(-input);
        return 1 / result;
    }
}

