using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POPTVR.Entities;
using POPTVR.Utilities;
using POPTVR.PoptvrArchitecture.BasisNode;

namespace POPTVR.PoptvrArchitecture
{
    class RsPopModel
    {
        public static PoptvrModel AttributeReduction(PoptvrModel poptvr, DataSet dataset)
        {
            Console.WriteLine("Start RSPOP reduction ...");

            double[] CO = new double[dataset.NumberOfOutputNodes * poptvr.OutputClusterSize];
            double[] COr = new double[dataset.NumberOfOutputNodes * poptvr.OutputClusterSize];

            string res = "Removed from total " + poptvr.RuleLayer.Length + "\n";

            for (int k = 0; k < poptvr.ConditionLayer.Length; k++)
            {
                bool deterioration = false;
                for (int i = 0; i < dataset.TotalNumberOfRecords; i++)
                {
                    Console.Write("at condition " + k + "\t" + "record " + i + "\n");

                    poptvr.forwardFeed(SystemFunctions.getArrayAtRow(dataset.Inputdata, i));
                    for (int j = 0; j < dataset.NumberOfOutputNodes * poptvr.OutputClusterSize; j++)
                    {
                        CO[j] = poptvr.ConsequenceLayer[j].Output;
                    }

                    poptvr.ConditionLayer[k].Blocked = true;
                    poptvr.forwardFeed(SystemFunctions.getArrayAtRow(dataset.Inputdata, i));
                    for (int j = 0; j < dataset.NumberOfOutputNodes * poptvr.OutputClusterSize; j++)
                    {
                        COr[j] = poptvr.ConsequenceLayer[j].Output;

                        if (COr[j] < CO[j])
                        {
                            deterioration = true;
                        }
                        else
                        {

                            for (int m = 0; m < CO.Length; m++)
                            {
                                if (COr[j] > CO[m])
                                {
                                    deterioration = true;
                                    break;
                                }
                            }
                        }

                        if (deterioration)
                        {
                            break;
                        }
                    }

                    if (deterioration)
                    {
                        break;
                    }
                }

                if (!deterioration)
                {
                    res += (k + " label is removed\n");
                    poptvr.ConditionLayer[k].Blocked = true;
                }
                if (deterioration)
                {
                    poptvr.ConditionLayer[k].Blocked = false;
                }
            }




            /*
            for (int k = 0; k < poptvr.RuleLayer.Length; k++)
            {


                bool deterioration = false;

                for (int i = 0; i < dataset.TotalNumberOfRecords; i++)
                {
                    Console.WriteLine("at rule " + k + "\t" + "record " + i + "\n");

                    poptvr.forwardFeed(SystemFunctions.getArrayAtRow(dataset.Inputdata, i));
                    for (int j = 0; j < dataset.NumberOfOutputNodes * poptvr.OutputClusterSize; j++)
                    {
                        CO[j] = poptvr.ConsequenceLayer[j].Output;
                    }

                    poptvr.RuleLayer[k].Blocked = true;
                    poptvr.forwardFeed(SystemFunctions.getArrayAtRow(dataset.Inputdata, i));
                    for (int j = 0; j < dataset.NumberOfOutputNodes * poptvr.OutputClusterSize; j++)
                    {
                        COr[j] = poptvr.ConsequenceLayer[j].Output;

                        if (COr[j] < CO[j])
                        {
                            deterioration = true;
                        }
                        else
                        {

                            for (int m = 0; m < CO.Length; m++)
                            {
                                if (COr[j] > CO[m])
                                {
                                    deterioration = true;
                                    break;
                                }
                            }
                        }

                        if (deterioration)
                        {
                            break;
                        }
                    }

                    if (deterioration)
                    {
                        break;
                    }

                }

                if (!deterioration)
                {
                    poptvr.RuleLayer[k].Blocked = true;
                    break;
                }
                if (deterioration)
                {
                    poptvr.RuleLayer[k].Blocked = false;
                }

            }
            */


            Utilities.FileWriter.WriteToFile(AppConfig.getOutputFolder() + "Removed Attribute.txt", res);
            return poptvr;
        }


        public static PoptvrModel RuleReduction(PoptvrModel poptvr, DataSet dataset)
        {
            Console.WriteLine("Start RSPOP reduction ..."); 

            double[] CO = new double[dataset.NumberOfOutputNodes * poptvr.OutputClusterSize];
            double[] COr = new double[dataset.NumberOfOutputNodes * poptvr.OutputClusterSize];

            string res = "Removed from total " + poptvr.RuleLayer.Length + "\n";

            for (int k = 0; k < poptvr.RuleLayer.Length; k++)
            {          


                for (int i = 0; i < dataset.TotalNumberOfRecords; i++)
                {
                    Console.Write("at condition " + k + "\t" + "record " + i + "\n");

                    poptvr.forwardFeed(SystemFunctions.getArrayAtRow(dataset.Inputdata, i));
                    for (int j = 0; j < dataset.NumberOfOutputNodes * poptvr.OutputClusterSize; j++)
                    {
                        CO[j] = poptvr.ConsequenceLayer[j].Output;
                    }
               
                    for (int l = 0; l < poptvr.InputClusterSize; l++)
                    {
                        bool deterioration = false;
                        poptvr.RuleLayer[k].PointBlocked = true;
                        poptvr.RuleLayer[k].PointBlockedAt = l;

                        poptvr.forwardFeed(SystemFunctions.getArrayAtRow(dataset.Inputdata, i));
                        for (int j = 0; j < dataset.NumberOfOutputNodes * poptvr.OutputClusterSize; j++)
                        {
                            COr[j] = poptvr.ConsequenceLayer[j].Output;

                            if (COr[j] < CO[j])
                            {
                                deterioration = true;
                            }
                            else
                            {

                                for (int m = 0; m < CO.Length; m++)
                                {
                                    if (COr[j] > CO[m])
                                    {
                                        deterioration = true;
                                        break;
                                    }
                                }
                            }

                            if (deterioration)
                            {
                                break;
                            }
                        }

                        if (!deterioration)
                        {
                            res += (l + " is remove at " + k + "\n");
                            poptvr.RuleLayer[k].PointBlocked = false;
                        }
                        if (deterioration)
                        {
                            poptvr.RuleLayer[k].PointBlocked = false;
                        }
                    }          
                }

            }

            Utilities.FileWriter.WriteToFile(AppConfig.getOutputFolder() + "Removed Rule.txt", res);
            return poptvr;
        }
    }
}
