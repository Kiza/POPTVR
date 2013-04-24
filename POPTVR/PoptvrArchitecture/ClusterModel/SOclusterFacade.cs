using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.Entities;

namespace POPTVR.PoptvrArchitecture.ClusterModel
{
    class SOclusterFacade : ClusterFacadeInterface
    {
        private ClusterSetting clusterSetting;
        private DataSet dataset;

        public SOclusterFacade()
        {
        }
        public SOclusterFacade(DataSet dataset, ClusterSetting clusterSetting)
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
            ClusterInterface cluster = new SOcluster(dataset.NumberOfInputNodes, clusterSetting.InputClusterSize);
            cluster.computeInitialCentroids(dataset.Inputdata, new double[0, 0], dataset.TotalNumberOfRecords);
            cluster.computeFinalCentroids(dataset.Inputdata, new double[0, 0], dataset.TotalNumberOfRecords);

            return cluster;
        }
        public ClusterInterface getOutputCluster()
        {
            ClusterInterface cluster = new SOcluster(dataset.NumberOfOutputNodes, clusterSetting.OutputClusterSize);
            cluster.computeInitialCentroids(dataset.DesiredOutputs, new double[0, 0], dataset.TotalNumberOfRecords);
            cluster.computeFinalCentroids(dataset.DesiredOutputs, new double[0, 0], dataset.TotalNumberOfRecords);

            return cluster;
        }
    }
}
