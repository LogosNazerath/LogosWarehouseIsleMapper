using System;
using System.Collections.Generic;

namespace RiversideDistributer.FC.Interfaces
{
    public interface iBay
    {
        iShelf GetShelf(int shelfID);

        Dictionary<string, string> GetShelfSkuPlacement();

        iBay NextBay();

        iBay PreviousBay();

        iIsle InIsle { get; }

        int BayID { get; }

        List<iShelf> Shelves { get; }

        bool HasOpenSpace(int shelfid);

    }
}
