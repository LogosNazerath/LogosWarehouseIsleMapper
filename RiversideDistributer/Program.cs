using System;
using System.IO;
using System.Linq;
using RiversideDistributer.Config;
using RiversideDistributer.FC.Classes;
using RiversideDistributer.FC.Interfaces;
using RiversideDistributer.Assigner;


namespace RiversideDistributer
{
    public class MainClass
    {
        public static iWarehouse RVS1 { get; set; }

        public static void Main(string[] args)
        {
            DynamicStartingConfig config = new DynamicStartingConfig();

            RVS1 = new Warehouse(config.FCName,
                                 config.BrandInUse,
                                 config.FirstIsle,
                                 config.LastIsle,
                                 config.OddIsles,
                                 config.EvenIsles
                                );

            AssignmentLogic LogicEngine = new AssignmentLogic();

            LogicEngine.GetSkus();
            LogicEngine.AssignIsles(RVS1);
            LogicEngine.WriteSkus(RVS1);

            int maxIsle = RVS1.Isles.Max(i => i.IsleID);
            int maxBayInMaxIsle = RVS1.Isles.FirstOrDefault(i => i.IsleID == maxIsle).Bays.Max(b => b.BayID);
            int maxShelf = 5;
            int maxPosition = 8;

            Console.WriteLine("The last shelf position is: " + RVS1.Isles.FirstOrDefault(i => i.IsleID == maxIsle)
                              .Bays.FirstOrDefault(b => b.BayID == maxBayInMaxIsle)
                              .Shelves.FirstOrDefault(s => s.ShelfID == maxShelf)
                              .Positions.FirstOrDefault(p => p.PositionID == maxPosition).Location());

            Console.WriteLine(Directory.GetCurrentDirectory());
        }
    }
}
