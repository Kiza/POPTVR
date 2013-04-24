using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.Entities;
using POPTVR.Utilities;
using POPTVR.PoptvrArchitecture;
using POPTVR.PoptvrArchitecture.ClusterModel;

namespace POPTVR
{
    class Program
    {
        public static void Main(string[] args)
        {
            starter();

            Console.Write("\nPress any key to continue . . . ");
            Console.ReadKey();
        }

        public static void starter()
        {
            ClusterSetting clusterSetting = ClusterSetting.getClusterSettings();

            string filename = AppConfig.getTrainFilename();
            DataSet dataset = DataFileReader.ReadData(filename);

            ClusterFacadeInterface clusterFacade;

            clusterFacade = new MlvqFacade();
            //clusterFacade = new SOclusterFacade();
            //clusterFacade = new RctFacade();
            //clusterFacade = new SrctFacade();

            PoptvrSystem popSystem = new PoptvrSystem(dataset, clusterSetting, clusterFacade);
            popSystem.InitClusters();
            popSystem.PopLearn();

            filename = AppConfig.getTestFilename();
            dataset = DataFileReader.ReadData(filename);
            popSystem.POPTVR = RsPopModel.AttributeReduction(popSystem.POPTVR, dataset);
            popSystem.POPTVR = RsPopModel.RuleReduction(popSystem.POPTVR, dataset);


            popSystem.PopTest(dataset);
        }
    }
}
