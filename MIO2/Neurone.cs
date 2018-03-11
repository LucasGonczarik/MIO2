using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using MIO2.FileOperations;
using PerceptonMIO.FileOperations;

namespace MIO2
{
    class Neurone
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Neurone));

        readonly DataParser _dataParser = new DataParser();

        private readonly List<double> _scales;
        private readonly double _activationThreshold;
        private readonly double _scanesChangeParameter;
        private int MAX_NUMBER_OF_ITERATIONS = 100;


        public Neurone()
        {
            this._scales = GetFilledList(1, 2);
            this._activationThreshold = 0.5d;
            this._scanesChangeParameter = 0.1d;
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

        private bool LearnFromRedord(double[] record, double expectedOutput)
        {
            var precitedCorrectly = true;
            var output = Predict(record);
            var predictionDifference = expectedOutput - output;

            if (DoubleIsNotZero(predictionDifference))
            {
                RecalculateScales(record, predictionDifference);
                precitedCorrectly = false;
            }

            _dataParser.CopyScalesToBuffor(_scales);
            return precitedCorrectly;
        }

        private double Predict(double[] record)
        {
            return Convert.ToDouble(CalculateScore(record) > _activationThreshold);
        }

        private void CopyScalesToBuffor(List<double> scales)
        {
            throw new NotImplementedException();
        }

        private void RecalculateScales(IReadOnlyList<double> record, double predictionDifference)
        {
            for (var scaleIndex = 0; scaleIndex < _scales.Count; scaleIndex++)
            {
                _scales[scaleIndex] += _scanesChangeParameter * record[scaleIndex] * predictionDifference;
            }
        }

        public double CalculateScore(params double[] record)
        {
            return Sigmoid(_scales.Select((t, scaleIndex) => t * record[scaleIndex]).Sum());
        }

        public static double Sigmoid(double value)
        {
            return 1.0d / (1.0d + Math.Exp(-value));
        }

        public static List<double> GetFilledList(int value, int size)
        {
            var result = new double[size];
            for (var index = 0; index < size; index++)
            {
                result[index] = value;
            }

            return result.ToList();
        }

        private static bool DoubleIsNotZero(double value)
        {
            return Math.Abs(value) > double.Epsilon;
        }
    }
}