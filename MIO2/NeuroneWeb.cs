using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIO2.FileOperations;
using PerceptonMIO.FileOperations;

namespace MIO2
{
    class NeuroneWeb
    {
        private List<NeuronLayer> _neuronLayers;

        public NeuroneWeb()
        {
            _neuronLayers = new List<NeuronLayer> {new NeuronLayer(2), new NeuronLayer(1)};
        }

        public string RunPerceptionTaskAsync(params string[] perceptionParams)
        {
            var score = CalculateScore(Array.ConvertAll(perceptionParams, Double.Parse));
            return score > _activationThreshold ? "ladna" : "brzydka";
        }


        public void RunLearningTask()
        {
            var filePath = FileSelectDialogHandler.GetFilePath();
            if (filePath != null)
            {
                var recordsStrings = DataParser.ParseDataCsv(filePath);
                var records = DataParser.ConvertStringRecordsToDoubles(recordsStrings);
                LearnFromRedords(records);
            }
        }

        private void LearnFromRedords(IReadOnlyList<double[]> records)
        {
            for (var recordIndex = 0; recordIndex < records.Count; recordIndex++)
            {
                double[] inputs = GetOnlyRecordInputs(records[recordIndex]);
                double exepctedOutput = inputs.Last();

                foreach (var layer in _neuronLayers)
                {
                    inputs = layer.runPerception().ToArray();
                }

                // 

                var predictionDifference = exepctedOutput - inputs;
                if (/*last layer wrong guess*/)
                {
                    recordIndex = 0;
                }

                _dataParser.CopyScalesToBuffor(_scales);
            }

            _dataParser.SaveBufferedScalesToCsv();
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
    }
}
