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

        public static void Start()
        {
            

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
            // test for C3D availability

            string PFpath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\";
            if (Directory.Exists(PFpath + Settings.ReadSetting("DIR_ACAD")) && Directory.Exists(PFpath + Settings.ReadSetting("DIR_C3D")))
                Application.Run(new Home());
            else
                MessageBox.Show("Civil3D installation not found (supported versions " + Settings.ReadSetting("SUPPORTED_ACAD") +"", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            EditScriptPath();
            string strCmdText;
            string strPathConsole = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\" + Settings.ReadSetting("DIR_ACAD") + "\\accoreconsole.exe";
            string strPathScript = Application.StartupPath;
            strCmdText = " /i \"" + Settings.ReadSetting("DWG_PATH") + "\" /s \" " + strPathScript + "\\scripts\\acad.scr\" ";
            System.Diagnostics.Process.Start(strPathConsole, strCmdText);
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
