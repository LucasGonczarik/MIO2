using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIO2
{
    class NeuronLayer
    {
        private readonly List<Neurone> _neurones;

        public NeuronLayer(int sizeOfLayer)
        {
            _neurones = new List<Neurone>();
            for (var index = 0; index < sizeOfLayer; index++)
            {
                _neurones.Add(new Neurone());
            }
        }

        public List<double> runPerception(params double[] inputs)
        {
            List<double> results = new List<double>();
            foreach (var neurone in _neurones)
            {
                results.Add(neurone.CalculateScore());
            }
        }
    }
}
