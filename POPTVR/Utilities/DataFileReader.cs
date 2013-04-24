using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using POPTVR.Entities;

namespace POPTVR.Utilities
{
    class DataFileReader
    {
        public static DataSet ReadData(string fileName)
        {
            TextReader textReader = new StreamReader(@fileName);

            string temp = textReader.ReadLine().Trim();
            int total = int.Parse(temp);

            temp = textReader.ReadLine().Trim();
            int[] tempIntArray = stringToIntArray(temp, new char[] { '\t', ' ' });

            int numberOfInputNodes = tempIntArray[0];
            int numberOfOutputNodes = tempIntArray[1];

            double [,] inputdata = new double[total, numberOfInputNodes];
            double [,] desiredOutputs = new double [total, numberOfOutputNodes];

            for (int i = 0; i < total; i++)
            {
                temp = textReader.ReadLine().Trim();
                double[] tempDoubleArray = stringToDoubleArray(temp, new char[] {'\t', ' ' });
                for (int j = 0; j < numberOfInputNodes; j++)
                {
                    inputdata[i, j] = tempDoubleArray[j];
                }

                for (int j = 0; j < numberOfOutputNodes; j++)
                {
                    desiredOutputs[i, j] = tempDoubleArray[numberOfInputNodes + j];
                }
            }

            DataSet result = new DataSet(inputdata, desiredOutputs);

            return result;
        }

        public static double[] stringToDoubleArray(string input, char[] separator)
        {
            return stringToDoubleArray(input, separator, System.StringSplitOptions.RemoveEmptyEntries);
        }
        public static double[] stringToDoubleArray(string input, char[] separator, StringSplitOptions option)
        {
            string[] tempArray = input.Trim().Split(separator, option);
            double[] result = new double [tempArray.Length];
            for (int i = 0; i < tempArray.Length; i++)
            {
                result[i] = double.Parse(tempArray[i].Trim());
            }

            return result;
        }

        public static int[] stringToIntArray(string input, char[] separator)
        {
            return stringToIntArray(input, separator, System.StringSplitOptions.RemoveEmptyEntries);
        }
        public static int[] stringToIntArray(string input, char[] separator, StringSplitOptions option)
        {
            string[] tempArray = input.Trim().Split(separator, option);
            int[] result = new int[tempArray.Length];
            for (int i = 0; i < tempArray.Length; i++)
            {
                result[i] = int.Parse(tempArray[i].Trim());
            }

            return result;
        }
    }
}
