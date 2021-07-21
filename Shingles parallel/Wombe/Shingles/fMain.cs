using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Word.Application;
using Microsoft.Office.Interop.Word;
using System.Diagnostics;

namespace Shingles
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();

            hashSelectionMethodComboBox.Items.Add("Min Hash");
            hashSelectionMethodComboBox.Items.Add("Min Hash Par");
            hashSelectionMethodComboBox.Items.Add("Max Hash");
            hashSelectionMethodComboBox.Items.Add("Max Hash Par");
            hashSelectionMethodComboBox.Items.Add("Random Hash");
            hashSelectionMethodComboBox.Items.Add("Random Hash Par");
            hashSelectionMethodComboBox.Items.Add("CRC32");
            hashSelectionMethodComboBox.Items.Add("CRC32 Par");
        }
        Shingle s = new Shingle();
        private String GetDocText(Object path)
        {
            Object oMissing = Missing.Value;

            var oWord = new Application();
            var oWordDoc = new Document();

            oWordDoc = oWord.Documents.Add(ref path, ref oMissing, ref oMissing, ref oMissing);

            String result = oWordDoc.Range().Text;

            object doNotSaveChanges = WdSaveOptions.wdDoNotSaveChanges;

            oWordDoc.Close(doNotSaveChanges);
            oWord.Quit();

            return result;
        }


        private void btnLoadDocOne_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbOne.Text = GetDocText(ofd.FileName);
            }
        }

        private void btnLoadDocTwo_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbTwo.Text = GetDocText(ofd.FileName);
            }
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            //var s = new Shingle();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            DateTime time1 = DateTime.Now;
            lResult.Text = "Процент совпадений\nравен " + s.Compare(tbOne.Text, tbTwo.Text);
            DateTime time2 = DateTime.Now;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            string elapsedTime1 = time2.Subtract(time1).ToString();
            TimeLabel.Text = elapsedTime1;

            canonizationTimeLabel.Text = s.canonizeTime;
            shinglingTimeLabel.Text = s.shinglingTime;
            comparisonTimeLabel.Text = s.comparisonTime;
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            //var s = new Shingle();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var files = new List<string>(Directory.GetFiles(fbd.SelectedPath));

                for (int i = 0; i < files.Count - 1; i++)
                {
                    for (int j = i + 1; j < files.Count; j++)
                        tbFiles.Text += files[i] + " похож на файл " + files[j] + " на  " +
                                        s.Compare(GetDocText(files[i]), GetDocText(files[j])).ToString() + "%\n";
                }
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbFile.Text = GetDocText(ofd.FileName);
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            //var s = new Shingle();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var files = new List<string>(Directory.GetFiles(fbd.SelectedPath));

                for (int i = 0; i < files.Count; i++)
                {
                    stub.Text = GetDocText(files[i]);
                    tbFileCompare.Text += "Исходный файл похож на файл " + files[i] + " на  " +
                                          s.Compare(tbFile.Text, stub.Text).ToString() + "%\n";
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void hashSelectionMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = hashSelectionMethodComboBox.SelectedItem.ToString();
            s.selectionMethod = selectedValue;
        }
    }
}