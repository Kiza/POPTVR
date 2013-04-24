using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POPTVR.Utilities
{
    class PrintToScreen
    {

        public static string stringrize(double [] data)
        {
            string result = "";
            for (int i = 0; i < data.Length; i++)
            {
                result += data[i] + "\n";
            }

            return result;
        }


        public static void print(string title, double[,] data)
        {
            Console.WriteLine(title);
            Console.WriteLine(stringrize(data));
        }
        public static string stringrize(double[,] data)
        {
            string result = "";
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); i ++ )
                {
                    result += data[i, j] + "\t";
                }

                result += "\n";
            }

            return result;
        }

        public static string stringrize(double[, ,] data)
        {
            string result = "";

            return result;
        }

    }
}
