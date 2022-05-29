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
        for (int i = 0; i < inputs.Length; i++) {
            ans += weights[i] * inputs[i];
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
    public const int neruonCount = 5 + 5 + 4 + 4 + 6;
    public double[] inputLayer = new double[neruonCount];
    public Layer layer1 = new Layer(neruonCount, neruonCount);

    public const int outputCount = 4 + 2;
    public Layer outputLayer = new Layer(outputCount, neruonCount);

    public NN()
    {
        for (int i = 0; i < inputLayer.Length; i++)
        {
            inputLayer[i] = UnityEngine.Random.Range(-1.0f, 1.0f);
        }

        layer1.InitializeRandomWeights(-1.0, 1.0);
        outputLayer.InitializeRandomWeights(-1.0, 1.0);        
    }

    public void Forward()
    {
        layer1.Forward(inputLayer);
        outputLayer.Forward(layer1.answers);
    }
}


public class NNAgent : Agent
{
    public NN nn;

    public override void Action(Map map)
    {
        
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
        nn = new NN();        
        nn.Forward();

        int index = GetMaxIndex(nn.outputLayer.answers);
        Debug.Log($"Pick: idx = {index}  val={nn.outputLayer.answers[index]}");

        for (int i = 0; i < nn.layer1.neurons.Length; i++) {
            for (int j = 0; j < nn.layer1.neurons.Length; j++) {
                double d = nn.layer1.neurons[i].weights[j];
                //Debug.Log($"Neuron: {i} weight: {d}");
            }
        }
        
    }

    
    void Update()
    {
        
    }
}
