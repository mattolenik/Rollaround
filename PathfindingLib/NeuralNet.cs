﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PathfindingLib
{
    public class NeuralNet
    {
        int hiddenLayers = 2;
        int bias = -1;
        double activationResponse = 1;

        readonly List<NeuronLayer> layers;

        public NeuralNet(int numInputs, int numOutputs, int numHiddenLayers, int neuronsPerHiddenLayer)
        {
            hiddenLayers = numHiddenLayers;
            layers = new List<NeuronLayer>();
            if (hiddenLayers > 0)
            {
                layers.Add(new NeuronLayer(neuronsPerHiddenLayer, numInputs));
                for (var i = 0; i < hiddenLayers - 1; i++)
                {
                    layers.Add(new NeuronLayer(neuronsPerHiddenLayer, neuronsPerHiddenLayer));
                }
                layers.Add(new NeuronLayer(numOutputs, neuronsPerHiddenLayer));
            }
            else
            {
                layers.Add(new NeuronLayer(numOutputs, numInputs));
            }
        }

        public List<double> GetWeights()
        {
            var weights = new List<double>();

            foreach (var layer in layers)
            {
                foreach (var neuron in layer.Neurons)
                {
                    weights.AddRange(neuron.Weights);
                }
            }

            return weights;
        }

        public void PutWeights(List<double> inputs)
        {
            var weight = 0;
            foreach (var layer in layers)
            {
                foreach (var neuron in layer.Neurons)
                {
                    for (var i = 0; i < neuron.Weights.Count; i++)
                    {
                        neuron.Weights[i] = inputs[weight++];
                    }
                }
            }
        }

        public List<double> FeedForward(List<double> inputs)
        {
            var outputs = new List<double>();

            for (var i = 0; i < hiddenLayers + 1; i++)
            {
                var layer = layers[i];
                if (i > 0)
                {
                    inputs = outputs.ToList();
                }
                outputs.Clear();

                foreach (var neuron in layer.Neurons)
                {
                    var netinput = 0.0;

                    for (var k = 0; k < neuron.Weights.Count - 1; k++)
                    {
                        netinput += neuron.Weights[k] * inputs[k];
                    }
                    netinput += neuron.Weights[neuron.Weights.Count - 1] * bias;
                    outputs.Add(Sigmoid(netinput, activationResponse));
                }
            }
            return outputs;
        }

        public double Sigmoid(double netinput, double response)
        {
            return 1 / (1 + Math.Exp(-netinput / response));
        }
    }
}