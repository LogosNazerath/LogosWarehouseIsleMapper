using System;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using RiversideDistributer.FC.Interfaces;

namespace RiversideDistributer.Assigner
{
    public class AssignmentLogic
    {
        public int currentIsle;
        public int currentBay;
        public bool frontBay;
        public int restartRangeAt;
        public Queue<string> Skus = new Queue<string>();

        /// <summary>
        /// Reads in Skus from csv file (SKU,Rank). Adds them to a queue to fill into positions in FC.
        /// </summary>
        public void GetSkus()
        {
            string sourceData = ConfigurationManager.AppSettings["InputFilePath"].ToString();

            if (!File.Exists(sourceData))
            {
                throw new ConfigurationErrorsException("Bad input file path: " + sourceData);
            }

            using (StreamReader sr = new StreamReader(sourceData))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');
                    Skus.Enqueue(values[0]);
                }
            }
        }

        public void WriteSkus(iWarehouse FC)
        {
            string outputDirectory = ConfigurationManager.AppSettings["OutputFolderPath"].ToString();

            if (!Directory.Exists(outputDirectory))
            {
                throw new ConfigurationErrorsException("Bad output directory: " + outputDirectory);
            }

            string outputFile = Path.Combine(outputDirectory, "skuPlacement.csv");

            using (FileStream fs = new FileStream(outputFile,FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach(iIsle isle in FC.Isles.OrderBy(i => i.IsleID))
                {
                    foreach(iBay bay in isle.Bays.OrderBy(b => b.BayID))
                    {
                        foreach (iShelf shelf in bay.Shelves.OrderBy(s => s.ShelfID))
                        {
                            foreach (iPosition position in shelf.Positions.OrderBy(p => p.PositionID))
                            {
                                string positionInformation = position.Location() + ",";
                                string entry = "";
                                foreach(string skuName in position.AssignedSkus)
                                {
                                    entry = positionInformation + skuName;
                                    sw.WriteLine(entry);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determine which range of Isle to be on, the rotation of the isles, and what the current shelf is, and which bay (front or back) to work from.
        /// </summary>
        /// <param name="FC">Fc.</param>
        public void AssignIsles(iWarehouse FC)
        {
            //Creates the ranges of isles SKU are prioritized into
            List<Tuple<int, int>> Ranges = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(FC.Isles.Min(f => f.IsleID),10),
                new Tuple<int, int>(11, 15),
                new Tuple<int, int>(16, 20),
                new Tuple<int, int>(21, 25),
                new Tuple<int, int>(26, 30),
                new Tuple<int, int>(31, FC.Isles.Max(f => f.IsleID))
            };

            //Loopes through each range of isles
            foreach(Tuple<int,int> range in Ranges)
            {
                //Goes shelf by shelf across FC.=
                for (int s = 1; s <= 5;s++)
                {
                    restartRangeAt = 0;
                    currentIsle = range.Item1;
                    if(FC.Isles.Min(v => v.IsleID) <= range.Item2 && FC.Isles.Min(v => v.IsleID) > range.Item1)
                    {
                        currentIsle = FC.Isles.Min(v => v.IsleID);
                    }
                    frontBay = true;
                    //checks that shelf in that range has space
                    while(FC.HasOpenSpace(s, range))
                    {
                        if (FC.Isles.Select(e => e.IsleID).ToList().Contains(currentIsle))
                        {
                            if (FC.Isles.FirstOrDefault(l => l.IsleID == currentIsle).HasOpenSpace(s))
                            {
                                assignPositionResults result = AssignBays(FC.Isles.FirstOrDefault(l => l.IsleID == currentIsle), s);

                                if(result == assignPositionResults.outOfSkusToAssign)
                                {
                                    return;
                                }
                            }
                        }

                        //cleanup at end of assignment each time
                        currentIsle += 3;
                        if (frontBay)
                        {
                            frontBay = false;
                        }
                        else
                        {
                            frontBay = true;
                        }

                        //Cleanup at end of looop through range
                        if (currentIsle > range.Item2)
                        {
                            restartRangeAt += 1;
                            if(restartRangeAt == 3)
                            {
                                restartRangeAt = 0;
                            }

                            currentIsle = range.Item1 + restartRangeAt;

                            if (frontBay)
                            {
                                frontBay = false;
                            }
                            else
                            {
                                frontBay = true;
                            }

                        }
                    }
                            
                }
            }
        }

        private assignPositionResults AssignBays(iIsle isle, int shelfid)
        {
            assignPositionResults result = assignPositionResults.assignedSkuToPosition;

            List<iBay> assignBays = new List<iBay>(isle.Bays);
            if (frontBay)
            {
                assignBays = assignBays.OrderBy(b => b.BayID).ToList();
            }
            else
            {
                assignBays = assignBays.OrderByDescending(b => b.BayID).ToList();
            }

            foreach(iBay b in assignBays)
            {
                if(b.HasOpenSpace(shelfid))
                {
                    result = AssignPosition(b, shelfid);
                    return result;
                }
            }

            return result;
        }

        private assignPositionResults AssignPosition(iBay bay, int shelfid)
        {
            foreach(iPosition p in bay.Shelves.FirstOrDefault(s => s.ShelfID == shelfid).Positions)
            {
                if(p.HasOpenSpace())
                {
                    p.AssignedSkus.Add(Skus.Dequeue());
                    if(Skus.Count() == 0)
                    {
                        Console.WriteLine("outta skus");
                        return assignPositionResults.outOfSkusToAssign;
                    }
                    return assignPositionResults.assignedSkuToPosition;
                }
            }
            return assignPositionResults.assignedSkuToPosition;
        }
    }
}
