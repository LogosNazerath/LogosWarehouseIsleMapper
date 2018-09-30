using System;
using System.Collections.Generic;
using System.Linq;
using RiversideDistributer.FC;
using RiversideDistributer.FC.Interfaces;

namespace RiversideDistributer.FC.Classes
{
    public class Position : iPosition
    {
        private int positionID;
        private iShelf inShelf;
        private List<string> assignedSkus = new List<string>();

        public Position(iShelf inshelf, int positionid, List<string> assignedskus)
        {
            positionID = positionid;
            inShelf = inshelf;
            assignedSkus = assignedskus;
        }

        public Position(iShelf inshelf, int positionid)
        {
            positionID = positionid;
            inShelf = inshelf;
        }

        public List<string> AssignedSkus => assignedSkus;
        public iShelf InShelf => inShelf;
        int iPosition.PositionID => positionID;

        public Dictionary<string, string> GetSkuPlacement()
        {
            Dictionary<string, string> skuPlacement = new Dictionary<string, string>(); 
            foreach(string sku in assignedSkus)
            {
                skuPlacement.Add(sku,Location());
            }
            return skuPlacement;
        }

        public string Location()
        {
            return FCHelper.GetLocation(
                InShelf.InBay.InIsle.InWarehouse.WmsBrand,
                InShelf.InBay.InIsle.IsleID.ToString("D2"),
                InShelf.InBay.BayID.ToString("D2"),
                InShelf.GetShelfName(),
                positionID.ToString("D2"));
        }

        public iPosition NextPosition()
        {
            return this.inShelf.Positions.FirstOrDefault(p => p.PositionID == (this.positionID + 1));
        }

        public iPosition PreviousPosition()
        {
            return this.inShelf.Positions.FirstOrDefault(p => p.PositionID == (this.positionID - 1));
        }

        public bool HasOpenSpace()
        {
            if(assignedSkus.Count() < this.inShelf.InBay.InIsle.GetSkusAllowedPerLocation())
            {
                return true;
            }

            return false;
        }
    }
}
