using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POPTVR.Entities
{
    class DataSet
    {
        private double[,] inputdata;
        private double[,] desiredOutputs;
        private int numberOfInputNodes;
        private int numberOfOutputNodes;
        private int totalNumberOfRecords;

        public DataSet(double[,] inputdata, double[,] desiredOutputs)
        {
            this.inputdata = inputdata;
            this.numberOfInputNodes = this.inputdata.GetLength(1);

            this.desiredOutputs = desiredOutputs;
            this.numberOfOutputNodes = this.desiredOutputs.GetLength(1);

            this.totalNumberOfRecords = this.inputdata.GetLength(0);
        }

        public double[,] Inputdata
        {
            get
            {
                return this.inputdata;
            }
        }

        public double[,] DesiredOutputs
        {
            get
            {
                return this.desiredOutputs;
            }
        }

        public int NumberOfInputNodes
        {
            get
            {
                return this.numberOfInputNodes;
            }
        }

        public int NumberOfOutputNodes
        {
            get
            {
                return this.numberOfOutputNodes;
            }
        }

        public int TotalNumberOfRecords
        {
            get
            {
                return this.totalNumberOfRecords;
            }
        }

        // inclusive "from", exclusive "to"
        public DataSet subset(int from, int to)
        {
            double[,] inputdata = new double[to - from, this.numberOfInputNodes];
            double[,] desiredOutputs = new double [to - from, this.numberOfOutputNodes];

            for (int i = 0; i < to - from; i++)
            {
                for (int j = 0; j < this.numberOfInputNodes; j++)
                {
                    inputdata[i,j] = this.inputdata[i + from,j];
                }

                for (int j = 0; j < this.numberOfOutputNodes; j++)
                {
                    desiredOutputs[i, j] = this.desiredOutputs[i + from, j];
                }
            }

            DataSet result = new DataSet(inputdata, desiredOutputs);

            return result;
        }

        public override string ToString()
        {
            string result = this.totalNumberOfRecords + "\n";
            result += this.numberOfInputNodes + "\t" + this.numberOfOutputNodes + "\t";

            for (int i = 0; i < this.totalNumberOfRecords; i++)
            {
                for (int j = 0; j < this.numberOfInputNodes; j++)
                {
                    result += inputdata[i, j] + "\t";
                }

                for (int j = 0; j < this.numberOfOutputNodes; j++)
                {
                    result += desiredOutputs[i, j] + "\t";
                }

                result += "\n";
            }


            return result;
        }
    }
}
