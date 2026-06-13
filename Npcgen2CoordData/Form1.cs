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
        CoordData coord = new CoordData();
        private string coordPath = "";
        private bool coordLoaded = false;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
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

        private void SetButtonsEnabled(bool enabled)
        {
            btnOpenCoord.Enabled = enabled;
            btnBulkImport.Enabled = enabled;
            btnManualImport.Enabled = enabled && coordLoaded;
        }

        private void btnOpenCoord_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Open coord_data.txt",
                Filter = "Coord_data|*.txt|All Files|*.*"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            coordPath = ofd.FileName;
            SetButtonsEnabled(false);

            new Thread(() =>
            {
                coord.Read(coordPath);
                coordLoaded = true;
                lblCoordPath.Text = coordPath;
                SetButtonsEnabled(true);
            }).Start();
        }

        /// <summary>
        /// Bulk Import: select a root folder, auto-scan all subfolders for npcgen.data,
        /// use subfolder name as location, import all without manual input.
        /// </summary>
        private void btnBulkImport_Click(object sender, EventArgs e)
        {
            string rootFolder = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter or paste the root folder path containing map subfolders with npcgen.data:\n\n" +
                "Example: H:\\DATA PW\\PWServer\\Tools PW\\npcgen2coord_data\\165",
                "Bulk Import Folder",
                "");

            if (string.IsNullOrEmpty(rootFolder)) return;
            rootFolder = rootFolder.Trim().Trim('"');

            if (!Directory.Exists(rootFolder))
            {
                MessageBox.Show("Folder not found:\n" + rootFolder, "Invalid Folder",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Find all npcgen.data files in the selected folder and its subfolders.
            string[] npcgenFiles = Directory.GetFiles(rootFolder, "npcgen.data", SearchOption.AllDirectories)
                .OrderBy(x => x)
                .ToArray();

            if (npcgenFiles.Length == 0)
            {
                MessageBox.Show("No npcgen.data files found in the selected folder.", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<KeyValuePair<string, string>> importList = new List<KeyValuePair<string, string>>();
            foreach (string npcgenPath in npcgenFiles)
            {
                string map = Path.GetFileName(Path.GetDirectoryName(npcgenPath));
                if (string.IsNullOrEmpty(map)) map = Path.GetFileName(rootFolder);
                if (string.IsNullOrEmpty(map)) map = "world";

                importList.Add(new KeyValuePair<string, string>(npcgenPath, map));
            }

            string duplicateLocations = string.Join(", ", importList
                .GroupBy(x => x.Value)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToArray());

            if (!string.IsNullOrEmpty(duplicateLocations))
            {
                MessageBox.Show(
                    "Duplicate location(s) found: " + duplicateLocations + "\n\n" +
                    "Bulk import needs one npcgen.data per location. Use Manual Import if you need custom location names.",
                    "Duplicate Locations",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string bulkFolder = Path.Combine(Application.StartupPath, "bulk");
            Directory.CreateDirectory(bulkFolder);

            string outputPath = Path.Combine(bulkFolder, "coord_data.txt");
            coord.Reset("ID\tMap\tX\tY\tZ");
            coordPath = outputPath;
            coordLoaded = true;
            lblCoordPath.Text = outputPath;

            string preview = string.Join("\n", importList
                .Take(25)
                .Select(x => x.Value + "  <-  " + x.Key)
                .ToArray());

            if (importList.Count > 25)
                preview += "\n... and " + (importList.Count - 25) + " more";

            if (MessageBox.Show(
                "Bulk import will create/update:\n" + outputPath + "\n\n" +
                "Locations:\n\n" + preview + "\n\nContinue?",
                "Confirm Bulk Import",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            SetButtonsEnabled(false);

            new Thread(() =>
            {
                try
                {
                    for (int f = 0; f < importList.Count; f++)
                    {
                        ImportNpcgen(importList[f].Key, importList[f].Value, f + 1, importList.Count);
                        coord.Save(coordPath);
                        SaveLocationFile(importList[f].Value, bulkFolder);
                    }

                    ProgressValue(0);
                    ProgressText($"Done - {importList.Count} location(s) imported and saved");
                    MessageBox.Show("Bulk import completed and saved to:\n" + coordPath, "Done",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    ProgressText("Error: " + ex.Message);
                    MessageBox.Show(ex.ToString(), "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SetButtonsEnabled(true);
                }
            }).Start();
        }

        /// <summary>
        /// Manual Import: select npcgen.data file(s), input location for each.
        /// </summary>
        private void btnManualImport_Click(object sender, EventArgs e)
        {
            if (!coordLoaded) return;

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select npcgen.data file(s)",
                Filter = "Npcgen|npcgen.data|All Files|*.*",
                Multiselect = true
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string[] npcgenFiles = ofd.FileNames;

            // Ask location for each file
            List<KeyValuePair<string, string>> importList = new List<KeyValuePair<string, string>>();
            foreach (string npcgenPath in npcgenFiles)
            {
                string folder = Path.GetFileName(Path.GetDirectoryName(npcgenPath));
                string defaultLocation = folder ?? "world";

                string map = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Location for: {npcgenPath}\n\nAuto-detected from folder: \"{folder}\"",
                    "Location Name", defaultLocation);
                if (string.IsNullOrEmpty(map)) return;

                importList.Add(new KeyValuePair<string, string>(npcgenPath, map));
            }

            SetButtonsEnabled(false);

            new Thread(() =>
            {
                try
                {
                    for (int f = 0; f < importList.Count; f++)
                    {
                        ImportNpcgen(importList[f].Key, importList[f].Value, f + 1, importList.Count);
                        coord.Save(coordPath);
                        SaveLocationFile(importList[f].Value, Application.StartupPath);
                    }

                    ProgressValue(0);
                    ProgressText($"Done - {importList.Count} file(s) imported and saved");
                    MessageBox.Show("Manual import completed and saved to:\n" + coordPath, "Done",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    ProgressText("Error: " + ex.Message);
                    MessageBox.Show(ex.ToString(), "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SetButtonsEnabled(true);
                }
            }).Start();
        }

        /// <summary>
        /// Core import logic: load npcgen, remove old location entries, import new data.
        /// </summary>
        private void ImportNpcgen(string npcgenPath, string map, int current, int total)
        {
            ProgressText($"[{current}/{total}] Loading {map}/npcgen.data...");

            NpcGen npc = new NpcGen();
            npc.ProgressMax += ProgressMax;
            npc.ProgressNext += ProgressNext;
            npc.ProgressText += ProgressText;
            npc.ProgressValue += ProgressValue;
            npc.ReadNpcgen(new BinaryReader(File.OpenRead(npcgenPath)));

            // Remove old entries for this location
            ProgressText($"[{current}/{total}] Removing old '{map}' entries...");
            foreach (var key in coord.Entrys.Keys.ToList())
            {
                coord.Entrys[key].RemoveAll(entry => entry.MapNumber == map);
                if (coord.Entrys[key].Count == 0)
                    coord.Entrys.Remove(key);
            }

            // Import new data
            ProgressText($"[{current}/{total}] Importing '{map}'...");
            ProgressMax(npc.NpcMobList.Count + npc.ResourcesList.Count);
            ProgressValue(0);

            npc.NpcMobList.ForEach(x =>
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

            npc.ResourcesList.ForEach(x =>
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
        }

        private void SaveLocationFile(string map, string outputFolder)
        {
            string fileName = map;
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(invalidChar, '_');

            Directory.CreateDirectory(outputFolder);
            string path = Path.Combine(outputFolder, fileName + ".txt");
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (KeyValuePair<string, List<CoordDataEntry>> entry in coord.Entrys)
                {
                    entry.Value
                        .Where(x => x.MapNumber == map)
                        .ToList()
                        .ForEach(x => sw.WriteLine(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                            "{0}\t{1}\t{2:F2}\t{3:F2}\t{4:F2}",
                            entry.Key, x.MapNumber, x.X, x.Y, x.Z)));
                }
            }
        }
    }
}
