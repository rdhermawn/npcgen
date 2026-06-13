using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Npcgen2CoordData
{
    public delegate void SetProgressMax(int value);
    public delegate void SetProgressNext();
    public delegate void SetProgressValue(int value);
    public delegate void SetProgressText(string value);

    public partial class Form1 : Form
    {
        NpcGen npcgen = new NpcGen();
        CoordData coord = new CoordData();

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            npcgen.ProgressMax += ProgressMax;
            npcgen.ProgressNext += ProgressNext;
            npcgen.ProgressText += ProgressText;
            npcgen.ProgressValue += ProgressValue;
            coord.ProgressMax += ProgressMax;
            coord.ProgressNext += ProgressNext;
            coord.ProgressText += ProgressText;
            coord.ProgressValue += ProgressValue;
        }

        private void ProgressValue(int value)
        {
            Progress.Value = value;
        }

        private void ProgressText(string value)
        {
            Status.Text = value;
        }

        private void ProgressNext()
        {
            ++Progress.Value;
        }

        private void ProgressMax(int value)
        {
            Progress.Maximum = value;
        }

        private void btnImportAndSave_Click(object sender, EventArgs e)
        {
            // 1. Select coord_data.txt
            OpenFileDialog ofdCoord = new OpenFileDialog()
            {
                Title = "Open coord_data.txt",
                Filter = "Coord_data|*.txt|All Files|*.*"
            };
            if (ofdCoord.ShowDialog() != DialogResult.OK) return;

            // 2. Select npcgen.data
            OpenFileDialog ofdNpcgen = new OpenFileDialog()
            {
                Title = "Open npcgen.data",
                Filter = "Npcgen|npcgen.data|All Files|*.*"
            };
            if (ofdNpcgen.ShowDialog() != DialogResult.OK) return;

            // 3. Input location name
            string map = Microsoft.VisualBasic.Interaction.InputBox(
                "Location name where mobs are located, e.g. world, a78, a64",
                "Location Name", "world");
            if (string.IsNullOrEmpty(map)) return;

            string coordPath = ofdCoord.FileName;
            string npcgenPath = ofdNpcgen.FileName;

            btnImportAndSave.Enabled = false;

            new Thread(() =>
            {
                // 4. Load coord_data.txt
                coord.Read(coordPath);

                // 5. Load npcgen.data
                npcgen.ReadNpcgen(new BinaryReader(File.OpenRead(npcgenPath)));

                // 6. Remove all old entries with matching location
                ProgressText($"Removing old '{map}' entries...");
                foreach (var key in coord.Entrys.Keys.ToList())
                {
                    coord.Entrys[key].RemoveAll(entry => entry.MapNumber == map);
                    if (coord.Entrys[key].Count == 0)
                        coord.Entrys.Remove(key);
                }

                // 7. Import new data from npcgen
                ProgressText("Importing...");
                ProgressMax(npcgen.NpcMobList.Count + npcgen.ResourcesList.Count);
                ProgressValue(0);

                npcgen.NpcMobList.ForEach(x =>
                {
                    ProgressNext();
                    x.MobDops.ForEach(y =>
                    {
                        string id = y.Id.ToString();
                        if (!coord.Entrys.ContainsKey(id))
                            coord.Entrys[id] = new List<CoordDataEntry>();
                        coord.Entrys[id].Add(new CoordDataEntry()
                        {
                            MapNumber = map,
                            X = x.X_position,
                            Y = x.Y_position,
                            Z = x.Z_position
                        });
                    });
                });

                npcgen.ResourcesList.ForEach(x =>
                {
                    ProgressNext();
                    x.ResExtra.ForEach(y =>
                    {
                        string id = y.Id.ToString();
                        if (!coord.Entrys.ContainsKey(id))
                            coord.Entrys[id] = new List<CoordDataEntry>();
                        coord.Entrys[id].Add(new CoordDataEntry()
                        {
                            MapNumber = map,
                            X = x.X_position,
                            Y = x.Y_position,
                            Z = x.Z_position
                        });
                    });
                });

                // 8. Save to same file
                coord.Save(coordPath);

                ProgressValue(0);
                ProgressText("Done");
                btnImportAndSave.Enabled = true;
            }).Start();
        }
    }
}
