using System;
public class NeuralNetworkLayer
{
    private Matrix weights = null;
    public float[,] Weight => weights;
    public int numberInputs => weights.Rows;
    public int numberOutputs => weights.Columns;

    //Input layers
    /*public NeuralNetworkLayer(int numberOutputs)
    {
        weight = new Matrix(numberOutputs, numberOutputs);
        for (int i = 0; i < numberOutputs; i++)
        {
            weight[i, i] = 1;
        }
    }*/
    
    public NeuralNetworkLayer(int numberInputs, int numberOutputs)
    {
        weights = new Matrix(numberInputs, numberOutputs);
        RandomWeights();
    }

    private NeuralNetworkLayer(NeuralNetworkLayer original)
    {
        weights = new Matrix(original.weights);
    }

    private NeuralNetworkLayer(Matrix weight)
    {
        this.weights = weight;
    }

    public static NeuralNetworkLayer Crossing(NeuralNetworkLayer first, NeuralNetworkLayer second)
    {
        if (first.numberInputs != second.numberInputs || first.numberOutputs != second.numberOutputs)
        {
            throw new ArgumentException("Crossing only equals size!");
        }

        int Rows = first.numberInputs;
        int Columns = first.numberOutputs;
        Matrix weight = new Matrix(Rows, Columns);

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (UnityEngine.Random.Range(0, 2) % 2 == 0)
                {
                    weight[i, j] = first.weights[i, j];
                }
                else
                {
                    weight[i, j] = second.weights[i, j];
                }                    
            }
        }
        return new NeuralNetworkLayer(weight);
    }

    public static NeuralNetworkLayer OneMutation(NeuralNetworkLayer original)
    {
        NeuralNetworkLayer layer = new NeuralNetworkLayer(original);

        int mutationRow = UnityEngine.Random.Range(0, layer.weights.Rows);
        int mutationColumn = UnityEngine.Random.Range(0, layer.weights.Columns);

        layer.weights[mutationRow, mutationColumn] = RandomWeight();

        return layer;
    }
    public Matrix CalculeteOutput(Matrix input)
    {
        if (input.Rows != 1)
            throw new InvalidOperationException($"{input.Rows} is not vector!");

        return Activate(input * weights);
    }

    private void RandomWeights()
    {
        for (int i = 0; i < numberInputs; i++)
        {
            for (int j = 0; j < numberOutputs; j++)
            {
                weights[i, j] = RandomWeight();
            }
        }
    }

    private static float RandomWeight()
    {
        return UnityEngine.Random.Range(-1, 1);
    }

    private Matrix Activate(Matrix input)
    {
        Matrix result = input;
        for (int i = 0; i < input.Rows; i++)
        {
            for (int j = 0; j < input.Columns; j++)
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

