using System;
using System.Collections.Generic;

namespace RiversideDistributer.FC.Interfaces
{
    public interface iShelf
    {

        iPosition GetPosition(int positionID);

        Dictionary<string, string> GetPositionSkuPlacement();

        iShelf NextShelf();

        iShelf PreviousShelf();

        int ShelfID { get; }

        iBay InBay { get; }

        List<iPosition> Positions { get; }

        string GetShelfName();

        bool HasOpenSpace();
    }
}
