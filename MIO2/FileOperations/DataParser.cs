using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace MIO2.FileOperations
{
    class DataParser
    {
        List<List<double>> _scalesBuffer = new List<List<double>>();
        private static string _csv_delimiter = ",";
        private const string FileName = "results.csv";

        public static List<string[]> ParseDataCsv(string filePath)
        {
            var resuls = new List<string[]>();
            using (var parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(_csv_delimiter);
                while (!parser.EndOfData)
                {
                    resuls.Add(parser.ReadFields());
                }
            }

            return resuls;
        }

        public static List<double[]> ConvertStringRecordsToDoubles(List<string[]> stringRecords)
        {
            var resultList = new List<double[]>();
            foreach (var stringRecord in stringRecords)
            {
                resultList.Add(Array.ConvertAll(stringRecord, Double.Parse));
            }

            return resultList;
        }

        public void CopyScalesToBuffor(List<double> scales)
        {
            _scalesBuffer.Add(new List<double>(scales));
        }

        public void SaveBufferedScalesToCsv()
        {
            var csv = new StringBuilder();

            foreach (var record in _scalesBuffer)
            {
                csv.AppendLine(ParseRecordToCsvString(record));
            }

            File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + FileName, csv.ToString());
            _scalesBuffer = new List<List<double>>();
        }

        private static string ParseRecordToCsvString(IEnumerable<double> record)
        {
            return string.Join(_csv_delimiter, record);
        }
    }
}