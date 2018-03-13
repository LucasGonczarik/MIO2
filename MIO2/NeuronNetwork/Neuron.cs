using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using MIO2.FileOperations;

namespace MIO2.NeuronNetwork
{
    class Neuron : INeuralNode
    {
        private static int _counter;
        private int _id;

        public Layer OwnLayer { get; }

        public double Value { get; private set; }
        public double PartialError { get; set; }
        public double HiddenSum { get; private set; }


        public List<Dendrite> InDendrites { get; }
        public List<Dendrite> OutDendrites { get; }


        public Neuron(Layer layer)
        {
            this._id = _counter += 1;

            this.InDendrites = new List<Dendrite>();
            this.OutDendrites = new List<Dendrite>();

            this.OwnLayer = layer;
        }

        public void AddOutDendriteTo(List<INeuralNode> nextNodes)
        {
            foreach (var nextNode in nextNodes)
            {
                var dendrite = new Dendrite(this, nextNode);
                OutDendrites.Add(dendrite);
                nextNode.AddInDendrite(dendrite);
            }
        }

        public void AddInDendrite(Dendrite dendrite)
        {
            this.InDendrites.Add(dendrite);
        }


        public void EvaluateNodeValue()
        {
            this.Value = Network.Sigmoid(InDendrites.Sum(dendrite => dendrite.Weight * dendrite.PreviousLayerNeuron.Value));
            this.HiddenSum = InDendrites.Sum(dendrite => dendrite.Weight * dendrite.PreviousLayerNeuron.Value);
        }

        public override string ToString()
        {
            return "Neuron: " + _id + " : " + Value;
        }
    }
}