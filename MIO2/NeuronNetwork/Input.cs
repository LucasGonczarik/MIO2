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
        public List<Dendrite> Dendrites { get; }


        public Input()
        {
            Dendrites = new List<Dendrite>();
        }

        public Input(double value) : this()
        {
            Value = value;
        }

        public void AddDendriteTo(List<INeuralNode> nodes)
        {
            throw new NotImplementedException();
        }

        public void EvaluateNodeValue()
        {
        }
    }
}
