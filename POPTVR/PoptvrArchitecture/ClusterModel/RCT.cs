using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.Entities;

namespace POPTVR.PoptvrArchitecture.ClusterModel
{
    class RCT : ClusterInterface
    {
        private const double LEARN_CONST = 0.01;
        private const double WIDTH_CONSTANT = 40.0;
        private const int MAXCYCLES = 1000;

        private int numberOfInputs;
        private int numberOfClusters;
        private int total;

        private double[] synapticInput;
        private double[] nodeProbability;
        private double[] synapticOutput;

        private double[,] centroids;
        private double[,] widths;

        private int[] patternCluster;

        public RCT(int numberOfInputs, int numberOfClusters, int totalNumber)
        {
            this.numberOfInputs = numberOfInputs;
            this.numberOfClusters = numberOfClusters;
            this.total = totalNumber;

            this.patternCluster = new int[this.total];
            for (int i = 0; i < this.total; i++)
            {
                patternCluster[i] = -1;
            }

            this.synapticInput = new double[this.numberOfClusters];
            this.synapticOutput = new double[this.numberOfClusters];
            this.nodeProbability = new double[this.numberOfClusters];

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
        private void computeInitialCentroids(double[,] inputs, int count)
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

                // using equal partitions
                double factor = (max - min) / (this.numberOfClusters + 1);
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    this.centroids[j, i] = min + (factor * (j + 1));
                }
            }

            // Sort the centroids of each cluster in ascending order
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    for (int k = j; k < this.numberOfClusters; k++)
                    {
                        if (this.centroids[j, i] > this.centroids[k, i])
                        {
                            double temp = this.centroids[j, i];
                            this.centroids[j, i] = this.centroids[k, i];
                            this.centroids[k, i] = temp;
                        }
                    }
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

        public void computeFinalCentroids_(double[,] inputs, double[,] outputs, int count)
        {
            Console.WriteLine("Compute Final Centroids ...");
            double prevError;
            double error = Double.MaxValue;

            int cycle = 0;
            do
            {
                prevError = error;
                error = 0.0;
                for (int j = 0; j < count; j++)
                {
                    // calculate the output of the network
                    for (int k = 0; k < this.numberOfClusters; k++)
                    {
                        this.synapticInput[k] = 0.0;

                        // calculate total synaptic inputs to each node
                        for (int i = 0; i < this.numberOfInputs; i++)
                        {
                            this.synapticInput[k] += inputs[j, i] * this.centroids[k, i];
                        }

                        // calculate the node probability & node output
                        this.nodeProbability[k] = 1.0 / (1.0 + Math.Exp(-this.synapticInput[k]));

                        // generate the node's threshold
                        Random random = new Random();
                        double threshold = random.NextDouble();

                        if (this.nodeProbability[k] > threshold)
                        {
                            this.synapticOutput[k] = 1.0;
                        }
                        else
                        {
                            this.synapticOutput[k] = 0.0;
                        }
                    }

                    // update the weights according to reward or punishment
                    for (int k = 0; k < this.numberOfClusters; k++)
                    {
                        // Using REINFORCE signal {-1, 1}
                        // temp = LEARN_CONST*((2.0*outputdata[j][k])-1.0)*(synap_ops[k]-node_probability[k]);

                        // Using REINFORCE signal {0, 1}
                        double temp = LEARN_CONST * outputs[j, k] * (this.synapticOutput[k] - this.nodeProbability[k]);
                        for (int i = 0; i < this.numberOfInputs; i++)
                        {
                            this.centroids[k, i] += temp * inputs[j, i];
                        }
                    }
                    for (int k = 0; k < this.numberOfClusters; k++)
                    {
                        error += ((outputs[j, k] - this.synapticOutput[k]) * (outputs[j, k] - this.synapticOutput[k]));
                    }
                }

                cycle++;
                Console.WriteLine("Previous error: {0:F3}\terror: {1:F3}\tCycle: {2:D}", prevError, error, cycle);
            } while ((error > (0.1 * 0.1 * count * this.numberOfClusters)) && ((prevError - error) > 0.001));

            // Sort the centroids of each cluster in ascending order
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                for (int k = 0; k < this.numberOfClusters; k++)
                {
                    for (int j = k; j < this.numberOfClusters; j++)
                    {
                        if (this.centroids[k, i] > this.centroids[j, i])
                        {
                            double temp = this.centroids[k, i];
                            this.centroids[k, i] = this.centroids[j, i];
                            this.centroids[j, i] = temp;
                        }
                    }
                }
            }

