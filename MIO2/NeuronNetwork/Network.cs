using System;
using System.Collections.Generic;
using System.Linq;
using MIO2.FileOperations;
using PerceptonMIO.FileOperations;

namespace MIO2.NeuronNetwork
{
    class Network
    {
        private const int Epoch = 1000;
        private const double LearningRate = 0.2;

        public List<Layer> NeuronLayers { get; }
        public Layer InputLayer { get; }

        public Network()
        {
            InputLayer = new Layer(this, new Input(), new Input());
            NeuronLayers = new List<Layer> {InputLayer, new Layer(this, 2), new Layer(this, 1)};
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
            var numberOfLearningIterations = records.Count * Epoch;
            var resultsDifference = new List<double>();

            for (var counter = 0; counter < numberOfLearningIterations; counter++)
            {
                var recordIndex = counter % records.Count;
                var inputs = GetOnlyRecordInputs(records[recordIndex]);
                var exepctedOutput = records[recordIndex].Last();

                var output = RunPredictionForRecord(inputs);
                var modulatedOutput = Sigmoid(output);
                var predictionDifference = exepctedOutput - modulatedOutput;

                if (Math.Abs(predictionDifference) >= double.Epsilon)
                {
                    CalculatePartialError(predictionDifference);
                    ModifyWeight();
                }

                resultsDifference.Add(predictionDifference);
                Console.WriteLine(@"{0} {1} : {2}", string.Join(string.Empty, InputLayer.NodesList.Select(node => node.Value)),
                    exepctedOutput, predictionDifference);
            }

            DataParser.SaveListToCsv(resultsDifference);
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
            //based on http://galaxy.agh.edu.pl/~vlsi/AI/backp_t/backprop.html
            foreach (var neuronLayer in NeuronLayers)
            {
                foreach (var neuralNode in neuronLayer.NodesList)
                {
                    foreach (var nodeInDendrite in neuralNode.InDendrites)
                    {
                        nodeInDendrite.Weight += LearningRate *
                                                 neuralNode.PartialError *
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
            var sigmoidValue = Sigmoid(x);
            return sigmoidValue * (1 - sigmoidValue);
//            return Math.Pow(Math.Exp(-x) / (1 + Math.Exp(-x)), 2);
        }
    }
}