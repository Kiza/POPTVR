using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POPTVR.PoptvrArchitecture.BasisNode
{
    class OutputNode
    {
        private int numberOfInputs;
        private double value;
        private double error = 0.0;
        private ConsequenceNode[] prevNodes;

        public OutputNode(int numberOfInputs)
        {
            this.numberOfInputs = numberOfInputs;
            this.prevNodes = new ConsequenceNode[this.numberOfInputs];
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

        // This function initializes one link to one consequence node address
        public void assignAncestorNodes(int index, ConsequenceNode consequenceNode) 
        {
            this.prevNodes[index] = consequenceNode;
        }

        // This function computes the value of this output node.  The formula used is
        // the modified centre average defuzzifier.
        public void computeInput() 
        {
            double num = 0.0;
            double denom = 0.0;
	        for (int i=0; i<this.numberOfInputs; i++) 
            {
		        num += (prevNodes[i].Centroid*(prevNodes[i].Output))/prevNodes[i].Width;
		        denom += prevNodes[i].Output/prevNodes[i].Width;
	        }

	        // If denominator is zero, we have a divide by zero problem. Default to 0.001
            if (denom == 0.0)
            {
                denom = 0.001;
            }

	        value = num/denom;
        }

        // This function calculates the errors at the output node.  Also, calculate 
        // and write to the consequence nodes, the common numerators and denominators
        // use for calculating error(IV), centroid(IV) & width(IV)
        public void computeError(double desiredValue) 
        {
            this.error = desiredValue - value;

            double num = 0.0;
            double denom = 0.0;

	        for (int i=0; i< this.numberOfInputs; i++) 
            {
		        num += ((prevNodes[i].Centroid*prevNodes[i].Output)/prevNodes[i].Width);
		        denom += (prevNodes[i].Output/prevNodes[i].Width);
	        }
	        for (int i=0; i< this.numberOfInputs; i++) 
            {
		        prevNodes[i].Num = num;
		        prevNodes[i].Denom = denom;
		        prevNodes[i].ErrorLyr5 = error;
	        }
        }

        // This function writes the value of this output node to all its consequence nodes
        // so that the membership grade of the consequence nodes can be derived.  These
        // written values are used for weight training & link selection.
        public void writeToConsequenceLayer(double outputLayerValue) 
        {
	        for (int i=0; i<this.numberOfInputs; i++) 
            {
                prevNodes[i].OutputLayerValue = outputLayerValue;	
	        }
        }

    }
}
