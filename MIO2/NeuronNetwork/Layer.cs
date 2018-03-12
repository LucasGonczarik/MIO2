using System.Collections.Generic;
using System.Linq;

namespace MIO2.NeuronNetwork
{
    class Layer
    {
        public List<INeuralNode> NodesList { get; }
        public Network OwnNetwork { get; }

        public Layer(Network network, int sizeOfLayer)
        {
            this.OwnNetwork = network;

            this.NodesList = new List<INeuralNode>();
            for (var index = 0; index < sizeOfLayer; index++)
            {
                this.NodesList.Add(new Neuron(this));
            }
        }

        public Layer(Network network, params Input[] inputs)
        {
            this.OwnNetwork = network;
            this.NodesList = new List<INeuralNode>(inputs.ToList());
        }

        //        public List<double> runPerception(params double[] inputs)
        //        {
        //            List<double> results = new List<double>();
        //            foreach (var neurone in Neurones)
        //            {
        //                results.Add(neurone.CalculateScore());
        //            }
        //        }
        public void EvaluateLayerNodes()
        {
            foreach (var neuralNode in NodesList)
            {
                neuralNode.EvaluateNodeValue();
            }
        }
    }
}