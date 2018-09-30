using System;
using System.Configuration;
using System.Collections.Generic;
using RiversideDistributer.FC.Enums;


namespace RiversideDistributer.Config
{
    /// <summary>
    /// Deprecated. Harded coded and compiled config values.
    /// </summary>
    public static class StartingConfigValues
    {
        public static int minIsle = 2;
        public static int maxIsle = 34;
        public static string riversideName = "RVS1";
        public static List<int> oddIsles = new List<int>() { 34 };
        public static List<int> evenIsles = new List<int>() {};
        public static wmsBrand brandInUse = wmsBrand.Snap;
    }

    /// <summary>
    /// Retrieves configuration values from app.config
    /// </summary>
    public class DynamicStartingConfig
    {
        public int FirstIsle { get; }
        public int LastIsle { get; }
        public string FCName { get; }
        public List<int> OddIsles { get; }
        public List<int> EvenIsles { get; }
        public wmsBrand BrandInUse { get; }

        public DynamicStartingConfig()
        {
            var firstIsle = ConfigurationManager.AppSettings["FirstIsle"].ToString();
            var lastIsle = ConfigurationManager.AppSettings["LastIsle"].ToString();
            var fulfillmentCenterName = ConfigurationManager.AppSettings["FulfillmentCenterName"].ToString();
            var wms = ConfigurationManager.AppSettings["WMS"].ToString();
            var oddIsles = ConfigurationManager.AppSettings["OddIsles"].ToString();
            var evenIsles = ConfigurationManager.AppSettings["EvenIsles"].ToString();

            FirstIsle = GetFirstLastIsle(firstIsle, true);
            LastIsle = GetFirstLastIsle(lastIsle, false);

            FCName = fulfillmentCenterName;

            OddIsles = GetIslesList(oddIsles,true);
            EvenIsles = GetIslesList(evenIsles, false);

            BrandInUse = GetWmsBrand(wms);
        }

        private int GetFirstLastIsle(string isle, bool isFirstIsle)
        {
            if (Int32.TryParse(isle, out var parsedIsle))
            {
                return parsedIsle;
            }
            else
            {
                throw new System.Configuration.ConfigurationErrorsException("Bad " + (isFirstIsle ? "FirstIsle" : "LastIsle") + " app.config value: " + isle);
            }
        }

        private wmsBrand GetWmsBrand(string wms)
        {
            if (wms == "SNAP")
            {
                return wmsBrand.Snap;
            }
            else if (wms == "GEN2")
            {
                return wmsBrand.Gen2;
            }
            else
            {
                throw new System.Configuration.ConfigurationErrorsException("Bad WMS app.config value: " + wms);
            }
        }

        private List<int> GetIslesList(string listIsles, bool isOddIsleList)
        {
            var islesList = new List<int>();

            if (listIsles != "")
            {
                var listOfIsles = listIsles.Split(';');
                foreach (string isle in listOfIsles)
                {
                    if (Int32.TryParse(isle, out var parsedIsle))
                    {
                        islesList.Add(parsedIsle);
                    }
                    else
                    {
                        throw new System.Configuration.ConfigurationErrorsException("Bad " + (isOddIsleList ? "OddIsles" : "EvenIsles") + " app.config value: " + isle);
                    }
                }
            }

            return islesList;
        }
    }
}
