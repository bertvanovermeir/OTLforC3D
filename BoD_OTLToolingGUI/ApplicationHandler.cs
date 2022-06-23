using BoD_OTLToolBoxGUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoD_OTLToolingGUI
{
    public static class ApplicationHandler
    {
        public static ApplicationSettings Settings;
        public static string currentversion = "0.1";
        public static void Start()
        {
                MessageBox.Show("This is an alpha version, regularly check for updates!", "ALPHA 0.1", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // import data from settings.dat
            Settings = new ApplicationSettings();
            bool success = Settings.Init("settings.dat");
            if (success)
                OpenGUI();
            else
                MessageBox.Show("Problem while reading application settings", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // start the home screen


        }

        private static void OpenGUI()
        {
            // check firstrun
            if (Settings.ReadSetting("FIRSTRUN").ToLower().Equals("true"))
                firstrun();

            // test for C3D availability

            string PFpath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Autodesk";
            if (Directory.Exists(PFpath + Settings.ReadSetting("DIR_ACAD")) && Directory.Exists(PFpath + Settings.ReadSetting("DIR_C3D")))
            {
                Settings.WriteSetting("DIR_INSTALL", PFpath);
            }                
            else
                {
                    MessageBox.Show("Civil3D installation not found (supported versions " + Settings.ReadSetting("SUPPORTED_ACAD") + "", "Minor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            Application.Run(new Home());
        }

        private static void firstrun()
        {
            Firstrun first = new Firstrun();
            first.ShowDialog();
        }

        public static async Task<bool> RunScript()
        {
            Loading loading = new Loading();
            loading.Show();
            try
            {
                await Task.Run(() => { ApplicationHandler.ExecAcad(); });
            }
            catch
            {
                MessageBox.Show("An error occured while injecting the DWG file.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            loading.Hide();
            return true;
        }

        public static void ExecAcad()
        {
            // adapt the path to the script for current user
            EditScriptPath();
            // check if new file is needed
            EditDWGPath();

            string strCmdText;
            string strPathConsole = Settings.ReadSetting("DIR_INSTALL") + Settings.ReadSetting("DIR_ACAD") + "\\accoreconsole.exe";
            string strPathScript = Application.StartupPath;
            strCmdText = " /i \"" + Settings.ReadSetting("DWG_PATH") + "\" /s \" " + strPathScript + "\\scripts\\acad.scr\" ";
            System.Diagnostics.Process.Start(strPathConsole, strCmdText);
        }

        private static void EditDWGPath()
        {
            if(Settings.ReadSetting("DWG_NEWFILE").ToLower().Equals("true"))
            {
                string fileToCopy = Application.StartupPath + "\\scripts\\otl_template_base.dwg";
                string destinationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\otl_template.dwg";
                Settings.WriteSetting("DWG_PATH", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\otl_template.dwg");
                File.Copy(fileToCopy, destinationDirectory,true);
            }
        }

        private static void EditScriptPath()
        {
            string strPathDLL = Application.StartupPath;
            string strReplaceDLL = Application.StartupPath + "\\BoD_OTLToolBox.dll";
            strReplaceDLL = strReplaceDLL.Replace("\\", "/");
            string text = File.ReadAllText(strPathDLL + "\\scripts\\acad_base.scr");
            text = text.Replace("[PATH]", strReplaceDLL);
            File.WriteAllText(strPathDLL + "\\scripts\\acad.scr", text);
        }
    }
}
