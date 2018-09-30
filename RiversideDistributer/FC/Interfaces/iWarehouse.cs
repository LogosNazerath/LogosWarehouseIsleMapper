using System;
using System.Collections.Generic;
using RiversideDistributer.FC.Enums;

namespace RiversideDistributer.FC.Interfaces
{
    public interface iWarehouse
    {

        iIsle GetIsle(int isleNumber);

        /// <summary>
        /// Get a dictionary listing the where SKUs need to be placed for this warehouse.
        /// </summary>
        /// <returns>The dictionary(string,string) as: Sku, Location</returns>
        Dictionary<string,string> GetWarehouseSkuPlacement();

        wmsBrand WmsBrand { get; }

        string Warehouse { get; }

        List<iIsle> Isles { get; }

        bool HasOpenSpace();
        bool HasOpenSpace(int shelfid);
        bool HasOpenSpace(int shelfid,Tuple<int,int> range);
    }
}
