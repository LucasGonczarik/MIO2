using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIO2.NeuronNetwork
{
    class Input : INeuralNode
    {
        public double Value { get; set; }
        public double PartialError { get; set; }
        public List<Dendrite> InDendrites { get; }
        public List<Dendrite> OutDendrites { get; }


        public Input()
        {
            InDendrites = new List<Dendrite>();
            OutDendrites = new List<Dendrite>();
        }

        public Input(double value) : this()
        {
            Value = value;
        }

        public void AddDendriteTo(List<INeuralNode> nodes)
        {
            throw new NotImplementedException();
        }

        public void AddOutgoingDendrite(Dendrite dendrite)
        {
            throw new NotImplementedException();
        }

        public void EvaluateNodeValue()
        {
        }
    }
}
