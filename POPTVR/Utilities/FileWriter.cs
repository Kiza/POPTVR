using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.PoptvrArchitecture;

namespace POPTVR.Utilities
{
    public class FileWriter
    {
        public static void WriteToFile(string fileName, string outputData)
        {
            outputData = outputData.Replace("\n", "\r\n");
            System.IO.File.WriteAllText(@fileName, outputData);
        }

        public static void WritePopLearnOutput(PoptvrModel poptvr)
        {
            WritePopLearnOutput("", poptvr);
        }

        public static void WritePopLearnOutput(string filename, PoptvrModel poptvr)
        {

            Console.Write("Output InitialWeights.txt ... ");
            FileWriter.WriteToFile(AppConfig.getOutputFolder() + filename + "InitialWeights.txt", poptvr.InitialWeightsString);
            Console.WriteLine("Done!");

            Console.Write("Output SelectedWeights.txt ... ");
            FileWriter.WriteToFile(AppConfig.getOutputFolder() + filename + "SelectedWeights.txt", poptvr.SelectedWeightsString);
            Console.WriteLine("Done!");

            Console.Write("Output FuzzyRulesString.txt ... ");
            FileWriter.WriteToFile(AppConfig.getOutputFolder() + filename + "FuzzyRulesString.txt", poptvr.FuzzyRulesString);
            Console.WriteLine("Done!");
        }

        public static void WritePopTestOutput(string outputData)
        {
            WritePopTestOutput("", outputData);
        }

        public static void WritePopTestOutput(string filename, string outputData)
        {
            Console.Write("Output TestOutput.txt ... ");
            FileWriter.WriteToFile(AppConfig.getOutputFolder() + filename + "TestOutput.txt", outputData);
            Console.WriteLine("Done!");
        }

        public static void WriteDoubleArray(string filename, double[,] data)
        {
            string res = "";
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    res += ("[" + i + ", " + j + "] = ");
                    res += (data[i, j].ToString() + "\t\t");
                }

                res += "\n";
            }
            FileWriter.WriteToFile(AppConfig.getOutputFolder() + filename, res);

        }
    }
}
