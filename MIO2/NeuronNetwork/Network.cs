using System;
using System.Collections.Generic;
using System.Linq;
using MIO2.FileOperations;
using PerceptonMIO.FileOperations;

namespace MIO2.NeuronNetwork
{
    class Network
    {
        public List<Layer> NeuronLayers { get; }
        public Layer InputLayer { get; }

        public Network()
        {
            NeuronLayers = new List<Layer> {new Layer(this, 2), new Layer(this, 1)};
            InputLayer = new Layer(this, new Input(), new Input());
            CreateDendritesBeetwen(InputLayer, NeuronLayers[0]);
            for (var indexOfLayer = 0; indexOfLayer < NeuronLayers.Count - 1; indexOfLayer++)
            {
                CreateDendritesBeetwen(NeuronLayers[indexOfLayer], NeuronLayers[indexOfLayer + 1]);
            }
        }

        private static void CreateDendritesBeetwen(Layer preciousLayer, Layer actualLayer)
        {
            foreach (var actualNeuralNode in actualLayer.NodesList)
            {
                actualNeuralNode.AddDendriteTo(preciousLayer.NodesList);
            }
        }

        public void RunLearningTask()
        {
            var filePath = FileSelectDialogHandler.GetFilePath();
            if (filePath == null) return;
            var recordsStrings = DataParser.ParseDataCsv(filePath);
            var records = DataParser.ConvertStringRecordsToDoubles(recordsStrings);
            LearnFromRedords(records);
        }

        private void LearnFromRedords(IReadOnlyList<double[]> records)
        {
            for (var recordIndex = 0; recordIndex < records.Count; recordIndex++)
            {
                //reset network inputs
                var inputs = GetOnlyRecordInputs(records[recordIndex]);
                var inputsNodesList = InputLayer.NodesList.Cast<Input>().ToList();
                UpdateInputNodes(inputsNodesList, inputs);

                foreach (var neuronLayer in NeuronLayers)
                {
                    //recalculate value from first layer to nth layer
                    neuronLayer.EvaluateLayerNodes();
                }

                //get value of last neuron MUST BE ONLY ONE IN LAST LAYER
                var output = NeuronLayers.Last().NodesList[0].Value;
                var exepctedOutput = inputs.Last();

                var predictionDifference = exepctedOutput - output;
                if (Math.Abs(predictionDifference) > double.Epsilon)
                {
                    Layer lastLayer = NeuronLayers.Last();
                    Neuron lastNeuron = (Neuron) lastLayer.NodesList[0];
                    double deltaOutputSum = SigmoidDeritive(lastNeuron.HiddenSum) * predictionDifference;
                    foreach (var dendrite in lastNeuron.InDendrites)
                    {
                        var weightChange = deltaOutputSum;
                        dendrite.PertialError = deltaOutputSum * dendrite.Weight;
                        dendrite.Weight += weightChange / dendrite.PreviousLayerNeuron.Value;
                    }

                    Layer previousLayer = NeuronLayers[NeuronLayers.Count - 2];
                    foreach (var neuralNode in previousLayer.NodesList)
                    {
                        foreach (var dendrite in neuralNode.InDendrites)
                        {
                            var weightChange = neuralNode.OutDendrites.Sum(dendrite1 => dendrite1.PertialError) * SigmoidDeritive(neuralNode.Value);
                            //zmiana ew kolejnych
                            dendrite.PertialError = deltaOutputSum * dendrite.Weight;
                            dendrite.Weight += weightChange / dendrite.PreviousLayerNeuron.Value;
                        }
                    }


//                    for (var layerIndex = NeuronLayers.Count - 1; layerIndex >= 0; layerIndex--)
//                    {
//                        Layer actualLayer = NeuronLayers[layerIndex];
//
//                        //recalculate score for layer
//
//                    }
                    //run backward propagation
                    recordIndex = 0;
                }
            }
        }

        private static void UpdateInputNodes(List<Input> inputsNodesList, IReadOnlyList<double> inputs)
        {
            if (inputsNodesList.Count != inputs.Count)
                throw new ArgumentException("Number of inputs is different than number of input nodes");
            for (var index = 0; index < inputsNodesList.Count; index++)
            {
                inputsNodesList[index].Value = inputs[index];
            }
        }

        private static T[] GetOnlyRecordInputs<T>(T[] data)
        {
            return SubArray(data, 0, data.Length - 1);
        }

        private static T[] SubArray<T>(T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        private static double SigmoidDeritive(double x)
        {
            return Math.Pow(Math.Exp(-x) / (1 + Math.Exp(-x)), 2);
        }
    }
}