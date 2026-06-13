using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Npcgen2CoordData
{
    public class CoordDataEntry
    {
        public string MapNumber = "";
        public float X = 0;
        public float Y = 0;
        public float Z = 0;
    }

    class CoordData
    {
        private string Header = "";
        public Dictionary<string, List<CoordDataEntry>> Entrys = new Dictionary<string, List<CoordDataEntry>>();

        public event SetProgressMax ProgressMax;
        public event SetProgressValue ProgressValue;
        public event SetProgressNext ProgressNext;
        public event SetProgressText ProgressText;

        public void Read(string path)
        {
            Entrys.Clear();
            ProgressMax?.Invoke(File.ReadAllLines(path).Length);
            ProgressText("Loading coord_data.txt");
            StreamReader sr = new StreamReader(path);
            Header = sr.ReadLine();
            while (!sr.EndOfStream)
            {
                ProgressNext?.Invoke();
                string line = sr.ReadLine();
                if (string.IsNullOrEmpty(line) || line.Trim().Length == 0) continue;

                // Split by tab or whitespace, remove quotes
                string[] data = Regex.Replace(line.Replace("\"", "").Trim(), @"\s+", "\t").Split('\t');
                if (data.Length >= 5)
                {
                    if (!Entrys.ContainsKey(data[0]))
                        Entrys.Add(data[0], new List<CoordDataEntry>());
                    Entrys[data[0]].Add(new CoordDataEntry()
                    {
                        MapNumber = data[1],
                        X = float.Parse(data[2], CultureInfo.InvariantCulture),
                        Y = float.Parse(data[3], CultureInfo.InvariantCulture),
                        Z = float.Parse(data[4], CultureInfo.InvariantCulture)
                    });
                }
            }
            ProgressValue?.Invoke(0);
            ProgressText?.Invoke($"coord_data.txt loaded, {Entrys.Keys.Count} objects");
            sr.Close();
        }

        public void Save(string path)
        {
            ProgressMax?.Invoke(Entrys.Keys.Count);
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(Header);
            ProgressValue?.Invoke(0);
            foreach (KeyValuePair<string, List<CoordDataEntry>> entry in Entrys)
            {
                ProgressNext?.Invoke();
                entry.Value.ForEach(x =>
                {
                    sw.WriteLine(string.Format(CultureInfo.InvariantCulture,
                        "{0}\t{1}\t{2:F2}\t{3:F2}\t{4:F2}",
                        entry.Key, x.MapNumber, x.X, x.Y, x.Z));
                });
            }
            ProgressValue?.Invoke(0);
            ProgressText?.Invoke($"coord_data.txt saved successfully");
            sw.Close();
        }

        public void Reset(string header)
        {
            Header = header;
            Entrys.Clear();
            ProgressValue?.Invoke(0);
        }
    }
}
