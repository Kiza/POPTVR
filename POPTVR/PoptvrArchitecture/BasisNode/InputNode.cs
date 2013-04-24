using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POPTVR.PoptvrArchitecture.BasisNode
{
    class InputNode
    {
        private double value;

        public double Value
        {
            set
            {
                this.value = value;
            }

            get
            {
                return this.value;
            }
        }

        public InputNode()
        {
            this.value = 0.0;
        }

        public InputNode(double value)
        {
            // Use for feedforward
            this.value = value;
        }

    }   
}
