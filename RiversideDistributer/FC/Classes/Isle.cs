using System;
using System.Collections.Generic;
using System.Linq;
using RiversideDistributer.FC;
using RiversideDistributer.FC.Enums;
using RiversideDistributer.FC.Interfaces;

namespace RiversideDistributer.FC.Classes
{
    public class Isle : iIsle
    {
        private int isleID;
        private iWarehouse inWarehouse;
        private isleBayConfiguration config;
        private List<iBay> bays = new List<iBay>();

        public Isle(iWarehouse inwarehouse, int isleid, isleBayConfiguration islebayconfig)
        {
            inWarehouse = inwarehouse;
            isleID = isleid;
            config = islebayconfig;

            switch (config)
            {
                case isleBayConfiguration.Even:
                    for (int i = 2; i <= 24; i += 2)
                    {
                        bays.Add(new Bay(this, i));
                    }
                    break;
                case isleBayConfiguration.Odd:
                    for (int i = 1; i <= 24; i+= 2)
                    {
                        bays.Add(new Bay(this, i));
                    }
                    break;
                case isleBayConfiguration.All:
                default:
                    for (int i = 1; i <= 24; i++)
                    {
                        bays.Add(new Bay(this,i));
                    }
                    break;
            }

        }

        public isleBayConfiguration IsleBayConfig => config;
        public int IsleID => isleID;
        public iWarehouse InWarehouse => inWarehouse;
        public List<iBay> Bays => bays;

        public iBay GetBay(int bayID)
        {
            return bays.FirstOrDefault(b => b.BayID == bayID);
        }

        public Dictionary<string, string> GetIsleSkuPlacement()
        {
            Dictionary<string, string> SkuPositions = new Dictionary<string, string>();
            foreach (Bay b in this.bays)
            {
                SkuPositions = SkuPositions.Concat(b.GetShelfSkuPlacement()).ToDictionary(x => x.Key, x => x.Value);
            }
            return SkuPositions;
        }

        public bool HasOpenSpace(int shelfid)
        {
            foreach(iBay b in this.Bays)
            {
                if(b.HasOpenSpace(shelfid))
                {
                    return true;
                }
            }

            return false;
        }

        public int GetSkusAllowedPerLocation()
        {
            return FCHelper.GetSkusAllowedByIsle(isleID);
        }

        public iIsle NextIsle()
        {
          return this.inWarehouse.Isles.FirstOrDefault(i => i.IsleID == (this.IsleID + 1));
        }

        public iIsle PreviousIsle()
        {
            return this.inWarehouse.Isles.FirstOrDefault(i => i.IsleID == (this.IsleID - 1));
        }
    }
}
