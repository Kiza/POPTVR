using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POPTVR.Utilities
{
    class SystemFunctions
    {
        public static double[] getArrayAtRow(double[,] boxArray, int row)
        {
            double[] result = new double[boxArray.GetLength(1)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = boxArray[row, i];
            }

            return result;
        }
    }
}
