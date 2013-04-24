using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POPTVR.PoptvrArchitecture.ClusterModel
{
    class SOcluster : ClusterInterface
    {
        private const int MAXCYCLES = 5000;
        private const double WIDTH_CONSTANT = 40.0;

        private int numberOfInputs;
        private int numberOfClusters;

        private double[] weights;
        private double[,] centroids;
        private double[,] widths;

        public SOcluster(int numberOfInputs, int numberOfClusters)
        {
            this.numberOfInputs = numberOfInputs;
            this.numberOfClusters = numberOfClusters;

            this.weights = new double[numberOfClusters];
            this.centroids = new double[numberOfClusters, numberOfInputs];
            this.widths = new double[numberOfClusters, numberOfInputs];
        }

        public double[,] Widths
        {
            get
            {
                return this.widths;
            }
        }

        public double[,] Centroids
        {
            get
            {
                return this.centroids;
            }
        }

        public void computeInitialCentroids(double[,] inputdata, double[,] outputdata, int count)
        {
            computeInitialCentroids(inputdata, count);
        }
        private void computeInitialCentroids(double [,] inputs, int count)
        {
            Console.WriteLine("Compute Initial Centroids ...");
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                double max = Double.MinValue;
                double min = Double.MaxValue;
                
                for (int j = 0; j < count; j++)
                {
                    if (max < inputs[j, i])
                    {
                        max = inputs[j, i];
                    }
                    if (min > inputs[j, i])
                    {
                        min = inputs[j, i];
                    }
                }

                // equal partitioning 
                double factor = (max - min) / (this.numberOfClusters + 1);
                for (int j = 1; j <= this.numberOfClusters; j++)
                {
                    this.centroids[j - 1, i] = min + (factor * j);
                }            
            }

            Console.WriteLine("Initial Clusters: ");
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    Console.Write("{0:F3}\t", this.centroids[j, i]);
                }
                Console.WriteLine();
            }
        }

        public void computeFinalCentroids(double[,] inputs, double[,] outputs, int count)
        {
            
            computeFinalCentroids(inputs, count);
        }
        private void computeFinalCentroids(double[,] inputs, int count)
        {
            Console.WriteLine("Compute Final Centroids ...");
            double min;
            int minIndex;

            int[] clusterCount = new int[this.numberOfClusters];

            for (int i = 1; i <= MAXCYCLES; i++)
            {
                for (int j = 0; j < clusterCount.Length; j++)
                {
                    clusterCount[j] = 0;
                }

                for (int j = 0; j < count; j++)
                {
                    for (int m = 0; m < this.numberOfClusters; m++)
                    {
                        for (int n = 0; n < this.numberOfInputs; n++)
                        {
                            this.weights[m] += Math.Sqrt(Math.Pow(inputs[j, n] - this.centroids[m, n], 2));
                        }
                    }

                    min = this.weights[0];
                    minIndex = 0;

                    for (int k = 0; k < this.numberOfClusters; k++)
                    {
                        if (min > this.weights[k])
                        {
                            min = this.weights[k];
                            minIndex = k;
                        }
                    }

                    clusterCount[minIndex]++;

                    for (int k = 0; k < this.numberOfInputs; k++)
                    {
                        this.centroids[minIndex, k] = this.centroids[minIndex, k] + (1.0 / i) * (inputs[j, k] - this.centroids[minIndex, k]);
                    }
                }

                if (i % 1000 == 0)
                {
                    Console.WriteLine("\tTrain Cycle: " + i);
                }
            }

            // Sort the centroids of each cluster in ascending order
            double temp;
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    for (int k = j; k < this.numberOfClusters; k++)
                    {
                        if (this.centroids[j, i] > this.centroids[k, i])
                        {
                            temp = this.centroids[j, i];
                            this.centroids[j, i] = this.centroids[k, i];
                            this.centroids[k, i] = temp;
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
                    else
                    {
                        this.widths[j, i] = rightCentroid / WIDTH_CONSTANT;
                    }
                }
            }

            Console.WriteLine("Final Clusters: ");
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    Console.Write("{0:F3}\t", this.centroids[j, i]);
                }
                Console.WriteLine();
            }
        }

        public String testOutputClassification(double [,] inputs, double[,] outputs, int count)
        {
            int correctCount = 0;
            String result = "";

            for (int i = 0; i < count; i++)
            {
                // calculate the output of the network
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    this.weights[j] = 0.0;
                    // calculate total synaptic inputs to each node
                    for (int k = 0; k < this.numberOfInputs; k++)
                    {
                        this.weights[j] += Math.Abs(inputs[i, k] - this.centroids[j,k]);
                    }
                }

                double min = Double.MaxValue;
                int minIndex = -1;
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    if (min > this.weights[j])
                    {
                        min = weights[j];
                        minIndex = j;
                    }
                }
                if (outputs[i, minIndex] == 1.0)
                {
                    correctCount++;
                }

                result += String.Format("Pattern {0:D5}: (", i);
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    if (j == minIndex)
                    {
                        result += "1,";
                    }
                    else
                    {
                       result += "0,";
                    }
                }
                result +=  ")\n";
            }

            double percent = ((double)correctCount / (double)count) * 100.0;
            result += String.Format("correct: {0:F3} percent\n", percent);

            return result;
        }


    }  
}