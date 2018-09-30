using System;
using System.Collections.Generic;
using RiversideDistributer.FC.Enums;

namespace RiversideDistributer.FC.Interfaces
{
    public interface iIsle
    {
        isleBayConfiguration IsleBayConfig { get; }

        iBay GetBay(int bayNumber);

        Dictionary<string, string> GetIsleSkuPlacement();

        iIsle NextIsle();

        iIsle PreviousIsle();

        int GetSkusAllowedPerLocation();

        int IsleID { get; }

        iWarehouse InWarehouse { get; }

        List<iBay> Bays { get; }

        bool HasOpenSpace(int shelfid);
    }
}
