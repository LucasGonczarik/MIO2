using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIO2.NeuronNetwork
{
    class Input : INeuralNode
    {
        private double _value;

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public double HiddenSum => _value;

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
            this._value = value;
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
            throw new InvalidOperationException();
        }

        public void EvaluateNodeValue()
        {
        }
    }
}