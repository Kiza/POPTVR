using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.Entities;
using POPTVR.PoptvrArchitecture.ClusterModel;
using POPTVR.PoptvrArchitecture.BasisNode;
using POPTVR.Utilities;

namespace POPTVR.PoptvrArchitecture
{
    public class PoptvrModel
    {
        private int numberOfInputs;
        private int numberOfOutputs;
        private int inputClusterSize;
        private int outputClusterSize;

        private int[,] rules;
        private InputNode[] inputLayer;
        private ConditionNode[] conditionLayer;
        private RuleNode[] ruleLayer;
        private ConsequenceNode[] consequenceLayer;
        private OutputNode[] outputLayer;

        private string initialWeightsString = "";
        private string selectedWeightsString = "";
        private string fuzzyRulesString = "";

        public OutputNode[] OutputLayer
        {
            get
            {
                return this.outputLayer;
            }
        }
        public ConditionNode[] ConsequenceLayer
        {
            get
            {
                return conditionLayer;
            }
        }
        public RuleNode[] RuleLayer
        {
            get
            {
                return this.ruleLayer;
            }
        }
        public ConditionNode[] ConditionLayer
        {
            get
            {
                return conditionLayer;
            }
        }
        public int NumberOfInputs
        {
            get
            {
                return this.numberOfInputs;
            }
        }
        public int InputClusterSize
        {
            get
            {
                return this.inputClusterSize;
            }
        }
        public int OutputClusterSize
        {
            get
            {
                return this.outputClusterSize;
            }
        }
        


        private DataSet dataset;
        private ClusterSetting clusterSetting;

        public PoptvrModel(DataSet dataset, ClusterSetting clusterSetting)
        {
            this.dataset = dataset;
            this.clusterSetting = clusterSetting;

            this.numberOfInputs = this.dataset.NumberOfInputNodes;
            this.numberOfOutputs = this.dataset.NumberOfOutputNodes;

            this.inputClusterSize = this.clusterSetting.InputClusterSize;
            this.outputClusterSize = this.clusterSetting.OutputClusterSize;

            buildRULES();
	        buildPOPTVR();
        }  
        public PoptvrModel(int numberOfInputs, int inputClustersSize, int numberOfOutputs, int outputClustersSize) 
        {
            this.numberOfInputs = numberOfInputs;
            this.numberOfOutputs = numberOfOutputs;
            this.inputClusterSize = inputClustersSize;
            this.outputClusterSize = outputClustersSize;
	        buildRULES();
	        buildPOPTVR();
        }

        


        public string InitialWeightsString
        {
            get
            {
                return this.initialWeightsString;
            }
        }
        public string SelectedWeightsString
        {
            get
            {
                return this.selectedWeightsString;
            }
        }
        public string FuzzyRulesString
        {
            get
            {
                return this.fuzzyRulesString;
            }
        }

        public string initMembership(ClusterInterface inputCluster, ClusterInterface outputCluster) 
        {

            double[,] inputCentroids = inputCluster.Centroids;
            double[,] inputWidths = inputCluster.Widths;
            double[,] outputCentroids = outputCluster.Centroids;
            double[,] outputWidths = outputCluster.Widths;

            string result = "Condition Layer:\n";
	        
            int k = 0;
	        for (int i=0; i<this.numberOfInputs; i++) 
            {
		        for (int j=0; j<this.inputClusterSize; j++) 
                {
                    conditionLayer[k].setCentroidAndWidth(inputCentroids[j, i], inputWidths[j, i]);
                    result += String.Format("centroid:{0:F3}\twidth:{1:F3}\n", conditionLayer[k].Centroid, conditionLayer[k].Width);
			        
                    k++;		
		        }
	        }

	        result += "\nConsequence Layer:\n";
	        k = 0;
	        for (int i=0; i<this.numberOfOutputs; i++) 
            {
		        for (int j=0; j<this.outputClusterSize; j++) 
                {
                    consequenceLayer[k].setCentroidAndWidth(outputCentroids[j, i], outputWidths[j, i]);
                    result += String.Format("centroid:{0:F3}\twidth:{1:F3}\n", consequenceLayer[k].Centroid, consequenceLayer[k].Width);
			        
			        k++;
		        }
	        }

            return result;
        }

