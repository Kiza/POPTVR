using System;
using POPTVR.Entities;
namespace POPTVR.PoptvrArchitecture.ClusterModel
{
    interface ClusterFacadeInterface
    {
        ClusterSetting ClusterSetting { set; }
        DataSet DataSet { set; }

        ClusterInterface getInputCluster();
        ClusterInterface getOutputCluster();
    }
}
