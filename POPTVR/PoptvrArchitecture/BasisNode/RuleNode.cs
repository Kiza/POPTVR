using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POPTVR.PoptvrArchitecture.BasisNode
{
    class RuleNode
    {
        private double value;
        private double error;
        private bool blocked = false;
        private ConditionNode [] prevNodes;
        public RuleNode()
        {
            this.value = 0.0;
            this.blocked = false;
        }
        public bool Blocked
        {
            set
            {
                this.blocked = value;
            }
        }


        private bool pointBlocked = false;
        private int pointBlockedAt = 0;

        public bool PointBlocked
        {
            set
            {
                this.pointBlocked = value;
            }
        }
        public int PointBlockedAt
        {
            set
            {
                this.pointBlockedAt = value;
            }
        }

        public double Error
        {
            set
            {
                this.error = value;
            }
            get
            {
                return this.error;
            }
        }
        public double Output
        {
            get
            {
                return this.value;
            }
        }
        public ConditionNode[] PrevNodes
        {
            get
            {
                return this.prevNodes;
            }
        }

        public RuleNode(int numberOfInputs)
        {
            this.createAncestorNodes(numberOfInputs);
        }

        public void createAncestorNodes(int numberOfInputs)
        {
            this.prevNodes = new ConditionNode [numberOfInputs];
        }

        public void assignAncestorNode(int index, ConditionNode ancestorNode)
        {
            this.prevNodes[index] = ancestorNode;
        }

        public void computeInput()
        {
            if (blocked)
            {
                this.value = 0;
            }
            else
            {
                double min = Double.MaxValue;
                for (int i = 0; i < this.prevNodes.Length; i++)
                {
                    if (this.pointBlocked && i == this.pointBlockedAt)
                    {
                        continue;
                    }
                    else if (this.prevNodes[i] != null && min > this.prevNodes[i].Output)
                    {
                        min = this.prevNodes[i].Output;
                    }
                }

                this.value = min;
            }
        }

    }
}