        public void buildRULES() 
        {	
	        this.rules = new int[this.numberOfInputs, (int)Math.Pow(this.inputClusterSize, this.numberOfInputs)];

	        // permutate all the rules; constructing truth table with MSB first
	        for (int i=1; i<=numberOfInputs; i++) 
            {
		        int index = 0;
		        for (int j=0; j<(int)Math.Pow(this.inputClusterSize,i); j++) 
                {
			        index = (index+1) % this.inputClusterSize;

			        for (int k=0; k<(int) Math.Pow(this.inputClusterSize, this.numberOfInputs - i ); k++) 
                    {
				        this.rules[i-1, j* (int)Math.Pow(this.inputClusterSize, this.numberOfInputs-i)+ k] = index;
			        }
		        }
	        }
        }

        private void initInputLayer()
        {
            this.inputLayer = new InputNode[this.numberOfInputs];
            for (int i = 0; i < this.inputLayer.Length; i ++ )
            {
                this.inputLayer[i] = new InputNode();
            }
        }
        private void initConditionLayer()
        {
            this.conditionLayer = new ConditionNode[this.inputClusterSize * this.numberOfInputs];
            // Connect Condition Layer to Input Layer
            int k = 0;
            for (int i = 1; i <= this.numberOfInputs; i++)
            {
                for (int j = 1; j <= this.inputClusterSize; j++)
                {
                    this.conditionLayer[k] = new ConditionNode(this.inputLayer[i - 1]);
                    k++;
                }
            }
        }
        private void initRuleLayer()
        {
            this.ruleLayer = new RuleNode[(int)Math.Pow(this.inputClusterSize, this.numberOfInputs)];
            // Connect Rule Layer to Condition Layer using the rules generated
            // First create in each Rule Node the no. of links
            for (int i = 0; i < this.ruleLayer.Length; i++)
            {
                this.ruleLayer[i] = new RuleNode(this.numberOfInputs);
            }
            // Then do the connection
            for (int i = 0; i < this.ruleLayer.Length; i++)
            {
                for (int j = 0; j < this.numberOfInputs; j++)
                {
                    this.ruleLayer[i].assignAncestorNode(j, this.conditionLayer[j * this.inputClusterSize + this.rules[j, i]]);
                }
            }
        }
        private void initConsequenceLayer()
        {
            this.consequenceLayer = new ConsequenceNode[this.outputClusterSize * this.numberOfOutputs];
            // Connect Consequence Layer to Rule Layer 
            // First create in each Consequence Node the no. of links
            for (int i = 0; i < this.consequenceLayer.Length; i++)
            {
                this.consequenceLayer[i] = new ConsequenceNode((int)Math.Pow(this.inputClusterSize, this.numberOfInputs));
            }
            // Then do the connection
            for (int i = 0; i < this.outputClusterSize * this.numberOfOutputs; i++)
            {
                for (int j = 0; j < (int)Math.Pow(this.inputClusterSize, this.numberOfInputs); j++)
                {
                    this.consequenceLayer[i].assignAncestorNodes(j, this.ruleLayer[j]);
                }
            }
        }
        private void initOutputLayer()
        {
            this.outputLayer = new OutputNode[this.numberOfOutputs];
            // Connect Output Layer to Consequence Layer
            // First create in each Output Node the no. of links
            for (int i = 0; i < this.numberOfOutputs; i++)
            {
                outputLayer[i] = new OutputNode(this.outputClusterSize);
            }
            // Then do the connection
            int k = 0;
            for (int i = 0; i < this.numberOfOutputs; i++)
            {
                for (int j = 0; j < this.outputClusterSize; j++)
                {
                    this.outputLayer[i].assignAncestorNodes(j, this.consequenceLayer[k]);
                    k++;
                }
            }
        }

        public void buildPOPTVR() 
        {
            // order is important
            initInputLayer();
            initConditionLayer();
            initRuleLayer();
            initConsequenceLayer();
            initOutputLayer();
        }

