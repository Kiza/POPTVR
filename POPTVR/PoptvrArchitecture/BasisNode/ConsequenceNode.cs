using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POPTVR.PoptvrArchitecture.BasisNode
{
    class ConsequenceNode
    {
        private const double LEARN_CONST = 0.0001;
        private double error = 0.0;
        private double centroidError;
        private double widthError;

        private int numberOfInputs;
        private int numberOfInvokation = 0;
        private int total = 0;

        private bool[] links;
        private double value = 0.0;

        private double[] weights;
        private double[] backpropagationWeights;// Backpropagation 

        private double centroid = 0.0;
        private double width = 1.0;
       
        private double synapticInput = 0.0;	// Variable for Reinforcement Rule Learning
        private double synapticOutput = 0.0;	// Variable for Reinforcement Rule Learning	
        private double nodeProbability = 0.0;	// Variable for Reinforcement Rule Learning
        private double membershipOutput;	// Variable for Reinforcement Rule Learning	

        // use for weight training
        private double outputLayerValue = 0.0;

        private double errorLyr5;
        private double denom;
        private double num;
        
        private RuleNode[] prevNodes;

        public ConsequenceNode(int numberOfInputs)
        {
            this.createAncestorNodes(numberOfInputs);

            this.value = 0.0;
            this.error = 0.0;

            this.outputLayerValue = 0.0;
            this.numberOfInvokation = 0;
            this.total = 0;

            this.synapticInput = 0;
            this.synapticOutput = 0;
            this.nodeProbability = 0.0;
        }

        public void setCentroidAndWidth(double centroid, double width)
        {
            this.centroid = centroid;
            if (width < 0.001)
            {
                this.width = 0.001;
            }
            else
            {
                this.width = width;
            }
        }

        public double Centroid
        {
            get
            {
                return this.centroid;
            }
        }
        public double Width
        {
            get
            {
                return this.width;
            }
        }
        public double[] BackpropagationWeights
        {
            get
            {
                return this.backpropagationWeights;
            }
        }
        public double[] Weights
        {
            get
            {
                return this.weights;
            }
        }
        public bool[] Links
        {
            get
            {
                return this.links;
            }
        }
        public double Output
        {
            get
            {
                return this.value;
            }
        }

        public double Error
        {
            get
            {
                return this.error;
            }
        }
        public int NumberOfInvokations
        {
            get
            {
                return this.numberOfInvokation;
            }
        }
        public int Total
        {
            get
            {
                return this.total;
            }
        }
        public double MembershipOutput
        {
            get
            {
                return this.membershipOutput;
            }
        }

        public double ErrorLyr5
        {
            set
            {
                this.errorLyr5 = value;
            }
        }
        public double Denom
        {
            set
            {
                this.denom = value;
            }
        }
        public double Num
        {
            set
            {
                this.num = value;
            }
        }
        public double OutputLayerValue
        {
            set
            {
                this.outputLayerValue = value;
            }
        }

        // create the ancestor nodes of this consequence node
        public void createAncestorNodes(int numberOfInputs)
        {
            this.numberOfInputs = numberOfInputs;
            this.prevNodes = new RuleNode[this.numberOfInputs];

            // Use for weight training to recognize the strongest links later
            this.weights = new double[this.numberOfInputs];
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                this.weights[i] = 0.0;
            }

            // Use as a boolean value to indicate a link. Use with weights
            links = new bool[this.numberOfInputs];
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                this.links[i] = true;
            }

            backpropagationWeights = new double[this.numberOfInputs];
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                this.backpropagationWeights[i] = 0.0;
            }
        }

        // initialize one link to one rulenode
        public void assignAncestorNodes(int index, RuleNode ruleNode) 
        {
            prevNodes[index] = ruleNode;
        }

        // This function initializes the width and centroid of this consequence node.
        // The value of width cannot be zero(divide by 0 error). Default to 0.001 
        public void initMembershipFunction(double centroid, double width) 
        {
            this.centroid = centroid;
            if (width < 0.001)
            {
                this.width = 0.001;
            }
            else
            {
                this.width = width;
            }
        }

        // used for feedforwarding. 
        public void computeInput() 
        {
	        double sum = 0.0;
	        for (int i=0; i < this.numberOfInputs; i++) 
            {
		        if (links[i]) 
                {
                    sum += prevNodes[i].Output;
		        }
	        }
            if (sum <= 1.0)
            {
                this.value = sum;
            }
            else
            {
                this.value = 1.0;
            }
        }

        //  compute the error propagated from the output layer
        public void computeError() 
        {
	        this.error = this.errorLyr5*((((this.centroid/this.width)*this.denom)-(this.num/this.width))/(this.denom*this.denom));
        }

        // compute the rate of change of the centroid and width during backpropagation
        public void computeMembershipError() 
        {
	        this.centroidError = this.errorLyr5*((this.value/this.width)/this.denom);
	        this.widthError = this.errorLyr5*((((this.value/(this.width*this.width))*this.num)-(((this.centroid*this.value)/(this.width*this.width))*this.denom))/(this.denom*this.denom));
	        this.centroid += (LEARN_CONST*this.centroidError);
	        this.width += 0.001*(LEARN_CONST*this.widthError);
            if (this.width < 0.001) 
            {
                this.centroid -= (LEARN_CONST * this.centroidError);
                this.width -= 0.001 * (LEARN_CONST * this.widthError);
	        }
        }

        // restores the centroid and width just before the most current backpropagation
        public void restoreMembershipError() 
        {
	        this.centroid -= (LEARN_CONST*this.centroidError);
	        this.width -= 0.001*(LEARN_CONST*this.widthError);
        }

        // computes the value of each weight links in this consequence(result)
        // node.  Each weight value is accumulated for the whole input pattern sequence.
        public void computeWeights() 
        {
	        for (int i=0; i< this.numberOfInputs; i++) 
            {
                double power = (this.outputLayerValue) - this.centroid;
		        power = (power*power)/this.width;
                double membershipGrade = Math.Exp(-power);

                if ((prevNodes[i].Output) * membershipGrade > 0.0)
                {
                    this.numberOfInvokation++;
                }
		        
                this.total++;
                this.weights[i] += this.prevNodes[i].Output * membershipGrade;
	        }
        }

        // computes the inner product/synaptic input of this consequence(result) node.
        public void computeInnerProduct() 
        {
	        // compute the membership grade of this node for a given output vector, y.	
            for (int i = 0; i < this.numberOfInputs; i++) 
            {
                double power = (this.outputLayerValue) - this.centroid;
		        power = (power*power)/this.width;
		        this.membershipOutput = Math.Exp(-power);
	        }

	        this.synapticInput = 0.0;
	        // compute the inner product of this node
	        for (int i=0; i< this.numberOfInputs; i++) 
            {
		        this.synapticInput += this.weights[i]*(this.prevNodes[i].Output);
	        }

	        // generate the node's probability
	        this.nodeProbability = 1.0/(1.0+Math.Exp(-this.synapticInput));

	        // generate the node's threshold
            double threshold = 0;
            Random random = new Random();
	        do 
            {
                threshold = random.NextDouble();
	        } while (threshold < 0.55);

	        // notice that 0 is for unlearning and 1 is for learning, 
	        // unlike the Kohonen-based REINFORCE algorithm

            if (this.nodeProbability > threshold)
            {
                this.synapticOutput = 0.0;
            }
            else
            {
                this.synapticOutput = 1.0;
            }
        }

        public void updateRRWeights(double r) 
        {
	        // Update the weight vector using the REINFORCE equation for inner product
	        for (int i=0; i< this.numberOfInputs; i++) 
            {
		        // REINFORCE equation -> wij += LearnConst*r*(yi-pi)xj
		        this.weights[i] += 0.05*r*(synapticOutput-nodeProbability)*(prevNodes[i].Output);
	        }
        }

        // resets the weights of this consequence node
        public void randomizeWeights() 
        {
            Random random = new Random();
            for (int i=0; i< this.numberOfInputs; i++) 
            {
                if (random.NextDouble() > 0.5)
                {
                    this.weights[i] = random.NextDouble();
                }
                else
                {
                    this.weights[i] = -random.NextDouble();
                }
	        }
        }

        public void setWeights(double weight) 
        {
	        for (int i=0; i< this.numberOfInputs; i++) 
            {
                this.weights[i] = weight;
	        }
        }
        public void setWeights()
        {
            this.setWeights(0.0);
        }

        public void setBackpropagationWeights(double weight) 
        {
            for (int i = 0; i < this.numberOfInputs; i++)
            {
                this.backpropagationWeights[i] = weight;
            }
        }
        public void setBackpropagationWeights()
        {
            this.setBackpropagationWeights(0.0);
        }

        public void computeBackpropagationWeights() 
        {
	        for (int i=0; i< numberOfInputs; i++) 
            {
		        double power = (this.outputLayerValue)-this.centroid;
		        power = (power*power)/this.width;
		        double membershipGrade = Math.Exp(-power);

		        this.backpropagationWeights[i] += prevNodes[i].Output * membershipGrade;
	        }
        }
    }
}
