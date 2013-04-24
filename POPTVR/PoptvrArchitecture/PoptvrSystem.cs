using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.Entities;
using POPTVR.PoptvrArchitecture.ClusterModel;
using POPTVR.Utilities;

namespace POPTVR.PoptvrArchitecture
{
    class PoptvrSystem
    {
        private DataSet dataset;
        private ClusterSetting clusterSetting;
        private PoptvrModel poptvr;

        private ClusterFacadeInterface clusterFacade;
        private ClusterInterface inputCluster;
        private ClusterInterface outputCluster;

        public PoptvrModel POPTVR
        {
            get
            {
                return poptvr;
            }

            set
            {
                poptvr = value;
            }
        }

        public PoptvrSystem(DataSet dataset, ClusterSetting clusterSetting, ClusterFacadeInterface clusterFacade)
        {
            this.dataset = dataset;
            this.clusterSetting = clusterSetting;
            this.clusterFacade = clusterFacade;
        }

        public void InitClusters(ClusterFacadeInterface clusterFacade)
        {
            this.clusterFacade = clusterFacade;
            this.InitClusters();
        }
        public void InitClusters()
        {
            this.clusterFacade.ClusterSetting = this.clusterSetting;
            this.clusterFacade.DataSet = this.dataset;

            this.inputCluster = this.clusterFacade.getInputCluster();
            this.outputCluster = this.clusterFacade.getOutputCluster();
        }

        public PoptvrModel PopLearn()
        {
            this.poptvr = new PoptvrModel(this.dataset, this.clusterSetting);
            this.poptvr.initMembership(inputCluster, outputCluster);

            this.poptvr.popLearn();
            FileWriter.WritePopLearnOutput(this.poptvr);

            return this.poptvr;
        }
        public void PopTest(DataSet dataset)
        {
            string result = PoptvrModel.POPTest(this.poptvr, dataset);
            FileWriter.WritePopTestOutput(result);
        }

    }
}