        public void backpropagation(double[] inputdata, double[] desired) 
        {
            bool[][] links = new bool[this.outputClusterSize * this.numberOfOutputs][];
	        // init the weights ptrs to point to the weights in consequence layer
	        for (int i=0; i<this.outputClusterSize * this.numberOfOutputs; i++) 
            {
                links[i] = this.consequenceLayer[i].Links;
	        }

	        // compute error at the output layer
	        for (int i=0; i<this.numberOfOutputs; i++) 
            {
		        this.outputLayer[i].computeError(desired[i]);
	        }

	        // compute error at the consequence layer
	        for (int i=0; i< this.numberOfOutputs; i++) 
            {
		        for (int j=0; j<this.outputClusterSize; j++) 
                {
                    this.consequenceLayer[i * this.outputClusterSize + j].computeError();
                    this.consequenceLayer[i * this.outputClusterSize + j].computeMembershipError();
		        }
	        }

	        // compute error at rule layer
            for (int i = 0; i < (int)Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
		        double temp = 0.0;
                for (int j = 0; j < this.numberOfOutputs * this.outputClusterSize; j++) 
                {
			        if (links[j][i]) 
                    {
				        temp += this.consequenceLayer[j].Error;
			        }
		        }
		        this.ruleLayer[i].Error = temp;
	        }

	        // compute error at condition layer
	        // ConditionNode[] ConNode;
	        // reset each condition node's error to zero first
	        for (int i=0; i< this.numberOfInputs * this.inputClusterSize; i++) 
            {
		        conditionLayer[i].clearError();
	        }

	        // propagate the error from each rule node to its ancestors
	        for (int i=0; i <(int)Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
		        double error = this.ruleLayer[i].Error;
		        for (int j=0; j < this.numberOfInputs; j++) 
                {
			        // if Oij(II) = Fk(III)
                    if (ruleLayer[i].Output == this.ruleLayer[i].PrevNodes[j].Output) 
                    {
                        this.ruleLayer[i].PrevNodes[j].computeError(error);
			        }	
		        }
	        }

	        // Compute the membership error at the condition layer
	        for (int i=0; i<this.numberOfInputs * this.inputClusterSize; i++) 
            {
		        this.conditionLayer[i].computeMembershipError();
	        }
        }

        public void undoBackpropagation() 
        {
	        for (int i=0; i < this.numberOfOutputs; i++) 
            {
		        for (int j=0; j < this.outputClusterSize; j++) 
                {
			        this.consequenceLayer[i * outputClusterSize + j].restoreMembershipError();
		        }
	        }

	        for (int i=0; i < this.numberOfInputs * this.inputClusterSize; i++) 
            {
                conditionLayer[i].restoreLastMemebership();
	        }
        }

        public void popTrainBackpropagationWeights(double[] inputdata, double[] outputdata) 
        {
	        // feedforward input & condition layer
	        int k = 0;
	        for (int i=1; i<= this.numberOfInputs; i++) 
            {
		        this.inputLayer[i-1].Value = inputdata[i-1];
		        for (int j=1; j <= this.inputClusterSize; j++) 
                {
			        this.conditionLayer[k].computeInput();
			        k++;
		        }
	        }

	        // feedforward rule layer
	        for (int i=0; i<(int)Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
		        this.ruleLayer[i].computeInput();
	        }

	        // write the output vector to the output layer
	        for (int i=0; i < this.numberOfOutputs; i++) 
            {
		        this.outputLayer[i].writeToConsequenceLayer(outputdata[i]);
	        }

	        // compute the membership grade for the consequence layer & its corr weights
	        for (int i=0; i < this.outputClusterSize * this.numberOfOutputs; i++) 
            {
		       this. consequenceLayer[i].computeBackpropagationWeights();
	        }
        }

        public void forwardFeed(double[] inputdata) 
        {
	        // feedforward input & condition layer
	        int k = 0;
	        for (int i=1; i<= this.numberOfInputs; i++) 
            {
		        this.inputLayer[i-1].Value = inputdata[i-1];
		        for (int j=1; j <= this.inputClusterSize; j++) 
                {
			        this.conditionLayer[k].computeInput();
			        k++;
		        }
	        }

	        // feedforward rule layer
	        for (int i=0; i<(int)Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
		        this.ruleLayer[i].computeInput();
	        }

	        // feedforward consequence layer
	        for (int i=0; i <this.outputClusterSize * this.numberOfOutputs; i++) 
            {
                this.consequenceLayer[i].computeInput();
	        }

	        // feedforward output layer
	        for (int i=0; i < this.numberOfOutputs; i++) 
            {
                this.outputLayer[i].computeInput();	
	        }
        }