            // calculate the corresponding widths
            double LHScentroid;
            double RHScentroid;
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                // compute first & last element since they have no RHS & LHS neighbours
                RHScentroid = Math.Abs(this.centroids[0, i] - this.centroids[1, i]);
                this.widths[0, i] = RHScentroid / WIDTH_CONSTANT;
                LHScentroid = Math.Abs(this.centroids[this.numberOfClusters - 1, i] - this.centroids[this.numberOfClusters - 2, i]);
                this.widths[this.numberOfClusters - 1, i] = LHScentroid / WIDTH_CONSTANT;

                // compute the others
                for (int j = 1; j < this.numberOfClusters - 1; j++)
                {
                    LHScentroid = Math.Abs(this.centroids[j, i] - this.centroids[j - 1, i]);
                    RHScentroid = Math.Abs(this.centroids[j, i] - this.centroids[j + 1, i]);
                    if (LHScentroid <= RHScentroid)
                    {
                        this.widths[j, i] = LHScentroid / WIDTH_CONSTANT;
                    }
                    if (RHScentroid < LHScentroid)
                    {
                        this.widths[j, i] = RHScentroid / WIDTH_CONSTANT;
                    }
                }
            }

            Console.WriteLine("Final Clusters: ");
            for (int j = 0; j < this.numberOfClusters; j++)
            {
                for (int i = 0; i < this.numberOfInputs; i++)
                {
                    Console.Write("centroid: {0:F3}\twidth: {1:F3}\t", this.centroids[j, i], this.widths[j, i]);
                }
                Console.WriteLine();
            }
        }
        public void computeFinalCentroids(double[,] inputs, double[,] outputs, int count)
        {
            computeFinalCentroids(inputs, count);
        }
        public void computeFinalCentroids(double[,] inputs, int count)
        {
            Console.WriteLine("Compute Final Centroids ...");
            int countLearn = 0;
            int countUnlearn = 0;

            double[] euclideanDistance = new double[this.numberOfClusters];
            double[] distanceVector = new double[this.numberOfInputs];

            //training
            for (int loop = 0; loop < 1000; loop++)
            {
                // for each input pattern j
                for (int j = 0; j < count; j++)
                {
                    for (int k = 0; k < this.numberOfClusters; k++)
                    {
                        euclideanDistance[k] = 0.0;
                        for (int i = 0; i < numberOfInputs; i++)
                        {
                            euclideanDistance[k] += ((inputs[j, i] - this.centroids[k, i]) * (inputs[j, i] - this.centroids[k, i]));
                        }
                    }

                    // then, find the minimum euclidean distance within the clusters 
                    double min = double.MaxValue;
                    int minIndex = -1;
                    for (int k = 0; k < numberOfClusters; k++)
                    {
                        if (min > euclideanDistance[k])
                        {
                            min = euclideanDistance[k];
                            minIndex = k;
                        }
                    }

                    // calculate the output of the network
                    for (int k = 0; k < this.numberOfClusters; k++)
                    {
                        this.synapticInput[k] = 0.0;

                        // calculate total synaptic inputs to each node
                        for (int i = 0; i < this.numberOfInputs; i++)
                        {
                            this.synapticInput[k] += Math.Abs(inputs[j, i] - this.centroids[k, i]);
                        }

                        // calculate the node probability & node output
                        this.nodeProbability[k] = 1.0 / (1.0 + Math.Exp(-this.synapticInput[k]));

                        // generate the node's threshol
                        Random random = new Random();
                        double threshold = random.NextDouble();
                        while (threshold < 0.55)
                        {
                            threshold = random.NextDouble();
                        }

                        if (this.nodeProbability[k] > threshold)
                        {
                            this.synapticOutput[k] = 1.0;
                        }
                        else
                        {
                            this.synapticOutput[k] = 0.0;
                        }
                    }

                    for (int k = 0; k < this.numberOfClusters; k++)
                    {
                        double reinforcement;
                        // the winner node is found
                        if (k == minIndex)
                        {
                            reinforcement = 1.0;
                        }
                        else
                        {
                            reinforcement = 0.0;
                        }

                        // compute vector (X-W)
                        for (int i = 0; i < this.numberOfInputs; i++)
                        {
                            distanceVector[i] = (inputs[j, i] - this.centroids[k, i]);
                        }
                        // update the centroids
                        for (int i = 0; i < numberOfInputs; i++)
                        {
                            // if Y = 0, perform learning on winner
                            if (this.synapticOutput[k] < 0.9)
                            {
                                this.centroids[k, i] += 2.0 * LEARN_CONST * reinforcement * this.nodeProbability[k] * distanceVector[i];
                                countLearn++;
                            }
                            // else if Y = 1, perform unlearning on winner
                            else
                            {
                                this.centroids[k, i] += 2.0 * LEARN_CONST * reinforcement * (this.nodeProbability[k] - 1.0) * distanceVector[i];
                                countUnlearn++;
                            }
                        }
                    }
                }

                if (loop % 200 == 0)
                {
                    Console.WriteLine("\tTrain Cycle: " + loop);
                }
            } // end of training

            Console.WriteLine("No. of learnings: {0:D}\tNo. of unlearnings: {1:D}\n", countLearn, countUnlearn);

            // Sort the centroids of each cluster in ascending order
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    for (int k = j; k < this.numberOfClusters; k++)
                    {
                        if (this.centroids[j, i] > centroids[k, i])
                        {
                            double temp = this.centroids[j, i];
                            this.centroids[j, i] = this.centroids[k, i];
                            this.centroids[k, i] = temp;
                        }
                    }
                }
            }

            // calculate the corresponding widths
            double leftCentroid;
            double rightCentroid;
            for (int i = 0; i < numberOfInputs; i++)
            {
                // compute first & last element since they have no RHS & LHS neighbours
                rightCentroid = Math.Abs(this.centroids[0, i] - this.centroids[1, i]);
                this.widths[0, i] = rightCentroid / WIDTH_CONSTANT;
                leftCentroid = Math.Abs(this.centroids[numberOfClusters - 1, i] - this.centroids[numberOfClusters - 2, i]);
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

            Console.WriteLine("Final Clusters: ");
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                for (int j = 0; j < numberOfClusters; j++)
                {
                    Console.WriteLine("{0:F3}\t", this.centroids[j, i]);
                }
                Console.WriteLine("\n");
            }
        }

        public string testOutputClassification(double[,] inputs, double[,] outputs, int count)
        {
            int correctCount = 0;
            string result = "";

            for (int j = 0; j < count; j++)
            {
                // calculate the output of the network
                for (int k = 0; k < this.numberOfClusters; k++)
                {
                    this.synapticInput[k] = 0.0;
                    // calculate total synaptic inputs to each node
                    for (int i = 0; i < this.numberOfInputs; i++)
                    {
                        this.synapticInput[k] += Math.Abs(inputs[j, i] - this.centroids[k, i]);
                    }

                    // calculate the node probability & node output
                    this.nodeProbability[k] = 1.0 / (1.0 + Math.Exp(-this.synapticInput[k]));
                }

                double min = double.MaxValue;
                int minIndex = -1;
                for (int k = 0; k < this.numberOfClusters; k++)
                {
                    if (min > this.synapticInput[k])
                    {
                        min = synapticInput[k];
                        minIndex = k;
                    }
                }
                if (outputs[j, minIndex] == 1.0)
                {
                    correctCount++;
                }

                result += String.Format("Pattern{0:D3}: (", j);
                for (int k = 0; k < this.numberOfClusters; k++)
                {
                    if (k == minIndex)
                    {
                        this.synapticOutput[k] = 1.0;
                    }
                    else
                    {
                        this.synapticOutput[k] = 0.0;
                    }
                    result += String.Format("{0:F}", this.synapticOutput[k]);
                }
                result += ")\n";
            }
            result += String.Format("correct: {0:F3} percent\n", ((double)correctCount / (double)count) * 100.0);

            return result;
        }

        
    }
}
