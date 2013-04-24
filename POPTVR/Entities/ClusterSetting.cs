using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.Utilities;

namespace POPTVR.Entities
{
    class ClusterSetting
    {
        private int inputClusterSize;
        private int outputClusterSize;
        private int maxTrainCycleNumber;
        private int printOutInterval;
        private double widthConstant;
        private double maxError;
        private double learningRate;

        public ClusterSetting(int inputClusterSize, int outputClusterSize, int maxTrainCycleNumber, int printOutInterval, double widthConstant, double maxError, double learningRate)
        {
            this.inputClusterSize = inputClusterSize;
            this.outputClusterSize = outputClusterSize;
            this.maxTrainCycleNumber = maxTrainCycleNumber;
            this.printOutInterval = printOutInterval;
            this.widthConstant = widthConstant;
            this.maxError = maxError;
            this.learningRate = learningRate;
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
        public int MaxTrainCycleNumber
        {
            get
            {
                return this.maxTrainCycleNumber;
            }
        }
        public int PrintOutInterval
        {
            get
            {
                return printOutInterval;
            }
        }
        public double WidthConstant
        {
            get
            {
                return this.widthConstant;
            }
        }
        public double MaxError
        {
            get
            {
                return maxError;
            }
        }
        public double LearningRate
        {
            get
            {
                return learningRate;
            }
        }

        public static ClusterSetting getClusterSettings()
        {
            int inpuClusterSize = AppConfig.getInputClusterSize();
            int outputClusterSize = AppConfig.getOutputClusterSize();
            int maxTrainCycleNumber = AppConfig.getMaxTrainCycleNumber();
            int printOutInterval = AppConfig.getPrintOutInterval();
            double widthConstant = AppConfig.getWidthConstant();
            double maxError = AppConfig.getMaxError();
            double learningRate = AppConfig.getLearningRate();

            ClusterSetting clusterSetting = new ClusterSetting(inpuClusterSize, outputClusterSize, maxTrainCycleNumber, printOutInterval, widthConstant, maxError, learningRate);
            return clusterSetting;
        }
    }
}
