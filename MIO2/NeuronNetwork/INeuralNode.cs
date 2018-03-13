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
        List<Dendrite> InDendrites { get; }
        List<Dendrite> OutDendrites { get; }
        double PartialError { get; set; }
        double HiddenSum { get; }


        void AddOutDendriteTo(List<INeuralNode> nodes);
        void AddInDendrite(Dendrite dendrite);
        void EvaluateNodeValue();
    }
}
