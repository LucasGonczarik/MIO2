using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MIO2.FileOperations;

namespace MIO2.NeuronNetwork
{
    class Neuron : INeuralNode
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Neuron));

        public DataParser Parser { get; } = new DataParser();

        public Layer OwnLayer { get; }

        public double ActivationThreshold { get; }
        public double ScalesChangeParameter { get; }

        public List<Dendrite> OutDendrites;

        public double Value { get; private set; }
        public List<Dendrite> Dendrites { get; }

        public Neuron(Layer layer)
        {
            this.Dendrites = new List<Dendrite>();

            this.OwnLayer = layer;
            this.ActivationThreshold = 0.5d;
            this.ScalesChangeParameter = 0.1d;
        }

        /// <param name="previousNodes">Nodes from previous layer</param>
        public void AddDendriteTo(List<INeuralNode> previousNodes)
        {
            foreach (var previousNode in previousNodes)
            {
                Dendrite dendrite = new Dendrite(previousNode, this);
                Dendrites.Add(dendrite);
            }
        }

        public void EvaluateNodeValue()
        {
            this.Value = OutDendrites.Sum(dendrite => dendrite.Weight * dendrite.PreviousNeuron.Value);
        }

        private void CopyScalesToBuffor(List<double> scales)
        {
            throw new NotImplementedException();
        }

        private void RecalculateScales(IReadOnlyList<double> record, double predictionDifference)
        {
            //todo for each dendrite change value by passed error (delta)
            throw new NotImplementedException();
            //            for (var scaleIndex = 0; scaleIndex < _scales.Count; scaleIndex++)
            //            {
            //                _scales[scaleIndex] += ScalesChangeParameter * record[scaleIndex] * predictionDifference;
            //            }
        }

        public double CalculateScore()
        {
            double sum = 0;
            foreach (var dendrite in Dendrites)
            {
                double dendriteWeight = dendrite.Weight;
                double value = dendrite.PreviousNeuron.Value;
                sum += dendriteWeight * value;
            }

            return Sigmoid(sum);
        }

        public static double Sigmoid(double value)
        {
            return 1.0d / (1.0d + Math.Exp(-value));
        }

        private static bool DoubleIsNotZero(double value)
        {
            return Math.Abs(value) > double.Epsilon;
        }

        //        private void LearnFromRedords(IReadOnlyList<double[]> records)
        //        {
        //            var counter = 0;
        //            for (var recordIndex = 0; recordIndex < records.Count && counter < MAX_NUMBER_OF_ITERATIONS; recordIndex++)
        //            {
        //                var record = records[recordIndex];
        //                var exepctedOutput = record.Last();
        //
        //                var output = Predict(record);
        //
        //                var predictionDifference = exepctedOutput - output;
        //                if (DoubleIsNotZero(predictionDifference))
        //                {
        //                    RecalculateScales(record, predictionDifference);
        //                    recordIndex = 0;
        //                }
        //
        //                _dataParser.CopyScalesToBuffor(_scales);
        //            }
        //
        //            _dataParser.SaveBufferedScalesToCsv();
        //        }

        //        private bool LearnFromRedord(double[] record, double expectedOutput)
        //        {
        //            var precitedCorrectly = true;
        //            var output = Predict(record);
        //            var predictionDifference = expectedOutput - output;
        //
        //            if (DoubleIsNotZero(predictionDifference))
        //            {
        //                RecalculateScales(record, predictionDifference);
        //                precitedCorrectly = false;
        //            }
        //
        //            Parser.CopyScalesToBuffor(_scales);
        //            return precitedCorrectly;
        //        }

        //        private double Predict(double[] record)
        //        {
        //            return Convert.ToDouble(CalculateScore(record) > ActivationThreshold);
        //        }
    }
}