        public ConsequenceNode[] forwardFeedToConsequenceLayer(double[] inputdata)
        {
            // feedforward input & condition layer
            int k = 0;
            for (int i = 1; i <= this.numberOfInputs; i++)
            {
                this.inputLayer[i - 1].Value = inputdata[i - 1];
                for (int j = 1; j <= this.inputClusterSize; j++)
                {
                    this.conditionLayer[k].computeInput();
                    k++;
                }
            }

            // feedforward rule layer
            for (int i = 0; i < (int)Math.Pow(this.inputClusterSize, this.numberOfInputs); i++)
            {
                this.ruleLayer[i].computeInput();
            }

            // feedforward consequence layer
            for (int i = 0; i < this.outputClusterSize * this.numberOfOutputs; i++)
            {
                this.consequenceLayer[i].computeInput();
            }

            return consequenceLayer;
        }

        // This function returns the rule node that has fired in correspondence to 
        // a data vector.  It is called after method POPTVR::feedFoward() so that the 
        // layers are properly initialized.
        public void maxRuleNode(int[] ruleNodes) 
        {
            double maxValueOfRuleNode = this.ruleLayer[0].Output;

	        // Find the strongest rule node/nodes
	        for (int i=1; i<(int)Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
                if (maxValueOfRuleNode < this.ruleLayer[i].Output) 
                {
                    maxValueOfRuleNode = this.ruleLayer[i].Output;
		        }
	        }

	        // Find the rule node(s) that has/have fired and increment
	        // its corresponding index
	        for (int i=0; i<(int)Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
                if (maxValueOfRuleNode == this.ruleLayer[i].Output) 
                {
			        // check that the index is not exceeded
			        if (i < ruleNodes.Length) 
                    {
				        ruleNodes[i]++; // count nodes with max value
			        }
		        }
	        }
        }

        public void popResetWeights()
        {
            for (int i = 0; i < this.outputClusterSize * this.numberOfOutputs; i++)
            {
                consequenceLayer[i].setWeights();
            }
        }

        public void popTrainWeights()
        {
            Console.WriteLine("\nPop Train Wieghts ...");
            for (int i = 0; i < dataset.TotalNumberOfRecords; i++)
            {
                if( i % 10 == 0)
                {
                    Console.WriteLine("\tTraining for Record: " + i);
                }

                double[] inputTemp = new double[dataset.NumberOfInputNodes];
                double[] outputTemp = new double[dataset.NumberOfOutputNodes];

                for (int j = 0; j < inputTemp.Length; j++)
                {
                    inputTemp[j] = dataset.Inputdata[i, j];
                }

                for (int j = 0; j < outputTemp.Length; j++)
                {
                    outputTemp[j] = dataset.DesiredOutputs[i, j];
                }

                popTrainWeights(inputTemp, outputTemp);
            }

            Console.WriteLine("Pop Train Wieghts Ends");
        }
        private void popTrainWeights(double [] inputdata, double [] outputdata) 
        {
	        // feedforward input & condition layer
	        int k = 0;
	        for (int i=0; i< this.numberOfInputs; i++) 
            {
		        this.inputLayer[i].Value = inputdata[i];
		        for (int j=0; j < inputClusterSize; j++) 
                {
			        this.conditionLayer[k].computeInput();
			        k++;
		        }
	        }

	        // feedforward rule layer
	        for (int i=0; i<(int)Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
		        ruleLayer[i].computeInput();
	        }

	        // write the output vector to the output layer
	        for (int i=0; i<this.numberOfOutputs; i++) 
            {
		        this.outputLayer[i].writeToConsequenceLayer(outputdata[i]);
	        }

	        // compute the membership grade for the consequence layer & its corr weights
	        for (int i=0; i<this.outputClusterSize * this.numberOfOutputs; i++) 
            {
		        consequenceLayer[i].computeWeights();
	        }
        }

