using System;
using System.Linq;
using System.Collections.Generic;
using RiversideDistributer.FC.Interfaces;
using RiversideDistributer.FC.Enums;

namespace RiversideDistributer.FC.Classes
{
    public class Shelf : iShelf
    {
        private int shelfID;
        private iBay inBay;
        private List<iPosition> positions = new List<iPosition>(); 

        public Shelf(iBay inbay, int shelfid)
        {
            shelfID = shelfid;
            inBay = inbay;

            for (int i = 1; i <= 8; i++)
            {
                positions.Add(new Position(this,i));
            }
        }

        public iBay InBay => inBay;
        public List<iPosition> Positions => positions;
        public int ShelfID => shelfID;

        public iPosition GetPosition(int positionID)
        {
            return positions.FirstOrDefault(p => p.PositionID == positionID);
        }

        public Dictionary<string, string> GetPositionSkuPlacement()
        {
            Dictionary<string, string> SkuPositions = new Dictionary<string, string>();
            foreach(Position p in this.positions)
            {
                SkuPositions = SkuPositions.Concat(p.GetSkuPlacement()).ToDictionary(x=>x.Key,x=>x.Value);
            }
            return SkuPositions;
        }

        public virtual string GetShelfName()
        {
            return shelfID.ToString();
        }

        public iShelf NextShelf()
        {
            switch (this.shelfID)
            {
                case 5:
                    return this.inBay.Shelves.FirstOrDefault(s => s.ShelfID == 1);
                case 2:
                    return this.inBay.Shelves.FirstOrDefault(s => s.ShelfID == 5);
                case 3:
                    return this.inBay.Shelves.FirstOrDefault(s => s.ShelfID == 4);
                case 4:
                    return this.inBay.Shelves.FirstOrDefault(s => s.ShelfID == 2);
                default:
                case 1:
                    return null;

            }
        }

        public iShelf PreviousShelf()
        {
            switch (this.shelfID)
            {
                case 1:
                    return this.inBay.Shelves.FirstOrDefault(s => s.ShelfID == 5);
                case 2:
                    return this.inBay.Shelves.FirstOrDefault(s => s.ShelfID == 4);
                case 4:
                    return this.inBay.Shelves.FirstOrDefault(s => s.ShelfID == 3);
                case 5:
                    return this.inBay.Shelves.FirstOrDefault(s => s.ShelfID == 2);
                default:
                case 3:
                    return null;

            }
        }

        public bool HasOpenSpace()
        {
            foreach(iPosition p in this.positions)
            {
                if(p.HasOpenSpace())
                {
                    return true;
                }
            }

            return false;
        }

    }
    public class SnapShelf : Shelf
    {
        private snapShelves shelfLetter;

        public SnapShelf(iBay inbay, int shelfid) : base(inbay, shelfid)
        {
            shelfLetter = (snapShelves)shelfid;
        }

        public override string GetShelfName()
        {
            return shelfLetter.ToString();
        }

        public snapShelves ShelfLetter => shelfLetter;
    }
}
