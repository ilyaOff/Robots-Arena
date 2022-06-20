using System;
public struct NeuralNetworkLayer
{
    private Matrix weights;// = null;
    private Matrix offset;// = null;
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
        offset = new Matrix(1, numberOutputs);
        RandomWeights();
    }

    private NeuralNetworkLayer(NeuralNetworkLayer original)
    {
        weights = new Matrix(original.weights);
        offset = new Matrix(original.offset);
    }

    private NeuralNetworkLayer(Matrix weight, Matrix offset)
    {
        this.weights = weight;
        this.offset = offset;
    }

    public static NeuralNetworkLayer Crossing(NeuralNetworkLayer first, NeuralNetworkLayer second)
    {
        if (first.numberInputs != second.numberInputs || first.numberOutputs != second.numberOutputs)
        {
            throw new ArgumentException("Can crossing only equals size layers!");
        }

        int Rows = first.numberInputs;
        int Columns = first.numberOutputs;
        Matrix weight = new Matrix(Rows, Columns);

        for (int i = 0; i < Rows; i++)
        {
            /*NeuralNetworkLayer layerRows = null;
            if (UnityEngine.Random.Range(0, 2) % 2 == 0)
            {
                layerRows = first;
            }
            else
            {
                layerRows = second;
            }
            for (int j = 0; j < Columns; j++)
            {
                weight[i, j] = layerRows.weights[i, j];
            }*/
            for (int j = 0; j < Columns; j++)
            {
                //weight[i, j] = (first.weights[i, j]+ second.weights[i, j])/2;
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
        Matrix offset = new Matrix(1, Columns);
        for (int j = 0; j < Columns; j++)
        {
            //offset[0, j] = (first.offset[0, j] + second.offset[0, j]) / 2;
            if (UnityEngine.Random.Range(0, 2) % 2 == 0)
            {
                offset[0, j] = first.offset[0, j];
            }
            else
            {
                offset[0, j] = second.offset[0, j];
            }
        }
        return new NeuralNetworkLayer(weight, offset);
    }

    public static NeuralNetworkLayer OneMutation(NeuralNetworkLayer original)
    {
        NeuralNetworkLayer layer = new NeuralNetworkLayer(original);

        int mutationRow = UnityEngine.Random.Range(0, layer.numberInputs + 1);
        int mutationColumn = UnityEngine.Random.Range(0, layer.numberOutputs);
        if (mutationRow > layer.numberInputs - 1)
        {
            layer.offset[0, mutationColumn] = RandomWeight();
        }
        else
        {
            layer.weights[mutationRow, mutationColumn] = RandomWeight();
        }
        

        return layer;
    }
    public Matrix CalculeteOutput(Matrix input)
    {
        if (input.Rows != 1)
            throw new InvalidOperationException($"{input.Rows} is not vector!");

        return Activate(input * weights + offset);
    }

    private void RandomWeights()
    {
        for (int j = 0; j < numberOutputs; j++)
        {
            for (int i = 0; i < numberInputs; i++)
            {            
                weights[i, j] = RandomWeight();
            }
            offset[0, j] = RandomWeight();
        }
    }

    private static float RandomWeight()
    {
        float RangeWeight = 10f;
        return UnityEngine.Random.Range(-RangeWeight, RangeWeight);
    }

    private Matrix Activate(Matrix input)
    {
        Matrix result = input;
        for (int i = 0; i < input.Rows; i++)
        {
            for (int j = 0; j < input.Columns; j++)
            {
                result[i, j] = HyperbolicTangent(input[i, j]);
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

    private float HyperbolicTangent(float input)
    {
        float result = (float)Math.Exp(2*input);
        if (float.IsInfinity(result))
            return 1f;
        return (result - 1) / (result + 1);
    }

    private float Sin(float input)
    {
        return (float)Math.Sin(Math.PI * input);
    }

    private float NonChange(float input)
    {
        return input;
    }
}

