using System;

namespace MIO2.NeuronNetwork
{
    class Dendrite
    {
        public double Weight { get; }

        public INeuralNode ActualNeuron { get; }

        public INeuralNode PreviousNeuron { get; }

        public Dendrite(INeuralNode previousNeuron, INeuralNode actualNeuron)
        {
            var random = new Random();
            Weight = random.NextDouble() * (0.00000001 - 1.0) + 0.00000001;
            PreviousNeuron = previousNeuron;
            ActualNeuron = actualNeuron;
        }
    }
}