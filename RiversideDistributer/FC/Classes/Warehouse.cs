using System;
using System.Collections.Generic;
using System.Linq;
using RiversideDistributer.FC.Enums;
using RiversideDistributer.FC.Interfaces;

namespace RiversideDistributer.FC.Classes
{
    public class Warehouse : iWarehouse
    {
        private List<iIsle> isles = new List<iIsle>();
        private string warehouse;
        private wmsBrand wmsBrand;

        public Warehouse(string warehouseName, wmsBrand warehouseSystem, int isleMin, int isleMax, List<int> oddIsles, List<int> evenIsles)
        {
            warehouse = warehouseName;
            wmsBrand = warehouseSystem;

            isleBayConfiguration config = isleBayConfiguration.All;

            for (int i = isleMin; i <= isleMax;i++)
            {
                if(oddIsles.Contains(i))
                {
                    config = isleBayConfiguration.Odd;
                }
                else if(evenIsles.Contains(i))
                {
                    config = isleBayConfiguration.Even;
                }
                else
                {
                    config = isleBayConfiguration.All;
                }
                isles.Add(new Isle(this, i, config));
            }
        }

        public wmsBrand WmsBrand => wmsBrand;

        public List<iIsle> Isles => isles;

        string iWarehouse.Warehouse => warehouse;

        public iIsle GetIsle(int isleNumber)
        {
            return this.isles.FirstOrDefault(i => i.IsleID == isleNumber);
        }

        public Dictionary<string, string> GetWarehouseSkuPlacement()
        {
            Dictionary<string, string> SkuPositions = new Dictionary<string, string>();
            foreach (iIsle i in this.isles)
            {
                SkuPositions = SkuPositions.Concat(i.GetIsleSkuPlacement()).ToDictionary(x => x.Key, x => x.Value);
            }
            return SkuPositions;
        }

        public bool HasOpenSpace()
        {
            for (int j = 1; j <= 5; j++)
            {
                foreach(iIsle i in this.Isles)
                {
                    if(i.HasOpenSpace(j))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public bool HasOpenSpace(int shelfid)
        {
            foreach (iIsle i in Isles)
            {
                if (i.HasOpenSpace(shelfid))
                {
                    return true;
                }
            }

            return false;
        }
        public bool HasOpenSpace(int shelfid, Tuple<int,int> range)
        {
            for (int j = range.Item1; j <= range.Item2; j++)
            {
                if (Isles.FirstOrDefault(i => i.IsleID == j).HasOpenSpace(shelfid))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