        public void rrTrainWeights(double [] inputdata, double []outputdata) 
        {
	        // feedforward input & condition layer
	        int k = 0;
	        for (int i=1; i<= this.numberOfInputs; i++)
            {
		        this.inputLayer[i-1].Value = inputdata[i-1];
		        for (int j=1; j <= this.inputClusterSize; j++)
                {
			        this.conditionLayer[k].computeInput();
			        k++;
		        }
	        }

	        // feedforward rule layer
	        for (int i=0; i <(int) Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
		        this.ruleLayer[i].computeInput();
	        }
	        // write the output vector to the output layer
	        for (int i=0; i < this.numberOfOutputs; i++)
            {
                this.outputLayer[i].writeToConsequenceLayer(outputdata[i]);
	        }

	        // compute the inner product, node probability, synaptic output and membership 
	        // grade for each node in the consequence layer
	        for (int i=0; i<this.numberOfOutputs * this.outputClusterSize; i++) 
            {
		        this.consequenceLayer[i].computeInnerProduct();
	        }

	        for (int i=0; i < this.numberOfOutputs; i++) 
            {
		        for (int j=0; j < this.outputClusterSize; j++) 
                {
			        // compute the reinforcement signal, 0-1
			        double temp = this.consequenceLayer[ i * outputClusterSize + j].MembershipOutput;
			        this.consequenceLayer[i * outputClusterSize + j].updateRRWeights(temp);		
		        }
	        }
        }

        public void popRandomizeWeights() 
        {
            for (int i = 0; i < this.outputClusterSize * this.numberOfOutputs; i++)
            {
                consequenceLayer[i].randomizeWeights();
            }
	    }
        public void popResetBackpropagationWeights() 
        {
	        for (int i=0; i<this.outputClusterSize * this.numberOfOutputs; i++) 
            {
                consequenceLayer[i].setBackpropagationWeights();
	        }
        }
        public void popLinksSelect() 
        {
            Console.WriteLine("POP Link Select ...");

	        bool[][] links = new bool[this.outputClusterSize * this.numberOfOutputs][];
	        double [][] weights = new double [this.outputClusterSize * this.numberOfOutputs][];

	        // init the weights ptrs to point to the weights in consequence layer
	        for (int j=0; j< this.outputClusterSize * this.numberOfOutputs; j++) 
            {
		        weights[j] = consequenceLayer[j].Weights;
		        links[j] = consequenceLayer[j].Links;
	        }

	        // write initial weights to file
            this.initialWeightsString = "";
	        for (int i=0; i<(int) Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
                this.initialWeightsString += String.Format("Rule Node{0:D}: ", i);
                for (int j=0; j < this.numberOfOutputs * this.outputClusterSize; j++) 
                {
                    this.initialWeightsString += String.Format("{0:F3}\t", weights[j][i]);
		        }
                this.initialWeightsString += "\n";
	        }

	        // select the strongest links 		
	        for (int i=0; i<(int) Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
		        for (int j=0; j< this.numberOfOutputs; j++) 
                {
			        // for each output node, find the linguistic label with the highest value
			        double maxIndex = 0;
			        double max = weights[j * this.outputClusterSize + 0][i];

			        for (int k=1; k< this.outputClusterSize; k++) 
                    {
				        if (max < weights[j * this.outputClusterSize + k][i]) 
                        {
					        max = weights[j * this.outputClusterSize + k][i];
					        maxIndex = k;
				        }
			        }

			        for (int k=0; k< this.outputClusterSize; k++) 
                    {
                        if (k != maxIndex)
                        {
                            links[j * outputClusterSize + k][i] = false;
                        }
                        else
                        {
                            links[j * outputClusterSize + k][i] = true;
                        }
			        }
		        }
	        }	
	
	        // write selected weights to file
            this.selectedWeightsString = "";
	        for (int i=0; i<(int)Math.Pow(this.inputClusterSize, this.numberOfInputs); i++) 
            {
                this.selectedWeightsString += String.Format("Rule Node{0:D}: ", i);
		        for (int j=0; j<this.numberOfOutputs * this.outputClusterSize; j++) 
                {
                    if (links[j][i])
                    {
                        this.selectedWeightsString += String.Format("{0:F3}\t", weights[j][i]);
                    }
                    else
                    {
                        this.selectedWeightsString += String.Format("{0:F3}\t", 0.0);
                    }
		        }
                this.selectedWeightsString += "\n";
	        }

	        // write the set of fuzzy rules to file
            this.fuzzyRulesString = "";
	        for (int j=0; j<(int)Math.Pow(this.inputClusterSize, this.numberOfInputs); j++) 
            {
                this.fuzzyRulesString += String.Format("Rule Node{0:D}: (", j);
                for (int i=0; i<this.numberOfInputs; i++) 
                {
                    this.fuzzyRulesString += String.Format("{0:D}, ", this.rules[i, j]);
		        }

                this.fuzzyRulesString += ")->(";
		        for (int i=0; i<this.numberOfOutputs * this.outputClusterSize; i++) 
                {
                    if (links[i][j])
                    {
                        this.fuzzyRulesString += " 1 ";
                    }
                    else
                    {
                        this.fuzzyRulesString += " - ";
                    }
		        }

                this.fuzzyRulesString += ")\n";
	        }
        }

