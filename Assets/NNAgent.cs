using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActivationFunction
{
    public double Activate(double input);
}

public class SigmoidActivationFunction : ActivationFunction
{
    double ActivationFunction.Activate(double input)
    {
        return 1.0f / (1.0f + Math.Exp(-input));
    }
}

public class RELUActivationFunction : ActivationFunction
{
    double ActivationFunction.Activate(double input)
    {
        return Math.Max(input, 0.0f);
    }
}


public class Neuron
{
    public double[] weights;
    public double bias;
    public double forward;
    public ActivationFunction activationFunction;
    public Neuron(int weightCount, double bias)
    {
        weights = new double[weightCount];
        this.bias = bias;
        this.forward = 0;
        activationFunction = new SigmoidActivationFunction();
    }

    public void InitializeRandomWeights(double min, double max)
    {
        for (int i = 0; i < weights.Length; i++) {
            double w = (double)UnityEngine.Random.Range((float)min, (float)max);
            weights[i] = w;
        }        
    }

    public double Forward(double[] inputs) 
    {
        double ans = 0.0f;
        for (int i = 0; i < inputs.Length - 1; i++) {
            if(i < weights.Length && i < inputs.Length)
            {
                ans += weights[i] * inputs[i];
            }
            
        }

        forward = activationFunction.Activate(ans);

        return ans;
    }
}

public class Layer
{
    public Neuron[] neurons;
    public double[] answers;

    public Layer()
    {
        
    }

    public Layer(int neuronCount, int weightCount)
    {
        neurons = new Neuron[neuronCount];
        answers = new double[neuronCount];
        for (int i = 0; i < neurons.Length; i++)
        {
            neurons[i] = new Neuron(weightCount, 0.0);
        }
    }

    public void InitializeRandomWeights(double min, double max)
    { 
        foreach (Neuron neuron in neurons) {
            neuron.InitializeRandomWeights(min, max);
        }
    }

    public void Forward(double[] inputs)
    {
        for (int i = 0; i < neurons.Length; i++) {
            answers[i] = neurons[i].Forward(inputs);
        }
    }
}

/*
INPUT FEATURES
player1X
player1Y
player1Health
player1Attack
player1DEF

player2X
player2Y
player2Health
player2Attack
player2DEF

swordLocation1X
swordLocation1Y
swordLocation2X
swordLocation2Y

sheildLocation1X
sheildLocation1Y
sheildLocation2X
sheildLocation2Y

healthLocation1X
healthLocation1Y
healthLocation2X
healthLocation2Y
healthLocation3X
healthLocation3Y
 */

/*
OUPUT FEATURES
moveLeft
moveRight
moveDown
moveUp

attack
pickup
 */


public class NN
{
    //public const int neruonCount = 5 + 5 + 4 + 4 + 6;
    public InputData InputData = GameObject.Find("NNInputGetter").GetComponent<InputData>();

    public ActivationFunction sigmoid;


    public List<double> inputLayer = new List<double>();
    public Layer InputLayer;

    public const int outputCount = 5;
    public Layer outputLayer;

    public NN()
    {
        foreach(double t in InputData.GetInputData())
        {
            //Debug.Log(t);
            inputLayer.Add(t);
        }

        for (int i = 0; i < inputLayer.ToArray().Length; i++)
        {
            //inputLayer[i] = UnityEngine.Random.Range(-1.0f, 1.0f);
            sigmoid = new SigmoidActivationFunction();
            inputLayer[i] = sigmoid.Activate(inputLayer[i]);
            
        }
        InputLayer = new Layer(inputLayer.ToArray().Length, inputLayer.ToArray().Length);
        outputLayer = new Layer(outputCount, outputCount);
        InputLayer.InitializeRandomWeights(-1.0, 1.0);
        outputLayer.InitializeRandomWeights(-1.0, 1.0);        
    }

    public void Forward()
    {
        InputLayer.Forward(inputLayer.ToArray());
        outputLayer.Forward(InputLayer.answers);
    }

    
}


public class NNAgent : Agent
{
    public NN nn;
    public int chioce;
    public InputData InputData;
    

    public override void Action(Map map)
    {
        
        nn = new NN();
        nn.Forward();
        int t = GetMaxIndex(nn.outputLayer.answers);
        chioce = t;
        Debug.Log(chioce);
        for (int i = 0; i < nn.InputLayer.neurons.Length; i++)
        {
            for (int j = 0; j < nn.InputLayer.neurons.Length; j++)
            {
                double d = nn.InputLayer.neurons[i].weights[j];
                //Debug.Log($"Neuron: {i} weight: {d}");
            }
        }
        for (int i = 0; i < 2; i++)
        {
            switch (chioce)
            {
                case 0:
                    {
                        BoardPlacement placement = map.placements[unit.CurrentPlacement.i - 1, unit.CurrentPlacement.j];
                        unit.MoveTo(placement, ref CurrentMoves);


                        if (placement.GetFirstUnit() != null && placement.GetSecondUnit() != null)
                            OffsetUnits(placement.GetFirstUnit(), placement.GetSecondUnit());
                    }

                    break;
                case 1:
                    {
                        BoardPlacement placement = map.placements[unit.CurrentPlacement.i + 1, unit.CurrentPlacement.j];
                        unit.MoveTo(placement, ref CurrentMoves);

                        if (placement.GetFirstUnit() != null && placement.GetSecondUnit() != null)
                            OffsetUnits(placement.GetFirstUnit(), placement.GetSecondUnit());
                    }
                    break;
                case 2:
                    {
                        BoardPlacement placement = map.placements[unit.CurrentPlacement.i, unit.CurrentPlacement.j - 1];
                        unit.MoveTo(placement, ref CurrentMoves);


                        if (placement.GetFirstUnit() != null && placement.GetSecondUnit() != null)
                            OffsetUnits(placement.GetFirstUnit(), placement.GetSecondUnit());
                    }
                    break;
                case 3:
                    {
                        BoardPlacement placement = map.placements[unit.CurrentPlacement.i, unit.CurrentPlacement.j + 1];
                        unit.MoveTo(placement, ref CurrentMoves);

                        if (placement.GetFirstUnit() != null && placement.GetSecondUnit() != null)
                            OffsetUnits(placement.GetFirstUnit(), placement.GetSecondUnit());
                    }
                    break;
                case 4:
                    unit.Attack();
                    break;
                case 5:
                    if (unit.CurrentPlacement.item is HPPotion)
                    {
                        unit.Heal();
                    }
                    else
                    {
                        unit.EquipItem();
                    }
                    break;
            }
            InputData.SetGameObjectsInstance();
        }
        
        AgentManager.Get().SwapTurns();
    }

    int GetMaxIndex(double[] arr)
    {
        int index = 0;
        double v = double.MinValue;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] > v)
            {
                index = i;
                v = arr[i];
            }
        }

        return index;
    }

    void Start()
    {
        InputData = GameObject.Find("NNInputGetter").GetComponent<InputData>();
        //nn = new NN();        
        //nn.Forward();

        //int index = GetMaxIndex(nn.outputLayer.answers);
        //Debug.Log($"Pick: idx = {index}  val={nn.outputLayer.answers[index]}");

        
        
    }

    
    void Update()
    {
       //int i = GetMaxIndex(nn.outputLayer.answers);
       //chioce = i;
    }

}
