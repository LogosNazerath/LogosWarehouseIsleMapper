using System;
using System.Collections.Generic;

namespace RiversideDistributer.FC.Interfaces
{
    public interface iPosition
    {
        iPosition NextPosition();

        iPosition PreviousPosition();

        string Location();

        Dictionary<string, string> GetSkuPlacement();

        iShelf InShelf { get; }

        int PositionID { get; }

        List<string> AssignedSkus { get; }

        bool HasOpenSpace();
    }
}
