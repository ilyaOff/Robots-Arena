using System;
public class Matrix
{
    private float[,] matrix;

    public int Columns => matrix.GetUpperBound(1);
    public int Rows => matrix.GetUpperBound(0);

    public Matrix(float[] value)
    {
        matrix = new float[1, value.Length];        
        for (int j = 0; j < Columns; j++)
        {
            matrix[0, j] = value[j];
        }        
    }
    public Matrix(float[,] value)
    {
        matrix = value;
    }
    public Matrix(int rows, int columns)
    {
        matrix = new float[rows, columns];
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                matrix[i, j] = 0;
            }
        }
    }

    public float this[int i, int j]
    {
        get { return matrix[i, j]; }
        set { matrix[i, j] = value; }
    }

    public static implicit operator float[,](Matrix matrix) => matrix.matrix;
    public static explicit operator Matrix(float[,] value) => new Matrix(value);

    public static Matrix operator *(Matrix left, Matrix right)
    {
        if (left.Columns != right.Rows)
            throw new InvalidOperationException($"Matrix *: {left.matrix}, {right.matrix}");

        Matrix result = new Matrix(left.Rows, right.Columns);
        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                for (int k = 0; k < right.Rows; k++)
                {
                    result[i, j] += left[i, k] * right[k, j];
                }
            }
        }
        return result;
    }
    public static Matrix operator +(Matrix left, Matrix right)
    {
        if (left.Columns != right.Columns || left.Rows != right.Rows)
            throw new InvalidOperationException($"Matrix +: {left.matrix}, {right.matrix}");

        Matrix result = new Matrix(left.Rows, right.Columns);
        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {                
                result[i, j] = left[i, j] + right[i, j];                
            }
        }
        return result;
    }
    public static Matrix operator -(Matrix left, Matrix right)
    {        
        return left + -right;
    }
    public static Matrix operator +(Matrix matrix)
    {        
        return matrix;
    }
    public static Matrix operator -(Matrix matrix)
    {
        Matrix result = new Matrix(matrix.Rows, matrix.Columns);
        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                result[i, j] = -matrix[i, j];
            }
        }
        return result;
    }

}
