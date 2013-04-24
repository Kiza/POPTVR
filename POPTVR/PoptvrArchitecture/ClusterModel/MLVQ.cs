using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.Utilities;

namespace POPTVR.PoptvrArchitecture.ClusterModel
{
    class MLVQ : ClusterInterface
    {
        private const double MAXERROR = 0.001;
        private const int MAXCYCLES = 50000;  //50000;
        private const double WIDTH_CONSTANT = 40.0;

        private int numberOfInputs;
        private int numberOfClusters;

        private double[] weights;
        private double[,] centroids;
        private double[,] widths;

        public MLVQ(int numberOfInputs, int numberOfClusters)
        {
            this.numberOfClusters = numberOfClusters;
            this.numberOfInputs = numberOfInputs;

            this.weights = new double[this.numberOfClusters];
            this.centroids = new double[this.numberOfClusters, this.numberOfInputs];
            this.widths = new double[this.numberOfClusters, this.numberOfInputs];

        }

        public double[,] Centroids
        {
            get
            {
                return this.centroids;
            }
        }
        public double[,] Widths
        {
            get
            {
                return this.widths;
            }
        }

        public void computeInitialCentroids(double[,] inputdata, double[,] outputdata, int count)
        {
            computeInitialCentroids(inputdata, count);
        }
        public void computeInitialCentroids(double[,] inputs, int count)
        {
            Console.WriteLine("Compute Initial Centroids ...");
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                double max = Double.MinValue;
                double min = Double.MaxValue;

                for (int j = 0; j < count; j++)
                {
                    if (max < inputs[j, i])
                        max = inputs[j, i];
                    if (min > inputs[j, i])
                        min = inputs[j, i];
                }

                // using MLVQ initial partition
                for (int j = 1; j <= this.numberOfClusters; j++)
                {
                    this.centroids[j - 1, i] = min + (((double)j + 0.5) / (double)this.numberOfClusters) * (max - min);
                }
            }

            Console.WriteLine("Initial Centroids: ");
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    Console.Write("{0:F3}\t", this.centroids[j, i]);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
        public void computeFinalCentroids(double[,] inputs, double[,] outputs, int count)
        {
            Console.WriteLine("Compute Final Centroids ...");
           
            double error = Double.MaxValue;

            int currentCycle = 1;// will be used as Denominator, cannot be zero.
            int plus = 0 ;
            int minus = 0;

            // Cluster input data to determine the input x's clusters
            do
            {
                error = 0.0;
                for (int j = 0; j < count; j++)
                {
                    // for each cluster, find its euclidean distance
                    for (int k = 0; k < this.numberOfClusters; k++)
                    {
                        weights[k] = 0.0;
                        for (int i = 0; i < this.numberOfInputs; i++)
                        {
                            this.weights[k] += Math.Sqrt((inputs[j, i] - this.centroids[k, i]) * (inputs[j, i] - this.centroids[k, i]));
                        }
                    }

                    // then, find the minimum euclidean distance within the clusters 
                    double min = this.weights[0];
                    int minIndex = 0;
                    for (int k = 1; k < this.numberOfClusters; k++)
                    {
                        if (min > this.weights[k])
                        {
                            min = this.weights[k];
                            minIndex = k;
                        }
                    }

                    // perform the update of the winning centroid, with emphasis on -ve learning
                    if (minIndex < outputs.GetLength(1) && outputs[j, minIndex] == 1.0) // index out of range point
                    {
                        for (int i = 0; i < this.numberOfInputs; i++)
                        {

                            this.centroids[minIndex, i] = this.centroids[minIndex, i] + (1.0 / (double)(currentCycle * count)) * (this.centroids[minIndex, i] - inputs[j, i]);
                        }
                        plus++;
                    }
                    else
                    {
                        for (int i = 0; i < this.numberOfInputs; i++)
                        {
                            this.centroids[minIndex, i] = this.centroids[minIndex, i] - (1.0 / (double)(currentCycle * count)) * (this.centroids[minIndex, i] - inputs[j, i]);
      
                        }
                        minus++;
                    }
                    error += weights[minIndex];

                }

                error = error / count;

                if (currentCycle % 5000 == 0)
                {
                    Console.WriteLine("error: {0:F3}\tloop: {1:D}", error, currentCycle);
                }
                currentCycle++;
                
                
            } while (error > MAXERROR && currentCycle <= MAXCYCLES);

            Console.WriteLine("error: {0:F3}\tloop: {1:D}", error, currentCycle);
            Console.WriteLine("+ve learning: {0:D}\t-ve learning: {1:D}", plus, minus);

            // Sort the centroids of each cluster in ascending order
            double temp;
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                for (int k = 0; k < this.numberOfClusters; k++)
                {
                    for (int j = k; j < this.numberOfClusters; j++)
                    {
                        if (this.centroids[k, i] > this.centroids[j, i])
                        {
                            temp = this.centroids[k, i];
                            this.centroids[k, i] = this.centroids[j, i];
                            this.centroids[j, i] = temp;
                        }
                    }
                }
            }

            // calculate the corresponding widths
            double leftCentroid;
            double rightCentroid;
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                // compute first & last element since they have no RHS & LHS neighbours
                rightCentroid = Math.Abs(this.centroids[0, i] - this.centroids[1, i]);
                this.widths[0, i] = rightCentroid / WIDTH_CONSTANT;
                leftCentroid = Math.Abs(this.centroids[this.numberOfClusters - 1, i] - this.centroids[this.numberOfClusters - 2, i]);
                this.widths[this.numberOfClusters - 1, i] = leftCentroid / WIDTH_CONSTANT;

                // compute the others
                for (int j = 1; j < this.numberOfClusters - 1; j++)
                {
                    leftCentroid = Math.Abs(this.centroids[j, i] - this.centroids[j - 1, i]);
                    rightCentroid = Math.Abs(this.centroids[j, i] - this.centroids[j + 1, i]);
                    if (leftCentroid <= rightCentroid)
                    {
                        this.widths[j, i] = leftCentroid / WIDTH_CONSTANT;
                    }
                    if (rightCentroid < leftCentroid)
                    {
                        this.widths[j, i] = rightCentroid / WIDTH_CONSTANT;
                    }
                }
            }

            Console.WriteLine("\nFinal Clusters: ");
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                for (int j = 0; j < this.numberOfClusters; j++)
                {

                    Console.Write("{0:F3}\t", this.centroids[j, i]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();

        }     
    }
}
