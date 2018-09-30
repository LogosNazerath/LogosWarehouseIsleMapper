using System;
using System.Linq;
using System.Collections.Generic;
using RiversideDistributer.FC.Interfaces;
using RiversideDistributer.FC.Enums;

namespace RiversideDistributer.FC.Classes
{
    public class Bay : iBay
    {
        private int bayID;
        private iIsle inIsle;
        private List<iShelf> shelves = new List<iShelf>();

        /// <summary>
        /// Assigns the correct type of shelf based on warehouse WMS system. Lettered Shelves in Snap, Numbered Shelves in Gen2
        /// </summary>
        /// <param name="inisle">Inisle.</param>
        /// <param name="bayid">Bayid.</param>
        public Bay(iIsle inisle, int bayid)
        {
            inIsle = inisle;
            bayID = bayid;

            for (int i = 1; i <= 5; i++)
            {
                if (this.inIsle.InWarehouse.WmsBrand == wmsBrand.NextWMS)
                {
                    shelves.Add(new Shelf(this, i));
                }
                else
                {
                    shelves.Add(new currentShelf(this,i));
                }
            }
        }

        public iIsle InIsle => inIsle;
        public List<iShelf> Shelves => shelves;
        public int BayID => bayID;

        public iShelf GetShelf(int shelfID)
        {
            return shelves.FirstOrDefault(s => s.ShelfID == shelfID);
        }

        public Dictionary<string, string> GetShelfSkuPlacement()
        {
            Dictionary<string, string> SkuPositions = new Dictionary<string, string>();
            foreach (Shelf s in this.shelves)
            {
                SkuPositions = SkuPositions.Concat(s.GetPositionSkuPlacement()).ToDictionary(x => x.Key, x => x.Value);
            }
            return SkuPositions;
        }

        public bool HasOpenSpace(int shelfid)
        {
            if(this.shelves.FirstOrDefault(s => s.ShelfID == shelfid).HasOpenSpace())
            {
                return true;
            }

            return false;
        }

        public iBay NextBay()
        {
            throw new NotImplementedException();
        }

        public iBay PreviousBay()
        {
            throw new NotImplementedException();
        }
    }
}
