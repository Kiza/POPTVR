using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POPTVR.PoptvrArchitecture.BasisNode
{
    class ConditionNode
    {
        private const double LEARN_CONST = 0.0001;

        private double value = 0.0;
        private double centroid = 0.0;
        private double width = 1.0;
        
        private double error = 0.0;
        private double centroidEerror;
        private double widthError;
        private bool blocked = false;

        public bool Blocked
        {
            set
            {
                blocked = value;
            }
        }

        private InputNode prevNode;

        public ConditionNode(InputNode inputNode)
        {
            this.prevNode = inputNode;
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

        public InputNode PrevNode
        {
            set
            {
                this.prevNode = value;
            }

            get
            {
                return this.prevNode;
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
        public double Width
        {
            get
            {
                return this.width;
            }
        }
        public double Centroid
        {
            get
            {
                return this.centroid;
            }
        }

        public void computeInput()
        {
            if (blocked)
            {
                this.value = double.MaxValue;
            }
            else
            {
                double temp;
                temp = Math.Pow(this.prevNode.Value - centroid, 2) / this.width;
                this.value = Math.Exp(-temp);
            }
        }

        public void computeError(double error)
        {
            this.error = this.error + error;
        }

        // compute the rate of change of the centroid and width during backpropagation
        public void computeMembershipError()
        {
            double temp = this.prevNode.Value - this.centroid;

            this.centroidEerror = this.error * this.value *((2.0 * temp) / (this.width * this.width));
            this.widthError = this.error * this.value * ((2.0 * temp * temp) /(this.width * this.width * this.width));
            
            this.centroid  = this.centroid + LEARN_CONST * this.centroidEerror;
            this.width = this.width + LEARN_CONST * this.widthError * 0.001;

            if(this.width < 0.001)
            {
                this.centroid = this.centroid - LEARN_CONST * this.centroidEerror;
                this.width = this.width - LEARN_CONST * this.widthError * 0.001;
            }
        }

        //restores the centroid and width just before the most current backpropagation
        public void restoreLastMemebership()
        {
            this.centroid = this.centroid - LEARN_CONST * this.centroidEerror;
            this.width = this.width - LEARN_CONST * this.widthError * 0.001;
        }

        public void clearError()
        {
            this.error = 0.0;
        }
    }
}
