using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.Entities;

namespace POPTVR.PoptvrArchitecture.ClusterModel
{
    class RctFacade : ClusterFacadeInterface
    {
        private ClusterSetting clusterSetting;
        private DataSet dataset;

        public RctFacade()
        {
        }
        public RctFacade(DataSet dataset, ClusterSetting clusterSetting)
        {
            this.clusterSetting = clusterSetting;
            this.dataset = dataset;
        }

        public ClusterSetting ClusterSetting
        {
            set
            {
                this.clusterSetting = value;
            }
        }
        public DataSet DataSet
        {
            set
            {
                this.dataset = value;
            }
        }

        public ClusterInterface getInputCluster()
        {
            ClusterInterface cluster = new RCT(dataset.NumberOfInputNodes, this.clusterSetting.InputClusterSize, dataset.TotalNumberOfRecords);
            cluster.computeInitialCentroids(dataset.Inputdata, new double[0, 0], dataset.TotalNumberOfRecords);
            cluster.computeFinalCentroids(dataset.Inputdata, dataset.DesiredOutputs, dataset.TotalNumberOfRecords);

            return cluster;
        }
        public ClusterInterface getOutputCluster()
        {
            ClusterInterface cluster = new RCT(dataset.NumberOfOutputNodes, this.clusterSetting.OutputClusterSize, dataset.TotalNumberOfRecords);
            cluster.computeInitialCentroids(dataset.DesiredOutputs, new double[0, 0], dataset.TotalNumberOfRecords);
            cluster.computeFinalCentroids(dataset.DesiredOutputs, dataset.DesiredOutputs, dataset.TotalNumberOfRecords);

            return cluster;
        }
    }
}
