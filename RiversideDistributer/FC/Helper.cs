using System;
using RiversideDistributer.FC.Enums;

namespace RiversideDistributer.FC
{
    public static class FCHelper
    {
        public static string GetLocation(wmsBrand Brand, string Isle, string Bay, string Shelf, string Position)
        {
            if (Brand == wmsBrand.NextWMS)
            {
                throw new NotImplementedException();
            }
            else if (Brand == wmsBrand.CurrentWMS)
            {
                return $"{Isle}-{Bay}-{Shelf}-{Position}";
            }

            throw new NotSupportedException("That WMS brand does not exist.");
        }

        public static int GetSkusAllowedByIsle(int isleID)
        {
            if (isleID <= 10)
            {
                return 1;
            }
            else if (isleID <= 15)
            {
                return 2;
            }
            else if (isleID <= 20)
            {
                return 3;
            }
            else if (isleID <= 25)
            {
                return 5;
            }
            else if (isleID <= 30)
            {
                return 7;
            }
            else
            {
                return 9;
            }

        }
    }
}