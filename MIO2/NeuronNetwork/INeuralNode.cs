using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIO2.NeuronNetwork
{
    interface INeuralNode
    {
        double Value { get; }
        List<Dendrite> Dendrites { get; }

        void AddDendriteTo(List<INeuralNode> nodes);
        void EvaluateNodeValue();
    }
}
