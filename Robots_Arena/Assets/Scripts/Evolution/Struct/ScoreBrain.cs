
public class ScoreBrain 
{
    public NeuralNetwork Brain { get; private set; }
    public float Score { get; private set; }

    public ScoreBrain(NeuralNetwork brain, float score)
    {
        Brain = brain;
        Score = score;
    }
}
