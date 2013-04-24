using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.Entities;

namespace POPTVR.PoptvrArchitecture.ClusterModel
{
    class SrctFacade : ClusterFacadeInterface
    {
        private ClusterSetting clusterSetting;
        private DataSet dataset;

        public SrctFacade()
        {
        }
        public SrctFacade(DataSet dataset, ClusterSetting clusterSetting)
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
            ClusterInterface cluster = new SRCT(dataset.NumberOfInputNodes, this.clusterSetting.InputClusterSize, dataset.NumberOfOutputNodes);
            cluster.computeInitialCentroids(dataset.Inputdata, dataset.DesiredOutputs, dataset.TotalNumberOfRecords);
            cluster.computeFinalCentroids(dataset.Inputdata, dataset.DesiredOutputs, dataset.TotalNumberOfRecords);

            return cluster;
        }
        public ClusterInterface getOutputCluster()
        {
            ClusterInterface cluster = new SRCT(dataset.NumberOfInputNodes, this.clusterSetting.OutputClusterSize, dataset.NumberOfOutputNodes);
            cluster.computeInitialCentroids(dataset.DesiredOutputs, dataset.DesiredOutputs, dataset.TotalNumberOfRecords);
            cluster.computeFinalCentroids(dataset.DesiredOutputs, dataset.DesiredOutputs, dataset.TotalNumberOfRecords);

            return cluster;
        }
    }
}
