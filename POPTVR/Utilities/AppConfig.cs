using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace POPTVR.Utilities
{
    public class AppConfig
    {
        public static int getInputClusterSize()
        {
            string appConfig = ConfigurationManager.AppSettings.Get("InputClusterSize");
            return  int.Parse(appConfig);
        }

        public static int getOutputClusterSize()
        {
            string appConfig = ConfigurationManager.AppSettings.Get("OutputClusterSize");
            return  int.Parse(appConfig);
        }

        public static int getMaxTrainCycleNumber()
        {
            string appConfig = ConfigurationManager.AppSettings.Get("MaxTrainCycleNumber");
            return int.Parse(appConfig);
        }

        public static int getPrintOutInterval()
        {
            string appConfig = ConfigurationManager.AppSettings.Get("PrintOutInterval");
            return int.Parse(appConfig);
        }

        public static double getWidthConstant()
        {
            string appConfig = ConfigurationManager.AppSettings.Get("WidthConstant");
            return double.Parse(appConfig);
        }

        public static double getMaxError()
        {
            string appConfig = ConfigurationManager.AppSettings.Get("MaxError");
            return double.Parse(appConfig);
        }

        public static double getLearningRate()
        {
            string appConfig = ConfigurationManager.AppSettings.Get("LearningRate");
            return double.Parse(appConfig);
        }

        public static string getTrainFilename()
        {
            string appConfig = ConfigurationManager.AppSettings.Get("TrainFilename");
            return appConfig;
        }

        public static string getTestFilename()
        {
            string appConfig = ConfigurationManager.AppSettings.Get("TestFilename");
            return appConfig;
        }

        public static string getOutputFolder()
        {
            string appConfig = ConfigurationManager.AppSettings.Get("OutputFolder");
            return appConfig;
        }
    }
}
