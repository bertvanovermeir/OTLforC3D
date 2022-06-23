using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoD_OTLToolingGUI
{
    public partial class Home : Form
    {

        private bool subsetOK = false;
        private bool DWGOK = false;
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkNewDrawingFile(object sender, EventArgs e)
        {
            if (checkNewDrawing.Checked)
            {
                buttonDWG.Enabled = false;
                textBoxDWG.Enabled = false;
                DWGOK = true;
                checkExportButtonEnable();
            }
            else
            {
                buttonDWG.Enabled = true;
                textBoxDWG.Enabled = true;
                DWGOK = false;
                checkExportButtonEnable();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://opendata.apps.mow.vlaanderen.be/otltool/subset/ui/");
        }

        private void buttonSubset_Click(object sender, EventArgs e)
        {
            subsetOK = false;
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select a Subset";
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Database Files (*.db)|*.db|Database Files (*.db)|*.db";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxSubset.Text = fdlg.FileName;
                subsetOK = true;
                checkExportButtonEnable();
            }
        }

        private void buttonDWG_Click(object sender, EventArgs e)
        {
            DWGOK = false;
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select a DWG file";
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "DWG Files (*.dwg)|*.dwg|DWG Files (*.dwg)|*.dwg";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxDWG.Text = fdlg.FileName;
                DWGOK = true;
                checkExportButtonEnable();
            }
        }

        private void checkExportButtonEnable()
        {
            if(subsetOK && DWGOK)
                buttonInject.Enabled = true;
            else
                buttonInject.Enabled = false;
        }

        private async void buttonInject_Click(object sender, EventArgs e)
        {

            ApplicationHandler.Settings.WriteSetting("DWG_NEWFILE", checkNewDrawing.Checked.ToString());
            ApplicationHandler.Settings.WriteSetting("DWG_OVERWRITE", checkoverwrite.Checked.ToString());
            ApplicationHandler.Settings.WriteSetting("DWG_PATH", textBoxDWG.Text);
            ApplicationHandler.Settings.WriteSetting("SQL_PATH", textBoxSubset.Text);

            await ApplicationHandler.RunScript();
        }
    }
}
