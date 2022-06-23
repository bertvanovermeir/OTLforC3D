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
            if (!CheckVersion())
                MessageBox.Show("A new version is available, it is recommended to update.", "New version available", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            string PFpath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\";
            if (Directory.Exists(PFpath + Settings.ReadSetting("DIR_ACAD")) && Directory.Exists(PFpath + Settings.ReadSetting("DIR_C3D")))
                Application.Run(new Home());
            else
                MessageBox.Show("Civil3D installation not found (supported versions " + Settings.ReadSetting("SUPPORTED_ACAD") +"", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        public static bool CheckVersion()
        {
            var downloadpath = System.IO.Path.GetTempPath() + "otlappversie\\";
            // create the folder if it does not exist
            Directory.CreateDirectory(downloadpath);
            string newestversion = "";
            // download the TTL file
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile("https://raw.githubusercontent.com/bertvanovermeir/OTL/master/OTLWizard/Data/version.dat", downloadpath + "version.dat");
                }
                string[] lines = File.ReadAllLines(downloadpath + "version.dat", System.Text.Encoding.UTF8);
                foreach (string item in lines)
                {
                    newestversion = item;
                }
                if (newestversion.Equals(currentversion))
                    return true;
                else
                    return false;
            }
            catch
            {
                return true;
            }
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
            string strPathConsole = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\" + Settings.ReadSetting("DIR_ACAD") + "\\accoreconsole.exe";
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
