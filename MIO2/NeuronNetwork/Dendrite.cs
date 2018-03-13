using System;

namespace MIO2.NeuronNetwork
{
    class Dendrite
    {
        private static int _counter;
        private static readonly Random Random = new Random();
        private readonly int _id;

        public double Weight { get; set; }

        public INeuralNode ActualNeuron { get; }

        public INeuralNode PreviousLayerNeuron { get; }

        public Dendrite(INeuralNode previousNeuron, INeuralNode actualNeuron)
        {
            this._id = _counter++;
//            Weight = Dendrite.Random.NextDouble() * (1.0 - 0.00000001) + 0.00000001;
            Weight = Dendrite.Random.NextDouble() * (1.0 - 0.1) + 0.1;
            PreviousLayerNeuron = previousNeuron;
            ActualNeuron = actualNeuron;
        }

        public override string ToString()
        {
            return "Dendrite: " + _id + ", weight: " + Weight;
        }
    }
}