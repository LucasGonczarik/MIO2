using System;

namespace MIO2.NeuronNetwork
{
    class Dendrite
    {
        public double Weight { get; set; }

        public INeuralNode ActualNeuron { get; }

        public INeuralNode PreviousLayerNeuron { get; }
        public double PertialError { get; set; }

        public Dendrite(INeuralNode previousNeuron, INeuralNode actualNeuron)
        {
            var random = new Random();
            Weight = random.NextDouble() * (0.00000001 - 1.0) + 0.00000001;
            PreviousLayerNeuron = previousNeuron;
            ActualNeuron = actualNeuron;
        }
    }
}