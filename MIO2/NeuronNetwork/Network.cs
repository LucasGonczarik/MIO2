using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MIO2.FileOperations;
using PerceptonMIO.FileOperations;

namespace MIO2.NeuronNetwork
{
    class Network
    {
        private const int SafeCount = 10000; 
        private const double ActivationThreshold = 0.5;
        private const double LearningRate = 2;
        public List<Layer> NeuronLayers { get; }
        public Layer InputLayer { get; }

        public Network()
        {
            InputLayer = new Layer(this, new Input(), new Input());
            NeuronLayers = new List<Layer> { InputLayer, new Layer(this, 3), new Layer(this, 2), new Layer(this, 1)};
            for (var indexOfLayer = 0; indexOfLayer < NeuronLayers.Count - 1; indexOfLayer++)
            {
                CreateOutDendritesBeetwen(NeuronLayers[indexOfLayer], NeuronLayers[indexOfLayer + 1]);
            }
        }

        private static void CreateOutDendritesBeetwen(Layer actualLayer, Layer nextLayer)
        {
            foreach (var actualNode in actualLayer.NodesList)
            {
                actualNode.AddOutDendriteTo(nextLayer.NodesList);
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
            var recordIndex = 0;
            for (var counter = 0; counter < SafeCount && recordIndex < records.Count; counter++)
            {
                //reset network inputs
                var inputs = GetOnlyRecordInputs(records[recordIndex]);
                var exepctedOutput = records[recordIndex].Last();
                recordIndex++;


                var output = RunPredictionForRecord(inputs);
                var modulatedOutput = Sigmoid(output);
                var predictionDifference = exepctedOutput - modulatedOutput;

                ValueType t = 0;
                //recalculate partial errors for each node
                if (Math.Abs(predictionDifference) > ActivationThreshold)
                {
                    CalculatePartialError(predictionDifference);
                    ModifyWeight();
                    //reset record loop
                    recordIndex = 0;
                }
                Console.WriteLine(string.Join(" ", InputLayer.NodesList.Select(node => node.Value)) + " : " + exepctedOutput + " / " + modulatedOutput);
            }
        }

        private double RunPredictionForRecord(IReadOnlyList<double> inputs)
        {
            var inputsNodesList = InputLayer.NodesList.Cast<Input>().ToList();
            UpdateInputNodes(inputsNodesList, inputs);

            foreach (var neuronLayer in NeuronLayers)
            {
                neuronLayer.EvaluateLayerNodes();
            }

            //get value of last neuron MUST BE ONLY ONE IN LAST LAYER
            var output = NeuronLayers.Last().NodesList[0].Value;
            return output;
        }

        private void CalculatePartialError(double predictionDifference)
        {
            NeuronLayers.Last().NodesList[0].PartialError = predictionDifference;
            for (var index = NeuronLayers.Count - 2; index >= 0; index--)
            {
                var neuronLayer = NeuronLayers[index];
                foreach (var neuralNode in neuronLayer.NodesList)
                {
                    neuralNode.PartialError = neuralNode.OutDendrites.Sum(dendrite =>
                        dendrite.Weight * dendrite.ActualNeuron.PartialError);
                }
            }
        }

        private void ModifyWeight()
        {
            foreach (var neuronLayer in NeuronLayers)
            {
                //we're looking backward, so we're skiping 0 layer -> inputs
                for (var index = 0; index < neuronLayer.NodesList.Count; index++)
                {
                    var neuralNode = neuronLayer.NodesList[index];
                    foreach (var nodeInDendrite in neuralNode.InDendrites)
                    {
                        nodeInDendrite.Weight += LearningRate * neuralNode.PartialError *
                                                 SigmoidDeritive(neuralNode.HiddenSum) *
                                                 nodeInDendrite.PreviousLayerNeuron.Value;
                    }
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

        public static double Sigmoid(double value)
        {
            return 1.0d / (1.0d + Math.Exp(-value));
        }

        private static double SigmoidDeritive(double x)
        {
            return Math.Pow(Math.Exp(-x) / (1 + Math.Exp(-x)), 2);
        }
    }
}