        public void popLearn(DataSet dataset)
        {
            this.dataset = dataset;
            popLearn();
        }
        public void popLearn()
        {
            this.popResetWeights();
            this.popTrainWeights();
            this.popLinksSelect();
        }

        public static string POPTest(PoptvrModel poptvr, DataSet dataset)
        {
            Console.WriteLine("POPTest ....");

            string resultString = "";
            int correct = 0;

            int[] ruleNodes = new int[(int)Math.Pow(poptvr.inputClusterSize, dataset.NumberOfInputNodes)];
            int[] classTotal = new int[dataset.NumberOfOutputNodes];
            int[] classCorrect = new int[dataset.NumberOfOutputNodes];

            for (int i = 0; i < dataset.TotalNumberOfRecords; i++)
            {
                bool correctFlag = false;

                poptvr.forwardFeed(SystemFunctions.getArrayAtRow(dataset.Inputdata, i));
                poptvr.maxRuleNode(ruleNodes);

                double max = poptvr.OutputLayer[0].Output;
                int maxIndex = 0;
                for (int j = 0; j < dataset.NumberOfOutputNodes; j++)
                {
                    resultString += String.Format("{0:F3}\t", poptvr.OutputLayer[j].Output);
                    if (max < poptvr.OutputLayer[j].Output)
                    {
                        max = poptvr.OutputLayer[j].Output;
                        maxIndex = j;
                    }
                }

                for (int j = 0; j < dataset.NumberOfOutputNodes; j++)
                {
                    // calculate the total no. in each class
                    if (dataset.DesiredOutputs[i, j] == 1.0)
                    {
                        classTotal[j]++;
                    }
                    // calculate the correct no. in each class, overall correct
                    if (j == maxIndex)
                    {
                        if (dataset.DesiredOutputs[i, j] == 1.0)
                        {
                            correct++;
                            classCorrect[j]++;
                            correctFlag = true;
                        }
                    }
                }

                if (correctFlag)
                {
                    resultString += "\n";
                }
                else
                {
                    resultString += "X\n";
                }
            }

            for (int i = 0; i < dataset.NumberOfOutputNodes; i++)
            {
                resultString += String.Format("Class [{0:D}]:\t{1:F3}\tpercent\n", i, ((double)classCorrect[i] * 100.0) / (double)classTotal[i]);
            }

            resultString += String.Format("Overall:\t {0:F3}\tpercent\n", (correct * 100.0) / dataset.TotalNumberOfRecords);

            int totalRulesFired = 0;
            for (int i = 0; i < ruleNodes.Length; i++)
            {
                if (ruleNodes[i] != 0)
                {
                    totalRulesFired++;
                }

                resultString += String.Format("ruleNode {0:D}:\t{1:D}\n", i, ruleNodes[i]);
            }

            resultString += String.Format("Total Rules Fired:\t{0:D}\n", totalRulesFired);

            return resultString;
        }

    }
}
